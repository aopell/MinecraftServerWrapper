using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MCServerWrapper.ServerWrapper
{
    /// <summary>
    /// A wrapper for the Minecraft server process. Provides events and methods for indirect interface with the server process.
    /// </summary>
    public class MinecraftServer
    {
        /// <summary>
        /// Path to the server.jar file for this Minecraft server
        /// </summary>
        public string JarFilePath { get; }
        /// <summary>
        /// The current working directory, the directory that holds <see cref="JarFilePath"/>
        /// </summary>
        public string WorkingDirectory => ServerProcess.StartInfo.WorkingDirectory;
        /// <summary>
        /// The Minecraft server process
        /// </summary>
        public Process ServerProcess { get; private set; }

        /// <summary>
        /// Called when the server process starts
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Called when the server process exits
        /// </summary>
        public event EventHandler Exited;
        /// <summary>
        /// Called when a line is received from the standard output stream of the server process
        /// </summary>
        public event DataReceivedEventHandler StandardOutput;
        /// <summary>
        /// Called when a line is received from the standard error stream of the server process
        /// </summary>
        public event DataReceivedEventHandler StandardError;

        /// <summary>
        /// Key-value dictionary containing the options stored in the server's server.properties file
        /// </summary>
        private readonly Dictionary<string, string> serverProperties = new Dictionary<string, string>();

        /// <summary>
        /// Set of arguments passed to the JVM when the server process is started
        /// </summary>
        private readonly string jvmArgs;

        /// <summary>
        /// Identifies whether the server process is currently running
        /// </summary>
        public bool Running
        {
            get
            {
                if (ServerProcess == null) return false;
                try { Process.GetProcessById(ServerProcess.Id); }
                catch (InvalidOperationException) { return false; }
                catch (ArgumentException) { return false; }
                return true;
            }
        }

        /// <summary>
        /// Creates a new Minecraft server process wrapper
        /// </summary>
        /// <param name="jarFilePath">Path to the server.jar file</param>
        public MinecraftServer(string jarFilePath, string jvmArguments)
        {
            JarFilePath = jarFilePath;
            jvmArgs = jvmArguments;
        }

        /// <summary>
        /// Creates (but does not start) a new Minecraft server process and adds event handlers
        /// </summary>
        private void CreateServerProcess(Process existingProcess = null)
        {
            ServerProcess = existingProcess ?? new Process
            {
                StartInfo =
                {
                    WorkingDirectory = Path.GetDirectoryName(JarFilePath),
                    Arguments = $"{jvmArgs} -jar \"{JarFilePath}\" nogui",
                    FileName = "java",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            ServerProcess.Exited += ServerProcess_Exited;
            ServerProcess.OutputDataReceived += ServerProcess_OutputDataReceived;
            ServerProcess.ErrorDataReceived += ServerProcess_ErrorDataReceived;
        }

        private void ServerProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            StandardError?.Invoke(this, e);
        }

        private void ServerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            StandardOutput?.Invoke(this, e);
        }

        private void ServerProcess_Exited(object sender, EventArgs e)
        {
            Exited?.Invoke(this, e);
        }

        /// <summary>
        /// Starts the server process and starts the standard output/error stream if it has not already been started
        /// </summary>
        /// <exception cref="InvalidOperationException">Process is already running</exception>
        private void StartServerProcess()
        {
            if (Running) throw new InvalidOperationException("Can't start the server process when it is already running!");
            ServerProcess.Start();
            Started?.Invoke(this, EventArgs.Empty);

            try
            {
                ServerProcess.BeginOutputReadLine();
                ServerProcess.BeginErrorReadLine();
            }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// Attempts to load properties from the server.properties file into the <see cref="serverProperties"/> dictionary.
        /// Fails if the server.properties file does not exist
        /// </summary>
        private void TryLoadProperties()
        {
            serverProperties.Clear();
            string propertiesPath = Path.Combine(WorkingDirectory, "server.properties");
            if (!File.Exists(propertiesPath)) return;

            foreach (string line in File.ReadAllLines(propertiesPath))
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;

                string[] keyValue = line.Split('=');
                serverProperties[keyValue[0]] = keyValue[1];
            }
        }

        /// <summary>
        /// Returns whether a value with the provided key exists in the server's properties
        /// and returns that value through an out parameter if it exists
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <param name="value">Will be set to the value of the property with the provided key, should it exist, else null</param>
        public bool TryGetProperty(string key, out string value)
        {
            bool r = serverProperties.TryGetValue(key, out string v);
            value = v;
            return r;
        }

        /// <summary>
        /// Sets the value of a specified property if the key already exists
        /// </summary>
        /// <param name="key">The property to set</param>
        /// <param name="value">The new value of the property</param>
        /// <returns>Whether the operation was successful</returns>
        public bool TrySetProperty(string key, string value)
        {
            if (!serverProperties.ContainsKey(key)) return false;
            serverProperties[key] = value;
            return true;
        }

        /// <summary>
        /// Starts the Minecraft server process if it is not already running
        /// </summary>
        /// <exception cref="InvalidOperationException">Server is already running</exception>
        public void Start()
        {
            if (Running) throw new InvalidOperationException("Can't start server, it's already running!");
            CreateServerProcess();
            TryLoadProperties();
            StartServerProcess();
        }

        /// <summary>
        /// Stops the Minecraft server process if it is currently running
        /// </summary>
        /// <exception cref="InvalidOperationException">Server is not running</exception>
        public void Stop()
        {
            if (!Running) throw new InvalidOperationException("Can't stop server, the process is not running");
            SendCommand("stop");
        }

        /// <summary>
        /// Stops the Minecraft server process if it is currently running and then starts it again
        /// </summary>
        /// <exception cref="InvalidOperationException">Server is not running</exception>
        public void Restart()
        {
            if (!Running) throw new InvalidOperationException("Can't restart server, the process is not running");
            ServerProcess.Exited += (s, e) => Start();
            Stop();
        }

        /// <summary>
        /// Sends a command to the Minecraft server
        /// </summary>
        /// <param name="text">The command to send</param>
        /// <exception cref="InvalidOperationException">Server is not running</exception>
        public void SendCommand(string text)
        {
            if (!Running) throw new InvalidOperationException("Can't send a command, the process is not running");

            ServerProcess.StandardInput.WriteLine(text);
            ServerProcess.StandardInput.FlushAsync();
        }
    }
}
