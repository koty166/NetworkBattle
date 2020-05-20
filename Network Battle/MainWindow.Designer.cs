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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.показатьСписокАдресовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.показатьСписокПерсонажейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.локальныйАдрессToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокСмертейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGLControl1 = new SharpGL.OpenGLControl();
            this.PersonImageMaker = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(484, 25);
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
            this.локальныйАдрессToolStripMenuItem,
            this.списокСмертейToolStripMenuItem});
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
            // списокСмертейToolStripMenuItem
            // 
            this.списокСмертейToolStripMenuItem.Name = "списокСмертейToolStripMenuItem";
            this.списокСмертейToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.списокСмертейToolStripMenuItem.Text = "Список смертей";
            this.списокСмертейToolStripMenuItem.Click += new System.EventHandler(this.списокСмертейToolStripMenuItem_Click);
            // 
            // openGLControl1
            // 
            this.openGLControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLControl1.DrawFPS = true;
            this.openGLControl1.FrameRate = 200;
            this.openGLControl1.Location = new System.Drawing.Point(0, 25);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl1.RenderContextType = SharpGL.RenderContextType.DIBSection;
            this.openGLControl1.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl1.Size = new System.Drawing.Size(484, 436);
            this.openGLControl1.TabIndex = 2;
            this.openGLControl1.OpenGLInitialized += new System.EventHandler(this.openGLControl1_OpenGLInitialized);
            this.openGLControl1.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl1_OpenGLDraw);
            this.openGLControl1.Resized += new System.EventHandler(this.openGLControl1_Resized);
            this.openGLControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.openGLControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.openGLControl1_MouseClick);
            // 
            // PersonImageMaker
            // 
            this.PersonImageMaker.Enabled = true;
            this.PersonImageMaker.Tick += new System.EventHandler(this.PersonImageMaker_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.openGLControl1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BalletWhoer_Tick1(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem показатьСписокАдресовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem показатьСписокПерсонажейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem локальныйАдрессToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокСмертейToolStripMenuItem;
        private SharpGL.OpenGLControl openGLControl1;
        private System.Windows.Forms.Timer PersonImageMaker;
    }
}

