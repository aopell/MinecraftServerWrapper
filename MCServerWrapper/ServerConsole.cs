using fNbt;
using MCServerWrapper.Messages;
using MCServerWrapper.Plugins;
using MCServerWrapper.ServerWrapper;
using MCServerWrapper.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MCServerWrapper
{
    public partial class ServerConsole : Form, IServerConsole
    {
        public MinecraftServer Server { get; private set; }
        private PerformanceCounter memoryCounter = null;
        private PerformanceCounter cpuCounter = null;

        private static readonly Color red = Color.FromArgb(0xf4, 0x47, 0x47);
        private static readonly Color green = Color.FromArgb(0x98, 0xc3, 0x79);

        private readonly DropOutLinkedList<string> commandHistory = new DropOutLinkedList<string>(20);
        private LinkedListNode<string> currentHistoryItem = null;

        private readonly PluginLoader plugins = new PluginLoader("Plugins");

        private Config config;

        private Dictionary<string, string> playerPrefixes = new Dictionary<string, string>();

        public ServerConsole()
        {
            Server = null;
            InitializeComponent();

            config = ConfigFile.Load<Config>("config.json");
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            Invoke((MethodInvoker)delegate
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Minecraft Server Files (*.jar)|*.jar",
                    CheckFileExists = true
                };

                if ((Server == null && openFileDialog.ShowDialog() == DialogResult.OK) || (Server != null && !Server.Running))
                {
                    if (Server != null)
                    {
                        Server.Start();
                    }
                    else
                    {
                        Server = new MinecraftServer(openFileDialog.FileName, config.JvmArguments);
                        Server.StandardOutput += Server_StandardOutputTextReceived;
                        Server.StandardError += Server_StandardOutputTextReceived;
                        Server.Exited += Server_Exited;
                        Server.Started += Server_Started;
                        Server.Start();
                    }
                }
            });
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void StopServer() => Server?.Stop();

        /// <summary>
        /// Restarts the server
        /// </summary>
        public void RestartServer() => Server?.Restart();

        /// <summary>
        /// Adds a text line to the console display with the given foreground color
        /// </summary>
        /// <param name="text">The text to add to the console display</param>
        /// <param name="color">The foreground color of the text</param>
        /// <param name="time">The time to display for the message, uses <see cref="DateTime.Now"/> by default</param>
        /// <param name="displayTime">Whether or not to display the time when logging the message</param>
        public void DisplayLine(string text, Color color, DateTime? time = null, bool displayTime = true)
        {
            consoleView.SuspendLayout();
            consoleView.SelectionColor = color;
            consoleView.AppendText(displayTime ? $"{time ?? DateTime.Now:[HH:mm:ss]} {text}{Environment.NewLine}" : $"{text}{Environment.NewLine}");
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
                    DisplayLine(command, Color.FromArgb(0x56, 0xb6, 0xc2));
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
                    return red;
                case ServerChatMessage cm:
                    switch (cm.MessageType)
                    {
                        case ServerChatMessage.ChatMessageType.Command:
                        case ServerChatMessage.ChatMessageType.Say:
                            return Color.FromArgb(0xc6, 0x78, 0xdd);
                    }
                    return Color.FromArgb(0xd1, 0x9a, 0x66);
                case ServerConnectionMessage _:
                    return Color.FromArgb(0xe5, 0xc0, 0x7b);
                case ServerSuccessMessage _:
                    return green;
                default:
                    return Color.LightGray;
            }
        }

        /// <summary>
        /// Loads plugin DLL files from the Plugins directory
        /// </summary>
        private void LoadPlugins()
        {
            var result = plugins.Reload();

            if (result.dlls == 0 || result.loaded == 0 && result.failed == 0) DisplayLine("No plugins found", Color.LightGray);

            if (result.loaded > 0) DisplayLine($"Successfully loaded {result.loaded} plugins from {result.dlls} DLLs", green);
            foreach (IPlugin plugin in plugins.Enabled)
            {
                DisplayLine($"{plugin.Name}: {plugin.Description}", green);
            }

            if (result.failed > 0) DisplayLine($"Failed to load {result.failed} plugins", red);

        }

        private void LoadScoreboardData()
        {
            playerPrefixes.Clear();
            try
            {
                bool worldPropertyExists = Server.TryGetProperty("level-name", out string world);
                if (!worldPropertyExists) return;

                var teams = new NbtFile(Path.Combine(Server.WorkingDirectory, world, "data/scoreboard.dat")).RootTag.Get<NbtCompound>("data")?.Get<NbtList>("Teams");
                if (teams == null) return;

                foreach (NbtTag nbtTag in teams)
                {
                    var team = (NbtCompound)nbtTag;
                    string prefix = string.Join("", (object[])MinecraftTextElement.FromJson(team.Get("MemberNamePrefix").StringValue));
                    string suffix = string.Join("", (object[])MinecraftTextElement.FromJson(team.Get("MemberNameSuffix").StringValue));

                    var players = team.Get<NbtList>("Players");
                    if (players == null) continue;

                    foreach (NbtTag player in players)
                    {
                        playerPrefixes[$"{prefix}{player.StringValue}{suffix}"] = player.StringValue;
                    }
                }
            }
            catch (IOException) { }
            catch (Exception ex)
            {
                playerPrefixes.Clear();
                DisplayLine($"Error Loading Scoreboard Data: {ex}", red);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void Server_Started(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                DisplayLine("Starting server", green);
                statusToolStripMenuItem.Text = "Server Online";
                statusToolStripMenuItem.ForeColor = Color.Green;
                bool propertyExists = Server.TryGetProperty("max-players", out string maxPlayers);
                playersLabel.Text = propertyExists ? $"Players ({playerView.Items.Count}/{maxPlayers})" : $"Players ({playerView.Items.Count})";

                openToolStripMenuItem.Enabled = true;
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
                restartToolStripMenuItem.Enabled = true;
                switchToolStripMenuItem.Enabled = true;
                commandBox.Enabled = true;
                ramToolStripMenuItem.Visible = true;
                cpuToolStripMenuItem.Visible = true;

                detailsTimer.Enabled = true;

                bool worldPropertyExists = Server.TryGetProperty("level-name", out string world);
                string directory = new DirectoryInfo(Server.WorkingDirectory).Name;
                Text = worldPropertyExists ? $"Minecraft Server | {directory} | {world}" : $"Minecraft Server | {directory}";

                if (worldPropertyExists)
                {
                    scoreboardFileWatcher.Path = Path.Combine(Server.WorkingDirectory, world, "data");
                }

                plugins.OnStart(this);

                LoadScoreboardData();
            });
        }

        private void Server_Exited(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                DisplayLine("Server exited successfully", red);
                playerView.Items.Clear();
                statusToolStripMenuItem.Text = "Server Offline";
                statusToolStripMenuItem.ForeColor = red;
                playersLabel.Text = "Players";

                openToolStripMenuItem.Enabled = false;
                startToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
                restartToolStripMenuItem.Enabled = false;
                switchToolStripMenuItem.Enabled = false;
                commandBox.Enabled = false;
                ramToolStripMenuItem.Visible = false;
                cpuToolStripMenuItem.Visible = false;

                detailsTimer.Enabled = false;

                Text = "Minecraft Server | Server Offline";

                plugins.OnExit(this);
            });
        }

        private void Server_StandardOutputTextReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.Data == null) return;
                ServerMessage m = ServerMessageParser.DetermineMessageType(e.Data, playerPrefixes);
                DisplayLine(config.DisplayRawOutput ? m.RawText : m.Text, GetColor(m), displayTime: !config.DisplayRawOutput);

                switch (m)
                {
                    case ServerChatMessage chat:
                        plugins.OnChatMessage(this, chat);
                        switch (chat.MessageType)
                        {
                            case ServerChatMessage.ChatMessageType.Command:
                                break;
                        }
                        break;
                    case ServerSuccessMessage s:
                        plugins.OnSuccessMessage(this, s);
                        break;
                    case ServerErrorMessage err:
                        plugins.OnErrorMessage(this, err);
                        break;
                    case ServerConnectionMessage connection:
                        switch (connection.ConnectionType)
                        {
                            case ServerConnectionMessage.ServerConnectionType.Connect:
                                PlayerJoined(connection);
                                plugins.OnPlayerConnect(this, connection);
                                break;
                            case ServerConnectionMessage.ServerConnectionType.Disconnect:
                                PlayerLeft(connection);
                                plugins.OnPlayerDisconnect(this, connection);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        bool propertyExists = Server.TryGetProperty("max-players", out string maxPlayers);
                        playersLabel.Text = propertyExists ? $"Players ({playerView.Items.Count}/{maxPlayers})" : $"Players ({playerView.Items.Count})";

                        break;
                    default:
                        plugins.OnOtherMessage(this, m);
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
            StopServer();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartServer();
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
                case Keys.Enter:
                    SendCommand(commandBox.Text);
                    commandBox.Text = "";
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
            plugins.Unload();
            DisplayLine("Unloaded all plugins", Color.LightGray);
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("Plugins"))
            {
                Directory.CreateDirectory("Plugins");
            }

            Process.Start(Path.GetFullPath("Plugins"));
        }

        private void scoreboardFileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadScoreboardData();
        }
    }
}
