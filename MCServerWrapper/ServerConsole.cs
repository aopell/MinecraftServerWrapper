using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
using MCServerWrapper.ServerWrapper;
using MCServerWrapper.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MCServerWrapper
{
    public partial class ServerConsole : Form, IServerConsole
    {
        /// <summary>
        /// Provides access to the underlying server process
        /// </summary>
        public MinecraftServer Server { get; private set; }
        private PerformanceCounter memoryCounter = null;
        private PerformanceCounter cpuCounter = null;

        private readonly DropOutLinkedList<string> commandHistory = new DropOutLinkedList<string>(20);
        private LinkedListNode<string> currentHistoryItem = null;

        private readonly List<IPlugin> plugins = new List<IPlugin>();
        private Config config;

        public ServerConsole()
        {
            Server = null;
            InitializeComponent();

            config = File.Exists("config.json") ? JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json")) : new Config();
            config.FilePath = "config.json";
        }

        /// <summary>
        /// Adds a text line to the console display with the given foreground color
        /// </summary>
        /// <param name="text">The text to add to the console display</param>
        /// <param name="color">The foreground color of the text</param>
        /// <param name="time">The time to display for the message, uses <see cref="DateTime.Now"/> by default</param>
        public void DisplayLine(string text, Color color, DateTime? time = null)
        {
            consoleView.SuspendLayout();
            consoleView.SelectionColor = color;
            consoleView.AppendText($"{time ?? DateTime.Now:[HH:mm:ss]} {text}{Environment.NewLine}");
            consoleView.ScrollToCaret();
            consoleView.ResumeLayout();
        }

        /// <summary>
        /// Sends a command to the underlying server and optionally displays it in the console window and adds it to command history
        /// </summary>
        /// <param name="command">The command to send to the server</param>
        /// <param name="displayInConsole">Whether or not to display the sent command in the console</param>
        /// <param name="addToHistory">Whether or not to add this command to sent command history</param>
        public void SendCommand(string command, bool displayInConsole = true, bool addToHistory = true)
        {
            Invoke((MethodInvoker)delegate
            {
                if (Server == null) return;

                if (displayInConsole)
                {
                    DisplayLine(command, Color.Cyan);
                }

                Server.SendCommand(command);

                if (addToHistory)
                {
                    if (commandHistory.Last?.Value != command)
                    {
                        commandHistory.AddLast(command);
                    }

                    currentHistoryItem = null;
                }
            });
        }

        /// <summary>
        /// Returns the appropriate message foreground color for the subclass of <see cref="ServerMessage"/> given
        /// </summary>
        /// <param name="serverMessage">Message to determine the color of</param>
        private static Color GetColor(ServerMessage serverMessage)
        {
            switch (serverMessage)
            {
                case ServerErrorMessage _:
                    return Color.Red;
                case ServerChatMessage cm:
                    switch (cm.MessageType)
                    {
                        case ServerChatMessage.ChatMessageType.Command:
                        case ServerChatMessage.ChatMessageType.Say:
                            return Color.Magenta;
                    }
                    return Color.Orange;
                case ServerConnectionMessage _:
                    return Color.Yellow;
                case ServerSuccessMessage _:
                    return Color.FromArgb(0, 255, 0);
                default:
                    return Color.LightGray;
            }
        }

        /// <summary>
        /// Loads plugin DLL files from the Plugins directory
        /// </summary>
        private void LoadPlugins()
        {
            plugins.Clear();
            if (!Directory.Exists("Plugins"))
            {
                DisplayLine("No plugins found", Color.LightGray);
                return;
            }

            try
            {
                var dlls = Directory.GetFiles("Plugins").Where(x => Path.GetExtension(x) == ".dll").ToList();

                if (dlls.Count == 0)
                {
                    DisplayLine("No plugins found", Color.LightGray);
                }

                foreach (string dll in dlls)
                {
                    try
                    {
                        Assembly assembly = Assembly.Load(File.ReadAllBytes(Path.GetFullPath(dll)));
                        var pluginTypes = assembly.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x));
                        plugins.AddRange(pluginTypes.Select(Activator.CreateInstance).Cast<IPlugin>());
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        DisplayLine($"Error loading plugins from DLL \"{Path.GetFullPath(dll)}\": Invalid plugin format. Please note that your project must reference System.Windows.Forms for your plugins to load correctly.", Color.Red);
                    }
                }

                DisplayLine($"Loaded {plugins.Count} plugins from {dlls.Count} DLLs", Color.LightGray);
                foreach (IPlugin plugin in plugins)
                {
                    DisplayLine($"{plugin.Name}: {plugin.Description}", Color.FromArgb(0, 255, 0));
                }
            }
            catch (Exception ex)
            {
                DisplayLine($"Error loading plugins: {ex}", Color.Red);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Minecraft Server Files (*.jar)|*.jar",
                    CheckFileExists = true
                };

                if ((Server == null || !Server.Running) && openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Server = new MinecraftServer(openFileDialog.FileName, config.JvmArguments);
                    Server.StandardOutput += Server_StandardOutputTextReceived;
                    Server.StandardError += Server_StandardOutputTextReceived;
                    Server.Exited += Server_Exited;
                    Server.Started += Server_Started;
                    Server.Start();
                }
            });
        }

        private void Server_Started(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                DisplayLine("Starting server", Color.FromArgb(0, 255, 0));
                statusToolStripMenuItem.Text = "Server Online";
                statusToolStripMenuItem.ForeColor = Color.Green;
                bool propertyExists = Server.TryGetProperty("max-players", out string maxPlayers);
                playersLabel.Text = propertyExists ? $"Players ({playerView.Items.Count}/{maxPlayers})" : $"Players ({playerView.Items.Count})";

                openToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
                restartToolStripMenuItem.Enabled = true;
                switchToolStripMenuItem.Enabled = true;
                sendButton.Enabled = true;
                commandBox.Enabled = true;
                ramToolStripMenuItem.Visible = true;
                cpuToolStripMenuItem.Visible = true;

                detailsTimer.Enabled = true;

                bool worldPropertyExists = Server.TryGetProperty("level-name", out string world);
                string directory = new DirectoryInfo(Server.WorkingDirectory).Name;
                Text = worldPropertyExists ? $"Minecraft Server | {directory} | {world}" : $"Minecraft Server | {directory}";

                plugins.ForEach(x => x.OnStart(this));
            });
        }

        private void Server_Exited(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                DisplayLine("Server exited successfully", Color.Red);
                playerView.Items.Clear();
                statusToolStripMenuItem.Text = "Server Offline";
                statusToolStripMenuItem.ForeColor = Color.Red;
                playersLabel.Text = "Players";

                openToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
                restartToolStripMenuItem.Enabled = false;
                switchToolStripMenuItem.Enabled = false;
                sendButton.Enabled = false;
                commandBox.Enabled = false;
                ramToolStripMenuItem.Visible = false;
                cpuToolStripMenuItem.Visible = false;

                detailsTimer.Enabled = false;

                Text = "Minecraft Server | Server Offline";

                plugins.ForEach(x => x.OnExit());
            });
        }

        private void Server_StandardOutputTextReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.Data == null) return;
                ServerMessage m = ServerOutputParser.DetermineMessageType(e.Data);
                DisplayLine(m.Text, GetColor(m));

                switch (m)
                {
                    case ServerChatMessage chat:
                        plugins.ForEach(x => x.OnChatMessage(this, chat));
                        break;
                    case ServerSuccessMessage s:
                        plugins.ForEach(x => x.OnSuccessMessage(this, s));
                        break;
                    case ServerErrorMessage err:
                        plugins.ForEach(x => x.OnErrorMessage(this, err));
                        break;
                    case ServerConnectionMessage connection:
                        switch (connection.ConnectionType)
                        {
                            case ServerConnectionMessage.ServerConnectionType.Connect:
                                PlayerJoined(connection);
                                plugins.ForEach(x => x.OnPlayerConnect(this, connection));
                                break;
                            case ServerConnectionMessage.ServerConnectionType.Disconnect:
                                PlayerLeft(connection);
                                plugins.ForEach(x => x.OnPlayerDisconnect(this, connection));
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        bool propertyExists = Server.TryGetProperty("max-players", out string maxPlayers);
                        playersLabel.Text = propertyExists ? $"Players ({playerView.Items.Count}/{maxPlayers})" : $"Players ({playerView.Items.Count})";

                        break;
                    default:
                        plugins.ForEach(x => x.OnOtherMessage(this, m));
                        break;
                }
            });
        }

        private void PlayerJoined(ServerConnectionMessage connectionMessage)
        {
            if (!playerView.Items.Contains(connectionMessage.Username))
            {
                playerView.Items.Add(connectionMessage.Username);
            }
        }

        private void PlayerLeft(ServerConnectionMessage connectionMessage)
        {
            playerView.Items.Remove(connectionMessage.Username);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server?.Stop();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server?.Restart();
        }

        private void switchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Server.Exited += startToolStripMenuItem_Click;
            Server.Stop();
            Server = null;
        }

        private void opsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "ops.json"));
        }

        private void whitelistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "whitelist.json"));
        }

        private void eULAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "eula.txt"));
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "server.properties"));
        }

        private void bannedPlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "banned-players.json"));
        }

        private void bannedIPsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "banned-ips.json"));
        }

        private void serverFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Server.WorkingDirectory);
        }

        private void worldFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Server != null && Server.TryGetProperty("level-name", out string world))
            {
                Process.Start(Path.Combine(Server.WorkingDirectory, world));
            }
        }

        private void logsFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Server.WorkingDirectory, "logs"));
        }

        private void datapacksFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Server != null && Server.TryGetProperty("level-name", out string world))
            {
                Process.Start(Path.Combine(Server.WorkingDirectory, world, "datapacks"));
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            SendCommand(commandBox.Text);
            commandBox.Text = "";
        }

        private void detailsTimer_Tick(object sender, EventArgs e)
        {
            if (!Server.Running)
            {
                memoryCounter = null;
                cpuCounter = null;
                return;
            }

            if (memoryCounter == null)
            {
                memoryCounter = new PerformanceCounter("Process", "Working Set - Private", Server.ServerProcess.ProcessName, true);
            }

            if (cpuCounter == null)
            {
                cpuCounter = new PerformanceCounter("Process", "% Processor Time", Server.ServerProcess.ProcessName, true);
            }

            int memsize = (int)(memoryCounter.NextValue() / 1024 / 1024);
            float cpu = (int)(cpuCounter.NextValue() / Environment.ProcessorCount);

            cpuToolStripMenuItem.Text = $"{cpu}% CPU";
            ramToolStripMenuItem.Text = $"{memsize} MB RAM";
        }

        private void ServerConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Server == null || !Server.Running) return;

            switch (MessageBox.Show("Stop the server before exiting?", "Server Running", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    Server.Exited += (ss, ee) => Application.Exit();
                    e.Cancel = true;
                    Server.Stop();
                    break;
                case DialogResult.No:
                    break;
                default:
                    e.Cancel = true;
                    return;
            }
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Server == null || !Server.Running)
            {
                if (MessageBox.Show("Start the server?", "Start Server", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    startToolStripMenuItem_Click(sender, e);
                }
            }
            else
            {
                if (MessageBox.Show("Stop the server?", "Stop Server", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Server.Stop();
                }
            }
        }

        private void setJVMArgumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string jvmArguments = PromptDialog.Show("Enter JVM arguments. \"-jar {JarFilePath} nogui\" will be added to the end automatically.", "JVM Arguments", config.JvmArguments, true);
            if (jvmArguments == null) return;
            config.JvmArguments = jvmArguments;
            config.Save();
        }

        private void commandBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    currentHistoryItem = currentHistoryItem == null ? commandHistory.Last : (currentHistoryItem.Previous ?? currentHistoryItem);
                    if (currentHistoryItem != null) commandBox.Text = currentHistoryItem.Value;
                    commandBox.SelectionStart = commandBox.Text.Length;
                    commandBox.SelectionLength = 0;
                    break;
                case Keys.Down:
                    currentHistoryItem = currentHistoryItem?.Next;
                    commandBox.Text = currentHistoryItem == null ? "" : currentHistoryItem.Value;
                    commandBox.SelectionStart = commandBox.Text.Length;
                    commandBox.SelectionLength = 0;
                    break;
            }
        }

        private void reloadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private void ServerConsole_Load(object sender, EventArgs e)
        {
            LoadPlugins();
        }

        private void unloadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            plugins.Clear();
            DisplayLine("Unloaded all plugins", Color.LightGray);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Path.GetFullPath("Plugins"));
        }
    }
}
