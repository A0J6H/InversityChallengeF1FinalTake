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
            components = new System.ComponentModel.Container();
            Race_Select = new ComboBox();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            LeaderBoardP = new Panel();
            DashP = new Panel();
            Test_Panel = new Panel();
            Choose_Graph = new Button();
            Session_Select = new ComboBox();
            Race_Timer = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            Layout_Button = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            DashP.SuspendLayout();
            Test_Panel.SuspendLayout();
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
            Race_Select.SelectedIndexChanged += Race_Select_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(1708, 9);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 1;
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
            // LeaderBoardP
            // 
            LeaderBoardP.Location = new Point(12, 154);
            LeaderBoardP.Name = "LeaderBoardP";
            LeaderBoardP.Size = new Size(382, 802);
            LeaderBoardP.TabIndex = 3;
            // 
            // DashP
            // 
            DashP.Controls.Add(Test_Panel);
            DashP.Location = new Point(452, 154);
            DashP.Name = "DashP";
            DashP.Size = new Size(1435, 800);
            DashP.TabIndex = 4;
            // 
            // Test_Panel
            // 
            Test_Panel.BorderStyle = BorderStyle.FixedSingle;
            Test_Panel.Controls.Add(Choose_Graph);
            Test_Panel.Location = new Point(1, 1);
            Test_Panel.Name = "Test_Panel";
            Test_Panel.Size = new Size(700, 400);
            Test_Panel.TabIndex = 0;
            // 
            // Choose_Graph
            // 
            Choose_Graph.Location = new Point(3, 3);
            Choose_Graph.Name = "Choose_Graph";
            Choose_Graph.Size = new Size(35, 29);
            Choose_Graph.TabIndex = 0;
            Choose_Graph.Text = "+";
            Choose_Graph.UseVisualStyleBackColor = true;
            Choose_Graph.Click += Choose_Graph_Click;
            // 
            // Session_Select
            // 
            Session_Select.BackColor = SystemColors.HighlightText;
            Session_Select.FlatStyle = FlatStyle.Flat;
            Session_Select.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Session_Select.FormattingEnabled = true;
            Session_Select.IntegralHeight = false;
            Session_Select.Location = new Point(855, 48);
            Session_Select.Name = "Session_Select";
            Session_Select.Size = new Size(287, 43);
            Session_Select.TabIndex = 5;
            // 
            // Race_Timer
            // 
            Race_Timer.Interval = 1000;
            // 
            // button1
            // 
            button1.Location = new Point(1793, 71);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 6;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // Layout_Button
            // 
            Layout_Button.Location = new Point(1548, 71);
            Layout_Button.Name = "Layout_Button";
            Layout_Button.Size = new Size(94, 29);
            Layout_Button.TabIndex = 7;
            Layout_Button.Text = "button2";
            Layout_Button.UseVisualStyleBackColor = true;
            Layout_Button.Click += Layout_Button_Click;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.dashboardbackgroundv2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1910, 981);
            Controls.Add(Layout_Button);
            Controls.Add(button1);
            Controls.Add(Session_Select);
            Controls.Add(DashP);
            Controls.Add(LeaderBoardP);
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
            DashP.ResumeLayout(false);
            Test_Panel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox Race_Select;
        private Label label1;
        private PictureBox pictureBox1;
        private Panel LeaderBoardP;
        private Panel DashP;
        private ComboBox Session_Select;
        private System.Windows.Forms.Timer Race_Timer;
        private Button button1;
        private Button Layout_Button;
        private Panel Test_Panel;
        private Button Choose_Graph;
    }
}