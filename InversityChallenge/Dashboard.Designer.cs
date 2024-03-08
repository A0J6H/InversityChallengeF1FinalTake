namespace InversityChallenge
{
    partial class Dashboard
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
            Race_Select = new ComboBox();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // Race_Select
            // 
            Race_Select.BackColor = SystemColors.HighlightText;
            Race_Select.FlatStyle = FlatStyle.Flat;
            Race_Select.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Race_Select.FormattingEnabled = true;
            Race_Select.IntegralHeight = false;
            Race_Select.Location = new Point(452, 48);
            Race_Select.Name = "Race_Select";
            Race_Select.Size = new Size(287, 43);
            Race_Select.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(698, 12);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.mercedesf1logo;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(357, 106);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Location = new Point(12, 154);
            panel1.Name = "panel1";
            panel1.Size = new Size(382, 802);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Location = new Point(452, 154);
            panel2.Name = "panel2";
            panel2.Size = new Size(1435, 802);
            panel2.TabIndex = 4;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.dashboardbackgroundv2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1910, 981);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(Race_Select);
            DoubleBuffered = true;
            Name = "Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            Load += Dashboard_Load;
            Resize += Dashboard_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox Race_Select;
        private Label label1;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Panel panel2;
    }
}