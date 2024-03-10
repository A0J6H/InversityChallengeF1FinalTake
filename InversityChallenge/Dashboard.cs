using C1.Chart;
using C1.Util.DX.WIC;
using C1.Win.Chart;
using InversityChallenge.JsonObjs;
using InversityF1.JsonObjs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static C1.Win.Design.DesignerNames;

namespace InversityChallenge
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        //Public Lists To Contain the Data//
        List<Meetings> Meetings;
        List<Sessions> Sessions;
        List<Drivers> Drivers;
        List<Intervals> Intervals;
        List<Stints> Stints;
        List<List<Laps>> Laps = new List<List<Laps>>();
        List<List<Car_Data>> Car_Data = new List<List<Car_Data>>();

        string Resources_Path = Path.Combine(Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName, "Resources"); //Gets the Path to the resources folder

        string Layout = "1";
        int Current_Lap = 0;
        int Total_Laps;

        List<Panel> DashPanels = new List<Panel>();
        List<List<C1.Win.Chart.FlexChart>> FlexCharts = new List<List<C1.Win.Chart.FlexChart>>();
        List<List<string>> Selected_Drivers = new List<List<string>>();
        List<ChartType> Chart_Types = new List<ChartType>() {ChartType.Line,ChartType.Bar};
        List<List<string>> Data_Types = new List<List<string>>();

        string Selected_Session_Key;

        private Panel Add_Driver_Panel(string Driver_Number,Panel Add_Panel)
        {
            Panel panel = new Panel();
            panel.Size = new Size(120, 30);
            Add_Panel.Controls.Add(panel);
            return panel;
        }
        private Label Add_Driver_Label(string Driver_Name)
        {
            Label label = new Label();
            string fullName = Driver_Name;
            string[] parts = fullName.Split(' ');
            string lastName = parts[parts.Length - 1];
            label.Text = lastName;
            label.Location = new Point(0, 10);
            return label;
        }

        private Button Add_Driver_Button(string Driver_Number)
        {
            Button Add_Driver = new Button();
            Add_Driver.Name = Driver_Number;
            Add_Driver.Size = new Size(15, 15);
            Add_Driver.Location = new Point(100, 10);
            Add_Driver.BackColor = Color.Red;
            Add_Driver.FlatStyle = FlatStyle.Flat;
            Add_Driver.Click += Add_Driver_Click;
            return Add_Driver;
        }

        private void Add_Driver_Click(object? sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panel = button.Parent.Parent.Parent.Parent as Panel;
            if (button.BackColor == Color.Red)
            {
                button.BackColor = Color.Green;
                Selected_Drivers[int.Parse(panel.Name)].Add(button.Name);
            }
            else
            {
                button.BackColor = Color.Red;
                Selected_Drivers[int.Parse(panel.Name)].RemoveAll(Driver_Number => Driver_Number == button.Name);
                //numbers.RemoveAll(x => x == 10);
            }
        }
        private Button Add_Data_Button(string Name)
        {
            Button Add_Data = new Button();
            Add_Data.Name = Name;
            Add_Data.Size = new Size(15, 15);
            Add_Data.Location = new Point(100, 10);
            Add_Data.BackColor = Color.Red;
            Add_Data.FlatStyle = FlatStyle.Flat;
            Add_Data.Click += Add_Data_Click;
            return Add_Data;
        }

        private void Add_Data_Click(object? sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panel = button.Parent.Parent.Parent.Parent as Panel;
            if(button.BackColor == Color.Red)
            {
                Data_Types[int.Parse(panel.Name)].Clear();
                Data_Types[int.Parse(panel.Name)].Add(button.Name);
                for(int i = 0; i < button.Parent.Parent.Controls.Count; i++)
                {
                    Panel panel1 = button.Parent.Parent.Controls[i] as Panel;
                    panel1.Controls[1].BackColor = Color.Red;
                }
                button.BackColor = Color.Green;
            }
        }

        public class laptimes
        {
            public string drivernumber { get; set; }
            public int lapnumber { get; set; }
            public double laptime { get; set; }

            public laptimes(string Drivernumber, int Lapnumber, double Laptime)
            {
                this.drivernumber = Drivernumber;
                this.lapnumber = Lapnumber;
                this.laptime = Laptime;
            }
        }

        static List<List<laptimes>> Lap_Time_Chart(List<List<Laps>> Laps, int current_lap, List<string> Driver_selection)
        {

            List<List<laptimes>> Lap_Times = new List<List<laptimes>>();
            List<Laps> Temp_Lap = new List<Laps>();
            for (int d = 0; d < Driver_selection.Count; d++)
            {
                List<laptimes> Lap_Time = new List<laptimes>();
                for (int l = 0; l < Laps.Count(); l++)
                {
                    try
                    {
                        if (Laps[l][0].driver_number.ToString() == Driver_selection[d].ToString())
                        {
                            Temp_Lap = Laps[l];
                        }
                    }
                    catch { }

                }
                for (int i = 0; i < current_lap; i++)
                {
                    if (Temp_Lap[i].lap_duration == null || Temp_Lap[i].is_pit_out_lap == true)
                    {

                    }
                    else
                    {
                        Lap_Time.Add(new laptimes(Temp_Lap[i].driver_number.ToString(), Temp_Lap[i].lap_number, double.Parse(Temp_Lap[i].lap_duration.ToString())) { });
                    }
                }
                Lap_Times.Add(Lap_Time);
            }
            return Lap_Times;
        }

        public class Speed
        {
            public string drivernumber { get; set; }
            public double time { get; set; }
            public double speed { get; set; }

            public Speed(string Drivernumber, double Time, double Speed)
            {
                this.drivernumber = Drivernumber;
                this.time = Time;
                this.speed = Speed;
            }
        }

        static List<List<Speed>> Speed_Chart(List<List<Car_Data>> Car_Data, List<List<Laps>> Lap_Info, int Current_Lap, List<string> Driver_Selection)
        {
            List<List<Speed>> Speeds = new List<List<Speed>>();
            
            for (int d = 0; d < Driver_Selection.Count; d++)
            {
                DateTime Start = new DateTime();
                DateTime End = new DateTime();
                List<Speed> Temp_Speeds = new List<Speed>();
                int c = 0;
                for(int k = 0;  k < Lap_Info.Count(); k++)
                {
                    for(int l = 0; l < Lap_Info[k].Count(); l++)
                    {
                        if (Driver_Selection[d] == Lap_Info[k][l].driver_number.ToString() && Current_Lap.ToString() == Lap_Info[k][l].lap_number.ToString())
                        {
                            
                            Start = DateTime.Parse(Lap_Info[k][l].date_start.ToString());
                            End = Start.AddMilliseconds(double.Parse(Lap_Info[k][l].lap_duration.ToString())*1000);
                        }
                    }
                }

                for(int i = 0; i < Car_Data.Count(); i++)
                {
                    for(int j = 0; j < Car_Data[i].Count(); j++)
                    {
                        if (Car_Data[i][j].driver_number.ToString() == Driver_Selection[d].ToString() && Car_Data[i][j].date>Start && Car_Data[i][j].date < End)
                        {
                            c++;
                            Temp_Speeds.Add(new Speed(Car_Data[i][j].driver_number.ToString(),c, Car_Data[i][j].speed));
                        }
                    }
                }

                Speeds.Add(Temp_Speeds);
            }
            return Speeds;
        }

        public class Stint
        {
            public int laps { get; set; }
            public string compound { get; set; }

            public Stint(int Laps,string Compound)
            {
                this.laps = Laps;
                this.compound = Compound;
            }
        }

        static List<Stint> Stint_Chart(List<Stints> stints,int Current_Lap)
        {
            List<Stint> Stints = new List<Stint>();
            int Soft = 0;
            int SC = 0;
            int Medium = 0;
            int MC = 0;
            int Hard = 0;
            int HC = 0;
            for(int i = 0; i < stints.Count; i++)
            {
                if (stints[i].lap_start > Current_Lap) { }
                else
                {
                    string Compound = stints[i].compound;

                    switch (Compound)
                    {
                        case "SOFT":
                            if (stints[i].lap_end > Current_Lap) 
                            {
                                SC++;
                                Soft += Current_Lap - stints[i].lap_start;
                            }
                            else
                            {
                                SC++;
                                Soft += stints[i].lap_end - stints[i].lap_start;
                            }
                            break;
                        case "MEDIUM":
                            if (stints[i].lap_end > Current_Lap)
                            {
                                MC++;
                                Medium += Current_Lap - stints[i].lap_start;
                            }
                            else
                            {
                                MC++;
                                Medium += stints[i].lap_end - stints[i].lap_start;
                            }
                            break;
                        case "HARD":
                            if (stints[i].lap_end > Current_Lap)
                            {
                                HC++;
                                Hard += Current_Lap - stints[i].lap_start;
                            }
                            else
                            {
                                HC++;
                                Hard += stints[i].lap_end - stints[i].lap_start;
                            }
                            break;
                    }
                }
                
            }
            Stints.Add(new Stint(Soft / SC, "Soft"));
            Stints.Add(new Stint(Medium / MC, "Medium"));
            Stints.Add(new Stint(Hard / HC, "Hard"));
            return Stints;
            
        }


            private void Update_Chart(int Index)
        {
            if (FlexCharts[Index][0].ChartType == ChartType.LineSymbols)
            {
                if (Data_Types[Index][0] == "Lap_Times")
                {
                    FlexCharts[Index][0].Series.Clear();

                    //Selecting chart's type 
                    this.FlexCharts[Index][0].ChartType = C1.Chart.ChartType.LineSymbols;

                    //Setting chart's Header and styling it
                    this.FlexCharts[Index][0].Header.Content = "Lap Times";
                    this.FlexCharts[Index][0].AxisX.Title = "Lap Times (Seconds)";
                    this.FlexCharts[Index][0].AxisY.Title = "Lap Number";

                    //Adding a Series to chart and binding it (AxisY) to 'Revenue' field of DataCollection
                    for (int j = 0; j < Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index]).Count(); j++)
                    {
                        C1.Win.Chart.Series series = new C1.Win.Chart.Series();
                        series.DataSource = Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index])[j];
                        series.Name = $"{Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index])[j][0].drivernumber}";
                        series.Binding = "laptime";
                        series.ChartType = C1.Chart.ChartType.LineSymbols;
                        FlexCharts[Index][0].Series.Add(series);
                    }

                    //this.FlexCharts[i].Series.Add(new C1.Win.Chart.Series
                    //{
                    //Name property specifies the string to be displayed corresponding to this Series in Legend
                    //    Name = "LapTimes",
                    //    Binding = "laptime"
                    //});
                    //Binding chart's AxisX to 'Date' so Dates are shown in Horizontal axis
                    this.FlexCharts[Index][0].BindingX = "lapnumber";
                }
                else if (Data_Types[Index][0] == "Speed")
                {
                    FlexCharts[Index][0].Series.Clear();

                    //Selecting chart's type 
                    this.FlexCharts[Index][0].ChartType = C1.Chart.ChartType.LineSymbols;

                    //Setting chart's Header and styling it
                    this.FlexCharts[Index][0].Header.Content = "Car Speed";
                    this.FlexCharts[Index][0].AxisX.Title = "Car Speed (km)";
                    this.FlexCharts[Index][0].AxisY.Title = "Data Points";

                    //Adding a Series to chart and binding it (AxisY) to 'Revenue' field of DataCollection
                    for (int j = 0; j < Speed_Chart(Car_Data,Laps,Current_Lap, Selected_Drivers[Index]).Count(); j++)
                    {
                        C1.Win.Chart.Series series = new C1.Win.Chart.Series();
                        series.DataSource = Speed_Chart(Car_Data, Laps, Current_Lap, Selected_Drivers[Index])[j];
                        series.Name = $"{Speed_Chart(Car_Data, Laps, Current_Lap, Selected_Drivers[Index])[j][0].drivernumber}";
                        series.Binding = "speed";
                        series.ChartType = C1.Chart.ChartType.LineSymbols;
                        FlexCharts[Index][0].Series.Add(series);
                    }

                    //this.FlexCharts[i].Series.Add(new C1.Win.Chart.Series
                    //{
                    //Name property specifies the string to be displayed corresponding to this Series in Legend
                    //    Name = "LapTimes",
                    //    Binding = "laptime"
                    //});
                    //Binding chart's AxisX to 'Date' so Dates are shown in Horizontal axis
                    this.FlexCharts[Index][0].BindingX = "time";
                }
            }
            else if(FlexCharts[Index][0].ChartType == ChartType.Bar)
            {
                if (Data_Types[Index][0] == "Tyre_Comp")
                {
                    FlexCharts[Index][0].DataSource = Stint_Chart(Stints, Current_Lap);

                    FlexCharts[Index][0].Series.Clear();

                    //Selecting chart's type 
                    this.FlexCharts[Index][0].ChartType = C1.Chart.ChartType.Bar;

                    //Setting chart's Header and styling it
                    this.FlexCharts[Index][0].Header.Content = "Tyre Degridation";
                    this.FlexCharts[Index][0].AxisX.Title = "Average Laps";
                    this.FlexCharts[Index][0].AxisY.Title = "Tyre Compounds";
                    //Adding a Series to chart and binding it (AxisY) to 'Revenue' field of DataCollection


                    this.FlexCharts[Index][0].Series.Add(new C1.Win.Chart.Series
                    {
                    //Name property specifies the string to be displayed corresponding to this Series in Legend
                        Name = "Average Laps",
                        Binding = "laps"
                    });
                    //Binding chart's AxisX to 'Date' so Dates are shown in Horizontal axis
                    this.FlexCharts[Index][0].BindingX = "compound";

                    
                }
            }
        }

        private Color Sector_Color(List<string> Colors)
        {
            int Yellow = 0;
            int Green = 0;
            int Purple = 0;
            Color Return_Color = new Color();

            try
            {
                foreach (string i in Colors)
                {
                    switch (i)
                    {
                        case "2048":
                            Yellow++;
                            break;
                        case "2049":
                            Green++;
                            break;
                        case "2051":
                            Purple++;
                            break;
                    }
                }

                if (Green + Purple == Colors.Count())
                {
                    Return_Color = Color.FromArgb(239, 41, 243);
                }
                else if (Green >= Yellow)
                {
                    Return_Color = Color.FromArgb(41, 243, 73);
                }
                else if (Yellow > Colors.Count() / 4)
                {
                    Return_Color = Color.FromArgb(243, 222, 41);
                }



            }
            catch (Exception e)
            {
                Return_Color = Color.Gray;
            }

            return Return_Color;
        }
        private string Find_Closest_Interval(DateTime Target, string Driver_Number)
        {
            string Interval = "N/A";
            List<Intervals> Temp_Intervals = Intervals.Where(Num => Num.driver_number.ToString() == Driver_Number).ToList();//Gets all intervals from the driver
            Temp_Intervals = Temp_Intervals.Where(Time => Time.date < Target).ToList();// Makes sure the target is less than the time
            TimeSpan shortestDifference = TimeSpan.MaxValue;

            for (int i = 0; i < Temp_Intervals.Count(); i++)
            {
                TimeSpan difference = Temp_Intervals[i].date - Target;
                TimeSpan absoluteDifference = difference.Duration();

                if (absoluteDifference < shortestDifference)
                {
                    shortestDifference = absoluteDifference;
                    if (Temp_Intervals[i].interval == null)
                    {
                        Interval = "N/A";
                    }
                    else
                    {
                        Interval = Temp_Intervals[i].interval.ToString();
                    }
                }
            }

            return Interval;
        }
        private void Update_LeaderBoard(List<Laps> LapInfo)
        {
            LeaderboardPanel.Controls.Clear();

            for (int i = 0; i < LapInfo.Count; i++)
            {
                bool Name_Found = false;
                Label label = new Label();

                for (int j = 0; j < Drivers.Count(); j++)
                {
                    if (Drivers[j].driver_number.ToString() == LapInfo[i].driver_number.ToString())
                    {
                        Name_Found = true;
                        if (LapInfo[i].date_start == null) { label.Text = $"{i + 1} {Drivers[j].first_name} {Drivers[j].last_name} : {LapInfo[i].lap_duration}"; }
                        else { label.Text = $"{i + 1}) {Drivers[j].full_name.ToString()} : {LapInfo[i].lap_duration} : int+{Find_Closest_Interval(DateTime.Parse(LapInfo[i].date_start.ToString()), LapInfo[i].driver_number.ToString())}"; }

                    }
                }
                if (Name_Found == false)
                {
                    label.Text = $"{i + 1}) {LapInfo[i].driver_number.ToString()} : {LapInfo[i].lap_duration}";
                }
                //Drivers Driver = Drivers.FirstOrDefault(Driver_num => Driver_num.driver_number == LapInfo[i].driver_number); Too hit and miss even with try catch etc
                //label.Text = $"{i + 1} {Driver.first_name} {Driver.last_name} : {LapInfo[i].lap_duration}";
                label.Width = 300;
                //Sessions FoundSession = Sessions.FirstOrDefault(session => session.session_name == Session_Select.Text);

                Label Sector_Times = new Label();
                Sector_Times.Text = "Sector Times :";
                Sector_Times.Height = 20;


                FlowLayoutPanel Driver_Panel = new FlowLayoutPanel();
                Driver_Panel.FlowDirection = FlowDirection.TopDown;
                Driver_Panel.Size = new Size(310, 20);
                FlowLayoutPanel Sector_Panel = new FlowLayoutPanel();
                Sector_Panel.FlowDirection = FlowDirection.LeftToRight;
                Sector_Panel.Height = 25;
                Sector_Panel.Width = 280;

                string Sector1_Text = string.Join("", LapInfo[i].duration_sector_1);
                Button Sector1 = new Button();
                Sector1.BackColor = Sector_Color(LapInfo[i].segments_sector_1.ToList());
                Sector1.Text = Sector1_Text;
                Sector1.ForeColor = Color.White;
                Sector1.Height = 25;
                Sector1.Width = 80;

                string Sector2_Text = string.Join("", LapInfo[i].duration_sector_1);
                Button Sector2 = new Button();
                Sector2.BackColor = Sector_Color(LapInfo[i].segments_sector_2.ToList());
                Sector2.Text = Sector2_Text;
                Sector2.ForeColor = Color.White;
                Sector2.Height = 25;
                Sector2.Width = 80;

                string Sector3_Text = string.Join("", LapInfo[i].duration_sector_1);
                Button Sector3 = new Button();
                Sector3.BackColor = Sector_Color(LapInfo[i].segments_sector_3.ToList());
                Sector3.Text = Sector3_Text;
                Sector3.ForeColor = Color.White;
                Sector3.Height = 25;
                Sector3.Width = 80;


                Sector_Panel.Controls.Add(Sector1);
                Sector_Panel.Controls.Add(Sector2);
                Sector_Panel.Controls.Add(Sector3);

                Driver_Panel.Controls.Add(label);


                LeaderboardPanel.Controls.Add(Driver_Panel);
                LeaderboardPanel.Controls.Add(Sector_Times);
                LeaderboardPanel.Controls.Add(Sector_Panel);
            }
        }
        private Button Add_Chart_Buttons()
        {
            Button Add_Chart = new Button();
            Add_Chart.Size = new Size(25, 25);
            Add_Chart.Text = "+";
            Add_Chart.Location = new Point(3, 3);
            Add_Chart.Click += Add_Chart_Click;
            return Add_Chart;
        }

        private Button Add_Settings_Buttons(int Width)
        {
            Button Settings = new Button();
            Settings.Size = new Size(25, 25);
            Settings.Text = "X";
            Settings.Location = new Point(Width - 28, 3);
            Settings.Click += Settings_Click;
            return Settings;
        }

        private void Settings_Click(object? sender, EventArgs e)
        {
            Button button = (Button)sender;
            Panel panel = button.Parent as Panel;
            Settings_Panel.Location = new Point(panel.Width - 131, 5);
            Settings_Panel.Visible = true;
            Select_Drivers_Panel.Location = new Point(panel.Width - Settings_Panel.Width - 143 - 35, 5);
            Select_Drivers_Panel.Visible = false;
            Select_Data_Panel.Location = new Point(panel.Width - Settings_Panel.Width - 143 - 35, 5);
            Select_Data_Panel.Visible = false;


            panel.Controls.Add(Settings_Panel);
            panel.Controls.Add(Select_Drivers_Panel);
            panel.Controls.Add(Select_Data_Panel);
            Select_Drivers_Panel.BringToFront();
            Select_Data_Panel.BringToFront();   
            Settings_Panel.BringToFront();


            for (int i = 0; i < Driver_Panel.Controls.Count; i++)
            {
                if (Selected_Drivers[int.Parse(panel.Name)].Count() == 0)
                {
                    Driver_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                }
                else
                {
                    bool Found = false;
                    for (int j = 0; j < Selected_Drivers[int.Parse(panel.Name)].Count(); j++)
                    {
                        if (Driver_Panel.Controls[i].Controls[1].Name == Selected_Drivers[int.Parse(panel.Name)][j])
                        {
                            Driver_Panel.Controls[i].Controls[1].BackColor = Color.Green;
                            Found = true;
                        }
                        else if (Found == false)
                        {
                            Driver_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                        }
                    }
                }

            }
            for(int i = 0; i < Data_Panel.Controls.Count; i++)
            {
                bool Found = false;
                for(int j = 0; j < Data_Types[int.Parse(panel.Name)].Count(); j++)
                {
                    if (Data_Panel.Controls[i].Controls[1].Name == Data_Types[int.Parse(panel.Name)][0])
                    {
                        Found = true;
                        Data_Panel.Controls[i].Controls[1].BackColor= Color.Green;
                    }
                    else if (Found == false)
                    {
                        Data_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                    }
                }
            }
        }

        private void Add_Chart_Click(object? sender, EventArgs e)
        {
            Button button = (Button)sender;
            Panel panel = button.Parent as Panel;
            Graph_Panel.Location = new Point(30, 5);
            Graph_Panel.Visible = true;
            panel.Controls.Add(Graph_Panel);
            Graph_Panel.BringToFront();

        }

        private void Create_Layout(string Current_Layout)
        {
            DashP.Controls.Clear();
            DashPanels.Clear();
            FlexCharts.Clear();
            Selected_Drivers.Clear();
            List<Point> Points = new List<Point>() { };
            int Panel_Height = DashP.Height;
            int Panel_Width = DashP.Width;

            switch (Current_Layout)
            {
                case "1":
                    Points.Add(new Point(0, 0));
                    Points.Add(new Point(Panel_Width / 2, 0));
                    Points.Add(new Point(0, Panel_Height / 2));
                    Points.Add(new Point(Panel_Width / 2, Panel_Height / 2));
                    for (int i = 0; i < 4; i++)
                    {
                        Panel panel = new Panel();
                        panel.Name = i.ToString();
                        panel.Width = Panel_Width / 2;
                        panel.Height = Panel_Height / 2;
                        panel.Location = Points[i];
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        DashPanels.Add(panel);
                        panel.Controls.Add(Add_Chart_Buttons());
                        panel.Controls.Add(Add_Settings_Buttons(Panel_Width / 2));
                        DashP.Controls.Add(panel);
                        List<string> Panels = new List<string>();
                        Selected_Drivers.Add(Panels);
                        List<string> Data = new List<string>() { "Lap_Times" };
                        Data_Types.Add(Data);
                        List<C1.Win.Chart.FlexChart> Chart = new List<C1.Win.Chart.FlexChart>();
                        FlexCharts.Add(Chart);
                    }
                    break;
                case "2":
                    Points.Add(new Point(0, 0));
                    Points.Add(new Point(Panel_Width / 2, 0));
                    Points.Add(new Point(0, Panel_Height / 2));
                    for (int i = 0; i < 3; i++)
                    {
                        int Width = 0;
                        Panel panel = new Panel();
                        panel.Name = i.ToString();
                        panel.Height = Panel_Height / 2;
                        if (i == 2) { panel.Width = Panel_Width; Width = Panel_Width; }
                        else { panel.Width = Panel_Width / 2; Width = Panel_Width / 2; }
                        panel.Location = Points[i];
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        DashPanels.Add(panel);
                        panel.Controls.Add(Add_Chart_Buttons());
                        panel.Controls.Add(Add_Settings_Buttons(Width));
                        DashP.Controls.Add(panel);
                        List<string> Panels = new List<string>();
                        Selected_Drivers.Add(Panels);
                        List<string> Data = new List<string>() { "Lap_Times" };
                        Data_Types.Add(Data);
                        List<C1.Win.Chart.FlexChart> Chart = new List<C1.Win.Chart.FlexChart>();
                        FlexCharts.Add(Chart);
                    }
                    break;
                case "3":
                    Points.Add(new Point(0, 0));
                    Points.Add(new Point(0, Panel_Height / 2));
                    Points.Add(new Point(Panel_Width / 2, Panel_Height / 2));
                    for (int i = 0; i < 3; i++)
                    {
                        int Width = 0;
                        Panel panel = new Panel();
                        panel.Name = i.ToString();
                        panel.Height = Panel_Height / 2;
                        if (i == 0) { panel.Width = Panel_Width; Width = Panel_Width; }
                        else { panel.Width = Panel_Width / 2; Width = Panel_Width / 2; }
                        panel.Location = Points[i];
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        DashPanels.Add(panel);
                        panel.Controls.Add(Add_Chart_Buttons());
                        panel.Controls.Add(Add_Settings_Buttons(Width));
                        DashP.Controls.Add(panel);
                        List<string> Panels = new List<string>();
                        Selected_Drivers.Add(Panels);
                        List<string> Data = new List<string>() { "Lap_Times" };
                        Data_Types.Add(Data);
                        List<C1.Win.Chart.FlexChart> Chart = new List<C1.Win.Chart.FlexChart>();
                        FlexCharts.Add(Chart);
                    }
                    break;
                case "4":
                    Points.Add(new Point(0, 0));
                    Points.Add(new Point(0, Panel_Height / 2));
                    for (int i = 0; i < 2; i++)
                    {
                        Panel panel = new Panel();
                        panel.Name = i.ToString();
                        panel.Width = Panel_Width;
                        panel.Height = Panel_Height / 2;
                        panel.Location = Points[i];
                        panel.BorderStyle = BorderStyle.FixedSingle;
                        DashPanels.Add(panel);
                        panel.Controls.Add(Add_Chart_Buttons());
                        panel.Controls.Add(Add_Settings_Buttons(Panel_Width));
                        DashP.Controls.Add(panel);
                        List<string> Panels = new List<string>();
                        Selected_Drivers.Add(Panels);
                        List<string> Data = new List<string>() { "Lap_Times" };
                        Data_Types.Add(Data);
                        List<C1.Win.Chart.FlexChart> Chart = new List<C1.Win.Chart.FlexChart>();
                        FlexCharts.Add(Chart);
                    }
                    break;

            }
        }
        public async Task<List<Meetings>> GetMeetings(string uri)//Function that Calls to the API to get all the meetings
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Meetings>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Sessions>> GetSessions(string uri)//Function that Calls to the API to get all the sessions for the meeting
        {//gets all of the session data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Sessions>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Drivers>> GetDrivers(string uri)//Function that Calls to the API to get all the drivers in the session
        {//gets all of the driver data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Drivers>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Laps>> GetLaps(string uri)//Function that Calls to the API to get all the drivers in the session
        {//gets all of the driver data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Laps>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Intervals>> GetIntervals(string uri)//Function that Calls to the API to get all the drivers in the session
        {//gets all of the driver data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Intervals>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Car_Data>> GetCar_Data(string uri)//Function that Calls to the API to get all the drivers in the session
        {//gets all of the driver data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Car_Data>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Stints>> GetStint(string uri)//Function that Calls to the API to get all the drivers in the session
        {//gets all of the driver data and then adds it into the list
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Stints>>(body);//Decerialize the String into Json 
            }
        }




        private async void Dashboard_Load(object sender, EventArgs e)//Dashboard inital loading
        //This function is mainly used to open everything that the software needs for the user
        {
            try
            {
                Meetings = await GetMeetings("https://api.openf1.org/v1/meetings?year=2023");
            }
            catch (Exception ex) { }

            for (int i = 0; i < Meetings.Count(); i++)
            {

                //Adds the 2023 Races to the combobox
                string raceName = Meetings[i].meeting_name.ToString();
                Race_Select.Invoke(new Action(() => Race_Select.Items.Add(raceName)));
            }

            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1w.png"));

            List<string> Temp_Data_Types = new List<string>() {"Lap_Times","Speed","Throttle","Pit_Stops","Tyre_Comp" };

            for (int i = 0;i < Temp_Data_Types.Count();i++)
            {
                Panel panel = Add_Driver_Panel(i.ToString(), Data_Panel);
                panel.Controls.Add(Add_Driver_Label(Temp_Data_Types[i]));
                panel.Controls.Add(Add_Data_Button(Temp_Data_Types[i]));
            }
            Create_Layout(Layout);
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            label1.Text = this.Size.ToString();
        }

        private async void Race_Select_SelectedIndexChanged(object sender, EventArgs e)//When the Race is selected it will then get the respective sessions
        {
            Session_Select.Items.Clear();
            Session_Select.Text = string.Empty;
            for (int i = 0; i < Meetings.Count(); i++)
            {
                if (Meetings[i].meeting_name.ToString() == Race_Select.Text)
                {
                    try
                    {
                        Sessions = await GetSessions($"https://api.openf1.org/v1/sessions?meeting_key={Meetings[i].meeting_key}&year=2023");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    for (int f = 0; f < Sessions.Count(); f++)
                    {
                        Session_Select.Items.Add(Sessions[f].session_name);
                        //Adds the Sessions to the combo box
                    }
                }
            }

        }

        private void Layout_Button_Click(object sender, EventArgs e)//Pops up the Layout Selecter Panel
        {
            if (Layout_Picker.Visible)
            {
                Layout_Picker.Visible = false;
            }
            else
            {
                Layout_Picker.Visible = true;
            }

        }

        private void Choose_Graph_Click(object sender, EventArgs e)
        {

        }

        private void Graph_Panel_MouseEnter(object sender, EventArgs e)
        {
            Console.WriteLine("ON");
        }

        private void Graph_Panel_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("OFF");
        }

        private void pictureBox2_Click(object sender, EventArgs e)//Should be called Layout1 But isnt and doesnt like me changing it lol
        {
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1w.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3b.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4b.png"));
            Layout = "1";
            Create_Layout(Layout);
        }

        private void Layout4_Click(object sender, EventArgs e)
        {
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1b.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3b.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4w.png"));
            Layout = "4";
            Create_Layout(Layout);
        }

        private void Layout3_Click(object sender, EventArgs e)
        {
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1b.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3w.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4b.png"));
            Layout = "3";
            Create_Layout(Layout);
        }

        private void Layout2_Click(object sender, EventArgs e)
        {
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1b.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2w.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3b.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4b.png"));
            Layout = "2";
            Create_Layout(Layout);
        }

        private async void Session_Select_SelectedIndexChanged(object sender, EventArgs e)// Used Linq This time because I felt fancy But this just gets the drivers that feature in the session
        {
            Race_Start.Enabled = true;
            try
            {
                Sessions FoundSession = Sessions.FirstOrDefault(session => session.session_name == Session_Select.Text);// Searches for the Sessions object which matches the text of the combox
                Selected_Session_Key = FoundSession.session_key.ToString();
                Drivers = await GetDrivers($"https://api.openf1.org/v1/drivers?session_key={Selected_Session_Key}");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        

        private async void Race_Start_Click(object sender, EventArgs e)
        {
            Race_Start.Enabled = false;
            Resume.Enabled = true;
            Pause.Enabled = true;
            LeaderboardPanel.Controls.Clear();
            List<Laps> Temp_Laps = new List<Laps>();
            for (int i = 0; i < Drivers.Count; i++)
            {
                Temp_Laps = await GetLaps($"https://api.openf1.org/v1/laps?session_key={Selected_Session_Key}&driver_number={Drivers[i].driver_number}");
                Laps.Add(Temp_Laps);
            }
            MessageBox.Show($"{Selected_Session_Key}");
            Intervals = await GetIntervals($"https://api.openf1.org/v1/intervals?session_key={Selected_Session_Key}");

            Stints = await GetStint($"https://api.openf1.org/v1/stints?session_key={Selected_Session_Key}");
            MessageBox.Show(Stints.Count().ToString());

            for (int i = 0; i < Laps.Count; i++)
            {
                if (Laps[i].Count() > Total_Laps)
                {
                    Total_Laps = Laps[i].Count();
                }
            }
            Race_Timer.Start();
            Current_Lap = 0;
            for (int i = 0; i < Drivers.Count; i++)
            {
                Panel panel = Add_Driver_Panel(Drivers[i].driver_number.ToString(),Select_Drivers_Panel);
                panel.Controls.Add(Add_Driver_Label(Drivers[i].full_name));
                panel.Controls.Add(Add_Driver_Button(Drivers[i].driver_number.ToString()));
                Driver_Panel.Controls.Add(panel);
            }
            List<Car_Data> Temp_Car_Data = new List<Car_Data>();
            for (int i = 0;i < Drivers.Count; i++)
            {
                Temp_Car_Data = await GetCar_Data($"https://api.openf1.org/v1/car_data?session_key={Selected_Session_Key}&speed>0&driver_number={Drivers[i].driver_number}");
                Car_Data.Add( Temp_Car_Data );
            }
            

        }

        private void Race_Timer_Tick(object sender, EventArgs e)
        {
            Current_Lap++;
            Lap_Counter.Text = $"{Current_Lap}/{Total_Laps}";
            List<Laps> Current_LapInfo_Temp = new List<Laps>();
            List<Laps> Current_LapInfo = new List<Laps>();
            for (int i = 0; i < Laps.Count; i++)
            {
                int temp_lap = 0;
                for (int j = 0; j < Laps[i].Count; j++)
                {
                    if (Current_Lap >= Total_Laps)
                    {

                        if (Laps[i][j].lap_number >= temp_lap)
                        {
                            temp_lap = Laps[i][j].lap_number;
                        }
                    }
                    else
                    {
                        if (Laps[i][j].lap_number == Current_Lap)
                        {
                            Current_LapInfo_Temp.Add(Laps[i][j]);
                        }
                    }

                }
                if (temp_lap > 0)
                {

                    Current_LapInfo_Temp.Add(Laps[i][temp_lap - 1]);
                }
            }
            //var sortedLaps = lapsList.OrderBy(lap => lap.date_start);
            //if (Current_Lap >= Total_Laps)
            //{
            //    Current_LapInfo = Current_LapInfo_Temp.OrderBy(lap => DateTime.Parse(lap.date_start.ToString()).AddMilliseconds(double.Parse(lap.lap_duration.ToString()) * 1000)).ToList();
            //}
            //else
            //{
            Current_LapInfo = Current_LapInfo_Temp.OrderBy(lap => lap.date_start).ToList();
            //}
            Update_LeaderBoard(Current_LapInfo);


            for(int i = 0;i < FlexCharts.Count; i++)
            {
                if (FlexCharts[i].Count() == 0)
                { }
                else { Update_Chart(i); }
            }

            if (Current_Lap >= Total_Laps)
            {
                Race_Timer.Stop();
            }

        }

        private void Resume_Click(object sender, EventArgs e)
        {
            Race_Timer.Start();
            Race_Start.Enabled = false;
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            Race_Timer.Stop();
            Race_Start.Enabled = true;
        }

        private void Line_Graph_Click(object sender, EventArgs e)
        {
            Panel Area = Line_Graph.Parent.Parent as Panel;
            int Width = Area.Width;
            int Height = Area.Height;
            C1.Win.Chart.FlexChart Chart = new C1.Win.Chart.FlexChart();
            Chart.Width = Width - 55;
            Chart.Height = Height - 20;
            Chart.Location = new Point(30, 10);
            Chart.ChartType = ChartType.LineSymbols;
            if (FlexCharts[int.Parse(Area.Name)].Count() == 1)
            {
                C1.Win.Chart.FlexChart Old_Chart = FlexCharts[int.Parse(Area.Name)][0];
                Old_Chart.Dispose();
                FlexCharts[int.Parse(Area.Name)].Clear();
                FlexCharts[int.Parse(Area.Name)].Add(Chart);
            }
            else
            {
                FlexCharts[int.Parse(Area.Name)].Add(Chart);
            }
            Area.Controls.Add(Chart);

        }

        private void Bar_Chart_Click(object sender, EventArgs e)
        {
            Panel Area = Line_Graph.Parent.Parent as Panel;
            int Width = Area.Width;
            int Height = Area.Height;
            C1.Win.Chart.FlexChart Chart = new C1.Win.Chart.FlexChart();
            Chart.Width = Width - 55;
            Chart.Height = Height - 20;
            Chart.Location = new Point(30, 10);
            Chart.ChartType = ChartType.Bar;
            
            if(FlexCharts[int.Parse(Area.Name)].Count() ==1) 
            {
                C1.Win.Chart.FlexChart Old_Chart = FlexCharts[int.Parse(Area.Name)][0] ;
                Old_Chart.Dispose();
                FlexCharts[int.Parse(Area.Name)].Clear();
                FlexCharts[int.Parse(Area.Name)].Add(Chart);
            }
            else
            {
                FlexCharts[int.Parse(Area.Name)].Add(Chart);
            }
            Area.Controls.Add(Chart);
        }

        private void Pie_Chart_Click(object sender, EventArgs e)
        {

        }

        private void Close_Graph_Click(object sender, EventArgs e)
        {
            Graph_Panel.Visible = false;
        }

        private void Close_Settings_Click(object sender, EventArgs e)
        {
            Settings_Panel.Visible = false;
        }

        private void Driver_Select_Click(object sender, EventArgs e)
        {
            Select_Drivers_Panel.Visible = true;
        }

        private void Select_Drivers_Panel_Close_Click(object sender, EventArgs e)
        {
            Select_Drivers_Panel.Visible = false;
        }

        private void Data_Select_Click(object sender, EventArgs e)
        {
            Select_Data_Panel.Visible = true;
        }

        private void Select_Data_Panel_Close_Click(object sender, EventArgs e)
        {
            Select_Data_Panel.Visible = false;
        }
    }
}
