namespace MCServerWrapper
{
    public partial class ServerConsole
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConsole));
            this.playersLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.commandBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whitelistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eULAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bannedPlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bannedIPsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.serverFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.worldFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.logsFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.datapacksFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setJVMArgumentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.unloadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleView = new System.Windows.Forms.RichTextBox();
            this.playerView = new System.Windows.Forms.ListBox();
            this.detailsTimer = new System.Windows.Forms.Timer(this.components);
            this.scoreboardFileWatcher = new System.IO.FileSystemWatcher();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scoreboardFileWatcher)).BeginInit();
            this.SuspendLayout();
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playersLabel.ForeColor = System.Drawing.Color.White;
            this.playersLabel.Location = new System.Drawing.Point(12, 30);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(57, 18);
            this.playersLabel.TabIndex = 2;
            this.playersLabel.Text = "Players";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(153, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Console";
            // 
            // commandBox
            // 
            this.commandBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.commandBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commandBox.Enabled = false;
            this.commandBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commandBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(182)))), ((int)(((byte)(194)))));
            this.commandBox.Location = new System.Drawing.Point(157, 540);
            this.commandBox.Name = "commandBox";
            this.commandBox.Size = new System.Drawing.Size(774, 19);
            this.commandBox.TabIndex = 4;
            this.commandBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.commandBox_KeyUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverToolStripMenuItem,
            this.openToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.cpuToolStripMenuItem,
            this.ramToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(943, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.switchToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "Server";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Enabled = false;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Enabled = false;
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // switchToolStripMenuItem
            // 
            this.switchToolStripMenuItem.Enabled = false;
            this.switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            this.switchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.switchToolStripMenuItem.Text = "Switch";
            this.switchToolStripMenuItem.Click += new System.EventHandler(this.switchToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opsToolStripMenuItem,
            this.whitelistToolStripMenuItem,
            this.eULAToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.bannedPlayersToolStripMenuItem,
            this.bannedIPsToolStripMenuItem,
            this.openFolderToolStripMenuItem1});
            this.openToolStripMenuItem.Enabled = false;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // opsToolStripMenuItem
            // 
            this.opsToolStripMenuItem.Name = "opsToolStripMenuItem";
            this.opsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.opsToolStripMenuItem.Text = "Ops";
            this.opsToolStripMenuItem.Click += new System.EventHandler(this.opsToolStripMenuItem_Click);
            // 
            // whitelistToolStripMenuItem
            // 
            this.whitelistToolStripMenuItem.Name = "whitelistToolStripMenuItem";
            this.whitelistToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.whitelistToolStripMenuItem.Text = "Whitelist";
            this.whitelistToolStripMenuItem.Click += new System.EventHandler(this.whitelistToolStripMenuItem_Click);
            // 
            // eULAToolStripMenuItem
            // 
            this.eULAToolStripMenuItem.Name = "eULAToolStripMenuItem";
            this.eULAToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.eULAToolStripMenuItem.Text = "EULA";
            this.eULAToolStripMenuItem.Click += new System.EventHandler(this.eULAToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // bannedPlayersToolStripMenuItem
            // 
            this.bannedPlayersToolStripMenuItem.Name = "bannedPlayersToolStripMenuItem";
            this.bannedPlayersToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.bannedPlayersToolStripMenuItem.Text = "Banned Players";
            this.bannedPlayersToolStripMenuItem.Click += new System.EventHandler(this.bannedPlayersToolStripMenuItem_Click);
            // 
            // bannedIPsToolStripMenuItem
            // 
            this.bannedIPsToolStripMenuItem.Name = "bannedIPsToolStripMenuItem";
            this.bannedIPsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.bannedIPsToolStripMenuItem.Text = "Banned IPs";
            this.bannedIPsToolStripMenuItem.Click += new System.EventHandler(this.bannedIPsToolStripMenuItem_Click);
            // 
            // openFolderToolStripMenuItem1
            // 
            this.openFolderToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverFolderToolStripMenuItem,
            this.worldFolderToolStripMenuItem1,
            this.logsFolderToolStripMenuItem1,
            this.datapacksFolderToolStripMenuItem1});
            this.openFolderToolStripMenuItem1.Name = "openFolderToolStripMenuItem1";
            this.openFolderToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.openFolderToolStripMenuItem1.Text = "Folder";
            // 
            // serverFolderToolStripMenuItem
            // 
            this.serverFolderToolStripMenuItem.Name = "serverFolderToolStripMenuItem";
            this.serverFolderToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.serverFolderToolStripMenuItem.Text = "Server Folder";
            this.serverFolderToolStripMenuItem.Click += new System.EventHandler(this.serverFolderToolStripMenuItem_Click);
            // 
            // worldFolderToolStripMenuItem1
            // 
            this.worldFolderToolStripMenuItem1.Name = "worldFolderToolStripMenuItem1";
            this.worldFolderToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.worldFolderToolStripMenuItem1.Text = "World Folder";
            this.worldFolderToolStripMenuItem1.Click += new System.EventHandler(this.worldFolderToolStripMenuItem1_Click);
            // 
            // logsFolderToolStripMenuItem1
            // 
            this.logsFolderToolStripMenuItem1.Name = "logsFolderToolStripMenuItem1";
            this.logsFolderToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.logsFolderToolStripMenuItem1.Text = "Logs Folder";
            this.logsFolderToolStripMenuItem1.Click += new System.EventHandler(this.logsFolderToolStripMenuItem1_Click);
            // 
            // datapacksFolderToolStripMenuItem1
            // 
            this.datapacksFolderToolStripMenuItem1.Name = "datapacksFolderToolStripMenuItem1";
            this.datapacksFolderToolStripMenuItem1.Size = new System.Drawing.Size(164, 22);
            this.datapacksFolderToolStripMenuItem1.Text = "Datapacks Folder";
            this.datapacksFolderToolStripMenuItem1.Click += new System.EventHandler(this.datapacksFolderToolStripMenuItem1_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.statusToolStripMenuItem.Text = "Server Offline";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // cpuToolStripMenuItem
            // 
            this.cpuToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cpuToolStripMenuItem.Name = "cpuToolStripMenuItem";
            this.cpuToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.cpuToolStripMenuItem.Text = "0% CPU";
            this.cpuToolStripMenuItem.Visible = false;
            // 
            // ramToolStripMenuItem
            // 
            this.ramToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ramToolStripMenuItem.Name = "ramToolStripMenuItem";
            this.ramToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.ramToolStripMenuItem.Text = "0 MB RAM";
            this.ramToolStripMenuItem.Visible = false;
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setJVMArgumentsToolStripMenuItem,
            this.pluginsToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // setJVMArgumentsToolStripMenuItem
            // 
            this.setJVMArgumentsToolStripMenuItem.Name = "setJVMArgumentsToolStripMenuItem";
            this.setJVMArgumentsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setJVMArgumentsToolStripMenuItem.Text = "Set JVM Arguments";
            this.setJVMArgumentsToolStripMenuItem.Click += new System.EventHandler(this.setJVMArgumentsToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem1
            // 
            this.pluginsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unloadAllToolStripMenuItem,
            this.reloadPluginsToolStripMenuItem,
            this.openFolderToolStripMenuItem});
            this.pluginsToolStripMenuItem1.Name = "pluginsToolStripMenuItem1";
            this.pluginsToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.pluginsToolStripMenuItem1.Text = "Plugins";
            // 
            // unloadAllToolStripMenuItem
            // 
            this.unloadAllToolStripMenuItem.Name = "unloadAllToolStripMenuItem";
            this.unloadAllToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.unloadAllToolStripMenuItem.Text = "Unload";
            this.unloadAllToolStripMenuItem.Click += new System.EventHandler(this.unloadAllToolStripMenuItem_Click);
            // 
            // reloadPluginsToolStripMenuItem
            // 
            this.reloadPluginsToolStripMenuItem.Name = "reloadPluginsToolStripMenuItem";
            this.reloadPluginsToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.reloadPluginsToolStripMenuItem.Text = "Reload";
            this.reloadPluginsToolStripMenuItem.Click += new System.EventHandler(this.reloadPluginsToolStripMenuItem_Click);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // consoleView
            // 
            this.consoleView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.consoleView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.consoleView.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleView.ForeColor = System.Drawing.Color.White;
            this.consoleView.Location = new System.Drawing.Point(157, 53);
            this.consoleView.Name = "consoleView";
            this.consoleView.ReadOnly = true;
            this.consoleView.Size = new System.Drawing.Size(774, 481);
            this.consoleView.TabIndex = 7;
            this.consoleView.Text = "";
            // 
            // playerView
            // 
            this.playerView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.playerView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(44)))), ((int)(((byte)(52)))));
            this.playerView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playerView.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerView.ForeColor = System.Drawing.Color.LightGray;
            this.playerView.FormattingEnabled = true;
            this.playerView.IntegralHeight = false;
            this.playerView.ItemHeight = 18;
            this.playerView.Location = new System.Drawing.Point(15, 53);
            this.playerView.Name = "playerView";
            this.playerView.Size = new System.Drawing.Size(136, 506);
            this.playerView.TabIndex = 8;
            // 
            // detailsTimer
            // 
            this.detailsTimer.Interval = 5000;
            this.detailsTimer.Tick += new System.EventHandler(this.detailsTimer_Tick);
            // 
            // scoreboardFileWatcher
            // 
            this.scoreboardFileWatcher.EnableRaisingEvents = true;
            this.scoreboardFileWatcher.Filter = "scoreboard.dat";
            this.scoreboardFileWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.scoreboardFileWatcher.SynchronizingObject = this;
            this.scoreboardFileWatcher.Changed += new System.IO.FileSystemEventHandler(this.scoreboardFileWatcher_Changed);
            // 
            // ServerConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(35)))), ((int)(((byte)(41)))));
            this.ClientSize = new System.Drawing.Size(943, 565);
            this.Controls.Add(this.playerView);
            this.Controls.Add(this.consoleView);
            this.Controls.Add(this.commandBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServerConsole";
            this.Text = "Minecraft Server | Server Offline";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerConsole_FormClosing);
            this.Load += new System.EventHandler(this.ServerConsole_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scoreboardFileWatcher)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label playersLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox commandBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whitelistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eULAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bannedPlayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bannedIPsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem serverFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem worldFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem logsFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem datapacksFolderToolStripMenuItem1;
        private System.Windows.Forms.RichTextBox consoleView;
        private System.Windows.Forms.ListBox playerView;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cpuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ramToolStripMenuItem;
        private System.Windows.Forms.Timer detailsTimer;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setJVMArgumentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem reloadPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unloadAllToolStripMenuItem;
        private System.IO.FileSystemWatcher scoreboardFileWatcher;
    }
}

