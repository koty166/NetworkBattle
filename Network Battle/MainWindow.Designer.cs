namespace Network_Battle
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.BattleField = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.показатьСписокАдресовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьСписокПерсонажейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.локальныйАдрессToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.BattleField)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BattleField
            // 
            this.BattleField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BattleField.Image = ((System.Drawing.Image)(resources.GetObject("BattleField.Image")));
            this.BattleField.Location = new System.Drawing.Point(0, 0);
            this.BattleField.Name = "BattleField";
            this.BattleField.Size = new System.Drawing.Size(800, 450);
            this.BattleField.TabIndex = 0;
            this.BattleField.TabStop = false;
            this.BattleField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BattleField_MouseClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(78, 22);
            this.toolStripButton1.Text = "Начать игру";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.показатьСписокАдресовToolStripMenuItem,
            this.показатьСписокПерсонажейToolStripMenuItem,
            this.локальныйАдрессToolStripMenuItem});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(70, 22);
            this.toolStripButton2.Text = "Система";
            // 
            // показатьСписокАдресовToolStripMenuItem
            // 
            this.показатьСписокАдресовToolStripMenuItem.Name = "показатьСписокАдресовToolStripMenuItem";
            this.показатьСписокАдресовToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.показатьСписокАдресовToolStripMenuItem.Text = "Список адресов";
            this.показатьСписокАдресовToolStripMenuItem.Click += new System.EventHandler(this.СписокАдресовToolStripMenuItem_Click);
            // 
            // показатьСписокПерсонажейToolStripMenuItem
            // 
            this.показатьСписокПерсонажейToolStripMenuItem.Name = "показатьСписокПерсонажейToolStripMenuItem";
            this.показатьСписокПерсонажейToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.показатьСписокПерсонажейToolStripMenuItem.Text = "Список персонажей";
            this.показатьСписокПерсонажейToolStripMenuItem.Click += new System.EventHandler(this.показатьСписокПерсонажейToolStripMenuItem_Click);
            // 
            // локальныйАдрессToolStripMenuItem
            // 
            this.локальныйАдрессToolStripMenuItem.Name = "локальныйАдрессToolStripMenuItem";
            this.локальныйАдрессToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.локальныйАдрессToolStripMenuItem.Text = "Локальный адрес";
            this.локальныйАдрессToolStripMenuItem.Click += new System.EventHandler(this.локальныйАдрессToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.BattleField);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.BattleField)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BalletWhoer_Tick1(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        internal System.Windows.Forms.PictureBox BattleField;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem показатьСписокАдресовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьСписокПерсонажейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem локальныйАдрессToolStripMenuItem;
    }
}

