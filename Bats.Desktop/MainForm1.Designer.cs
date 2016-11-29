using System.Windows.Forms;

namespace Bats.Desktop
{
    partial class MainForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm1));
            this.listView = new System.Windows.Forms.ListView();
            this.team1Header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.team2Header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.handicapHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wnTotalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wnForaHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ErrorsPanel = new System.Windows.Forms.Panel();
            this.FetchCountLabel = new System.Windows.Forms.Label();
            this.CounterLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HistoryListView = new System.Windows.Forms.ListView();
            this.waitTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.refreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GamesMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ErrorsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.GamesMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.team1Header,
            this.team2Header,
            this.totalHeader,
            this.handicapHeader,
            this.wnTotalHeader,
            this.wnForaHeader});
            this.listView.ContextMenuStrip = this.GamesMenuStrip;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.Location = new System.Drawing.Point(0, 24);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(774, 394);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // team1Header
            // 
            this.team1Header.Text = "Команда 1";
            this.team1Header.Width = 125;
            // 
            // team2Header
            // 
            this.team2Header.Text = "Команда 2";
            this.team2Header.Width = 125;
            // 
            // totalHeader
            // 
            this.totalHeader.Text = "Тотал(FB)";
            this.totalHeader.Width = 120;
            // 
            // handicapHeader
            // 
            this.handicapHeader.Text = "Фора(FB)";
            this.handicapHeader.Width = 120;
            // 
            // wnTotalHeader
            // 
            this.wnTotalHeader.Text = "Тотал(WL)";
            this.wnTotalHeader.Width = 120;
            // 
            // wnForaHeader
            // 
            this.wnForaHeader.Text = "Фора(WL)";
            this.wnForaHeader.Width = 120;
            // 
            // ErrorsPanel
            // 
            this.ErrorsPanel.Controls.Add(this.FetchCountLabel);
            this.ErrorsPanel.Controls.Add(this.CounterLabel);
            this.ErrorsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ErrorsPanel.Location = new System.Drawing.Point(0, 385);
            this.ErrorsPanel.Name = "ErrorsPanel";
            this.ErrorsPanel.Size = new System.Drawing.Size(774, 33);
            this.ErrorsPanel.TabIndex = 1;
            // 
            // FetchCountLabel
            // 
            this.FetchCountLabel.AutoSize = true;
            this.FetchCountLabel.ForeColor = System.Drawing.Color.DarkGreen;
            this.FetchCountLabel.Location = new System.Drawing.Point(99, 9);
            this.FetchCountLabel.Name = "FetchCountLabel";
            this.FetchCountLabel.Size = new System.Drawing.Size(13, 13);
            this.FetchCountLabel.TabIndex = 2;
            this.FetchCountLabel.Text = "0";
            // 
            // CounterLabel
            // 
            this.CounterLabel.AutoSize = true;
            this.CounterLabel.Location = new System.Drawing.Point(12, 9);
            this.CounterLabel.Name = "CounterLabel";
            this.CounterLabel.Size = new System.Drawing.Size(65, 13);
            this.CounterLabel.TabIndex = 1;
            this.CounterLabel.Text = "Ожидание: ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.HistoryListView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(774, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 394);
            this.panel1.TabIndex = 2;
            // 
            // HistoryListView
            // 
            this.HistoryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistoryListView.Location = new System.Drawing.Point(0, 0);
            this.HistoryListView.Name = "HistoryListView";
            this.HistoryListView.Size = new System.Drawing.Size(200, 394);
            this.HistoryListView.TabIndex = 0;
            this.HistoryListView.UseCompatibleStateImageBehavior = false;
            this.HistoryListView.ItemActivate += new System.EventHandler(this.HistoryListView_ItemActivate);
            // 
            // waitTimer
            // 
            this.waitTimer.Interval = 1000;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(974, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // refreshMenuItem
            // 
            this.refreshMenuItem.Image = global::Bats.Desktop.Properties.Resources.Refresh;
            this.refreshMenuItem.Name = "refreshMenuItem";
            this.refreshMenuItem.Size = new System.Drawing.Size(28, 20);
            this.refreshMenuItem.Click += new System.EventHandler(this.refreshMenuItem_Click);
            // 
            // GamesMenuStrip
            // 
            this.GamesMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeMenuItem});
            this.GamesMenuStrip.Name = "contextMenuStrip1";
            this.GamesMenuStrip.Size = new System.Drawing.Size(119, 26);
            // 
            // removeMenuItem
            // 
            this.removeMenuItem.Image = global::Bats.Desktop.Properties.Resources.Delete;
            this.removeMenuItem.Name = "removeMenuItem";
            this.removeMenuItem.Size = new System.Drawing.Size(118, 22);
            this.removeMenuItem.Text = "Удалить";
            this.removeMenuItem.Click += new System.EventHandler(this.removeMenuItem_Click);
            // 
            // MainForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 418);
            this.Controls.Add(this.ErrorsPanel);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm1";
            this.Text = "Winline - Fonbet форы/тоталы";
            this.ErrorsPanel.ResumeLayout(false);
            this.ErrorsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GamesMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        public System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader team1Header;
        private System.Windows.Forms.ColumnHeader totalHeader;
        private System.Windows.Forms.ColumnHeader handicapHeader;
        private System.Windows.Forms.ColumnHeader wnTotalHeader;
        private System.Windows.Forms.ColumnHeader wnForaHeader;
        private System.Windows.Forms.Panel ErrorsPanel;
        public System.Windows.Forms.Label FetchCountLabel;
        public System.Windows.Forms.Label CounterLabel;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ListView HistoryListView;
        private System.Windows.Forms.Timer waitTimer;
        private System.Windows.Forms.ColumnHeader team2Header;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem refreshMenuItem;
        private ContextMenuStrip GamesMenuStrip;
        private ToolStripMenuItem removeMenuItem;
    }
}

