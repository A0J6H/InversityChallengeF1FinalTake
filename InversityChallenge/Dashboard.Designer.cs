﻿namespace InversityChallenge
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
            Test_Panel = new Panel();
            Choose_Graph = new Button();
            DashP = new Panel();
            Selected_Drivers_Check = new CheckedListBox();
            Session_Select = new ComboBox();
            Race_Timer = new System.Windows.Forms.Timer(components);
            Race_Start = new Button();
            Layout_Button = new Button();
            Graph_Panel = new Panel();
            Close_Graph = new Button();
            Bar_Chart = new Button();
            Pie_Chart = new Button();
            Line_Graph = new Button();
            Layout_Picker = new Panel();
            Layout2 = new PictureBox();
            Layout3 = new PictureBox();
            Layout4 = new PictureBox();
            Layout1 = new PictureBox();
            LeaderboardPanel = new FlowLayoutPanel();
            Lap_Counter = new Label();
            Resume = new Button();
            Pause = new Button();
            Settings_Panel = new Panel();
            Driver_Select = new Button();
            Close_Settings = new Button();
            button3 = new Button();
            panel1 = new Panel();
            button2 = new Button();
            radioButton1 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            Test_Panel.SuspendLayout();
            DashP.SuspendLayout();
            Graph_Panel.SuspendLayout();
            Layout_Picker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Layout2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Layout3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Layout4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Layout1).BeginInit();
            Settings_Panel.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Race_Select
            // 
            Race_Select.BackColor = SystemColors.HighlightText;
            Race_Select.FlatStyle = FlatStyle.Flat;
            Race_Select.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Race_Select.FormattingEnabled = true;
            Race_Select.IntegralHeight = false;
            Race_Select.Location = new Point(396, 36);
            Race_Select.Margin = new Padding(3, 2, 3, 2);
            Race_Select.Name = "Race_Select";
            Race_Select.Size = new Size(252, 36);
            Race_Select.TabIndex = 0;
            Race_Select.SelectedIndexChanged += Race_Select_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(1, 1);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.mercedesf1logo;
            pictureBox1.Location = new Point(47, 9);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(273, 80);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // Test_Panel
            // 
            Test_Panel.BorderStyle = BorderStyle.FixedSingle;
            Test_Panel.Controls.Add(Choose_Graph);
            Test_Panel.Location = new Point(367, 658);
            Test_Panel.Margin = new Padding(3, 2, 3, 2);
            Test_Panel.Name = "Test_Panel";
            Test_Panel.Size = new Size(613, 300);
            Test_Panel.TabIndex = 0;
            // 
            // Choose_Graph
            // 
            Choose_Graph.Location = new Point(3, 2);
            Choose_Graph.Margin = new Padding(3, 2, 3, 2);
            Choose_Graph.Name = "Choose_Graph";
            Choose_Graph.Size = new Size(31, 22);
            Choose_Graph.TabIndex = 0;
            Choose_Graph.Text = "+";
            Choose_Graph.UseVisualStyleBackColor = true;
            Choose_Graph.Click += Choose_Graph_Click;
            // 
            // DashP
            // 
            DashP.Controls.Add(Selected_Drivers_Check);
            DashP.Location = new Point(396, 116);
            DashP.Margin = new Padding(3, 2, 3, 2);
            DashP.Name = "DashP";
            DashP.Size = new Size(1256, 600);
            DashP.TabIndex = 4;
            // 
            // Selected_Drivers_Check
            // 
            Selected_Drivers_Check.FormattingEnabled = true;
            Selected_Drivers_Check.Location = new Point(272, 74);
            Selected_Drivers_Check.Name = "Selected_Drivers_Check";
            Selected_Drivers_Check.Size = new Size(471, 400);
            Selected_Drivers_Check.TabIndex = 17;
            Selected_Drivers_Check.SelectedIndexChanged += Selected_Drivers_Check_SelectedIndexChanged;
            // 
            // Session_Select
            // 
            Session_Select.BackColor = SystemColors.HighlightText;
            Session_Select.FlatStyle = FlatStyle.Flat;
            Session_Select.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Session_Select.FormattingEnabled = true;
            Session_Select.IntegralHeight = false;
            Session_Select.Location = new Point(748, 36);
            Session_Select.Margin = new Padding(3, 2, 3, 2);
            Session_Select.Name = "Session_Select";
            Session_Select.Size = new Size(252, 36);
            Session_Select.TabIndex = 5;
            Session_Select.SelectedIndexChanged += Session_Select_SelectedIndexChanged;
            // 
            // Race_Timer
            // 
            Race_Timer.Interval = 3000;
            Race_Timer.Tick += Race_Timer_Tick;
            // 
            // Race_Start
            // 
            Race_Start.Enabled = false;
            Race_Start.Location = new Point(1528, 46);
            Race_Start.Margin = new Padding(3, 2, 3, 2);
            Race_Start.Name = "Race_Start";
            Race_Start.Size = new Size(82, 22);
            Race_Start.TabIndex = 6;
            Race_Start.Text = "button1";
            Race_Start.UseVisualStyleBackColor = true;
            Race_Start.Click += Race_Start_Click;
            // 
            // Layout_Button
            // 
            Layout_Button.Location = new Point(1168, 45);
            Layout_Button.Margin = new Padding(3, 2, 3, 2);
            Layout_Button.Name = "Layout_Button";
            Layout_Button.Size = new Size(82, 22);
            Layout_Button.TabIndex = 7;
            Layout_Button.Text = "Layout";
            Layout_Button.UseVisualStyleBackColor = true;
            Layout_Button.Click += Layout_Button_Click;
            // 
            // Graph_Panel
            // 
            Graph_Panel.BorderStyle = BorderStyle.FixedSingle;
            Graph_Panel.Controls.Add(Close_Graph);
            Graph_Panel.Controls.Add(Bar_Chart);
            Graph_Panel.Controls.Add(Pie_Chart);
            Graph_Panel.Controls.Add(Line_Graph);
            Graph_Panel.Location = new Point(1038, 12);
            Graph_Panel.Name = "Graph_Panel";
            Graph_Panel.Size = new Size(101, 90);
            Graph_Panel.TabIndex = 8;
            Graph_Panel.Visible = false;
            Graph_Panel.MouseEnter += Graph_Panel_MouseEnter;
            Graph_Panel.MouseLeave += Graph_Panel_MouseLeave;
            // 
            // Close_Graph
            // 
            Close_Graph.BackColor = Color.Brown;
            Close_Graph.FlatStyle = FlatStyle.Flat;
            Close_Graph.Font = new Font("Segoe UI", 5.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Close_Graph.Location = new Point(80, 4);
            Close_Graph.Name = "Close_Graph";
            Close_Graph.Size = new Size(15, 15);
            Close_Graph.TabIndex = 14;
            Close_Graph.Text = "x";
            Close_Graph.UseVisualStyleBackColor = false;
            Close_Graph.Click += Close_Graph_Click;
            // 
            // Bar_Chart
            // 
            Bar_Chart.Location = new Point(3, 32);
            Bar_Chart.Name = "Bar_Chart";
            Bar_Chart.Size = new Size(93, 23);
            Bar_Chart.TabIndex = 2;
            Bar_Chart.Text = "Bar Chart";
            Bar_Chart.UseVisualStyleBackColor = true;
            Bar_Chart.Click += Bar_Chart_Click;
            // 
            // Pie_Chart
            // 
            Pie_Chart.Location = new Point(3, 60);
            Pie_Chart.Name = "Pie_Chart";
            Pie_Chart.Size = new Size(93, 23);
            Pie_Chart.TabIndex = 1;
            Pie_Chart.Text = "Pie Chart";
            Pie_Chart.UseVisualStyleBackColor = true;
            Pie_Chart.Click += Pie_Chart_Click;
            // 
            // Line_Graph
            // 
            Line_Graph.Location = new Point(3, 4);
            Line_Graph.Name = "Line_Graph";
            Line_Graph.Size = new Size(74, 23);
            Line_Graph.TabIndex = 0;
            Line_Graph.Text = "Line Graph";
            Line_Graph.UseVisualStyleBackColor = true;
            Line_Graph.Click += Line_Graph_Click;
            // 
            // Layout_Picker
            // 
            Layout_Picker.BackColor = Color.FromArgb(143, 246, 235);
            Layout_Picker.BorderStyle = BorderStyle.FixedSingle;
            Layout_Picker.Controls.Add(Layout2);
            Layout_Picker.Controls.Add(Layout3);
            Layout_Picker.Controls.Add(Layout4);
            Layout_Picker.Controls.Add(Layout1);
            Layout_Picker.Location = new Point(1256, 9);
            Layout_Picker.Name = "Layout_Picker";
            Layout_Picker.Size = new Size(223, 223);
            Layout_Picker.TabIndex = 9;
            Layout_Picker.Visible = false;
            // 
            // Layout2
            // 
            Layout2.Image = Properties.Resources.Layout2b;
            Layout2.Location = new Point(116, 111);
            Layout2.Name = "Layout2";
            Layout2.Size = new Size(97, 97);
            Layout2.SizeMode = PictureBoxSizeMode.StretchImage;
            Layout2.TabIndex = 3;
            Layout2.TabStop = false;
            Layout2.Click += Layout2_Click;
            // 
            // Layout3
            // 
            Layout3.Image = Properties.Resources.Layout3b;
            Layout3.Location = new Point(13, 111);
            Layout3.Name = "Layout3";
            Layout3.Size = new Size(97, 97);
            Layout3.SizeMode = PictureBoxSizeMode.StretchImage;
            Layout3.TabIndex = 2;
            Layout3.TabStop = false;
            Layout3.Click += Layout3_Click;
            // 
            // Layout4
            // 
            Layout4.Image = Properties.Resources.Layout4b;
            Layout4.Location = new Point(116, 8);
            Layout4.Name = "Layout4";
            Layout4.Size = new Size(97, 97);
            Layout4.SizeMode = PictureBoxSizeMode.StretchImage;
            Layout4.TabIndex = 1;
            Layout4.TabStop = false;
            Layout4.Click += Layout4_Click;
            // 
            // Layout1
            // 
            Layout1.Image = Properties.Resources.Layout1b;
            Layout1.Location = new Point(13, 8);
            Layout1.Name = "Layout1";
            Layout1.Size = new Size(97, 97);
            Layout1.SizeMode = PictureBoxSizeMode.StretchImage;
            Layout1.TabIndex = 0;
            Layout1.TabStop = false;
            Layout1.Click += pictureBox2_Click;
            // 
            // LeaderboardPanel
            // 
            LeaderboardPanel.AutoScroll = true;
            LeaderboardPanel.FlowDirection = FlowDirection.TopDown;
            LeaderboardPanel.Location = new Point(12, 116);
            LeaderboardPanel.Name = "LeaderboardPanel";
            LeaderboardPanel.Size = new Size(338, 600);
            LeaderboardPanel.TabIndex = 10;
            LeaderboardPanel.WrapContents = false;
            // 
            // Lap_Counter
            // 
            Lap_Counter.AutoSize = true;
            Lap_Counter.Location = new Point(156, 98);
            Lap_Counter.Name = "Lap_Counter";
            Lap_Counter.Size = new Size(38, 15);
            Lap_Counter.TabIndex = 11;
            Lap_Counter.Text = "label2";
            // 
            // Resume
            // 
            Resume.Location = new Point(223, 93);
            Resume.Margin = new Padding(3, 2, 3, 2);
            Resume.Name = "Resume";
            Resume.Size = new Size(27, 22);
            Resume.TabIndex = 12;
            Resume.Text = "button1";
            Resume.UseVisualStyleBackColor = true;
            Resume.Click += Resume_Click;
            // 
            // Pause
            // 
            Pause.Location = new Point(256, 93);
            Pause.Margin = new Padding(3, 2, 3, 2);
            Pause.Name = "Pause";
            Pause.Size = new Size(27, 22);
            Pause.TabIndex = 13;
            Pause.Text = "button2";
            Pause.UseVisualStyleBackColor = true;
            Pause.Click += Pause_Click;
            // 
            // Settings_Panel
            // 
            Settings_Panel.BorderStyle = BorderStyle.FixedSingle;
            Settings_Panel.Controls.Add(Driver_Select);
            Settings_Panel.Controls.Add(Close_Settings);
            Settings_Panel.Controls.Add(button3);
            Settings_Panel.Location = new Point(1038, 108);
            Settings_Panel.Name = "Settings_Panel";
            Settings_Panel.Size = new Size(101, 62);
            Settings_Panel.TabIndex = 14;
            Settings_Panel.Visible = false;
            // 
            // Driver_Select
            // 
            Driver_Select.Location = new Point(3, 33);
            Driver_Select.Name = "Driver_Select";
            Driver_Select.Size = new Size(92, 23);
            Driver_Select.TabIndex = 15;
            Driver_Select.Text = "Drivers";
            Driver_Select.UseVisualStyleBackColor = true;
            Driver_Select.Click += Driver_Select_Click;
            // 
            // Close_Settings
            // 
            Close_Settings.BackColor = Color.Brown;
            Close_Settings.FlatStyle = FlatStyle.Flat;
            Close_Settings.Font = new Font("Segoe UI", 5.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Close_Settings.Location = new Point(80, 4);
            Close_Settings.Name = "Close_Settings";
            Close_Settings.Size = new Size(15, 15);
            Close_Settings.TabIndex = 14;
            Close_Settings.Text = "x";
            Close_Settings.UseVisualStyleBackColor = false;
            Close_Settings.Click += Close_Settings_Click;
            // 
            // button3
            // 
            button3.Location = new Point(3, 4);
            button3.Name = "button3";
            button3.Size = new Size(73, 23);
            button3.TabIndex = 1;
            button3.Text = "DataPoint";
            button3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(button2);
            panel1.Location = new Point(1145, 108);
            panel1.Name = "panel1";
            panel1.Size = new Size(128, 120);
            panel1.TabIndex = 16;
            // 
            // button2
            // 
            button2.BackColor = Color.Lime;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 5.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.Location = new Point(103, 4);
            button2.Name = "button2";
            button2.Size = new Size(15, 15);
            button2.TabIndex = 14;
            button2.Text = "x";
            button2.UseVisualStyleBackColor = false;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(748, 83);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(94, 19);
            radioButton1.TabIndex = 18;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.dashboardbackgroundv2;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1661, 724);
            Controls.Add(panel1);
            Controls.Add(radioButton1);
            Controls.Add(Settings_Panel);
            Controls.Add(Pause);
            Controls.Add(Resume);
            Controls.Add(Lap_Counter);
            Controls.Add(LeaderboardPanel);
            Controls.Add(Test_Panel);
            Controls.Add(Layout_Picker);
            Controls.Add(Graph_Panel);
            Controls.Add(Race_Start);
            Controls.Add(Layout_Button);
            Controls.Add(Session_Select);
            Controls.Add(DashP);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(Race_Select);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            Load += Dashboard_Load;
            Resize += Dashboard_Resize;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            Test_Panel.ResumeLayout(false);
            DashP.ResumeLayout(false);
            Graph_Panel.ResumeLayout(false);
            Layout_Picker.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Layout2).EndInit();
            ((System.ComponentModel.ISupportInitialize)Layout3).EndInit();
            ((System.ComponentModel.ISupportInitialize)Layout4).EndInit();
            ((System.ComponentModel.ISupportInitialize)Layout1).EndInit();
            Settings_Panel.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox Race_Select;
        private Label label1;
        private PictureBox pictureBox1;
        private Panel DashP;
        private ComboBox Session_Select;
        private System.Windows.Forms.Timer Race_Timer;
        private Button Race_Start;
        private Button Layout_Button;
        private Panel Test_Panel;
        private Button Choose_Graph;
        private Panel Graph_Panel;
        private Button Bar_Chart;
        private Button Pie_Chart;
        private Button Line_Graph;
        private Panel Layout_Picker;
        private PictureBox Layout1;
        private PictureBox Layout2;
        private PictureBox Layout3;
        private PictureBox Layout4;
        private FlowLayoutPanel LeaderboardPanel;
        private Label Lap_Counter;
        private Button Resume;
        private Button Pause;
        private Button Close_Graph;
        private Panel Settings_Panel;
        private Button Driver_Select;
        private Button Close_Settings;
        private Button button3;
        private Panel panel1;
        private CheckedListBox Selected_Drivers_Check;
        private Button button2;
        private RadioButton radioButton1;
    }
}