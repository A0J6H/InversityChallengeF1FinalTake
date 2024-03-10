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
        List<List<Laps>> Laps = new List<List<Laps>>();

        string Resources_Path = Path.Combine(Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName, "Resources"); //Gets the Path to the resources folder

        string Layout = "1";
        int Current_Lap = 0;
        int Total_Laps;

        List<Panel> DashPanels = new List<Panel>();
        List<C1.Win.Chart.FlexChart> FlexCharts = new List<C1.Win.Chart.FlexChart>();
        List<List<int>> Selected_Drivers = new List<List<int>>();

        string Selected_Session_Key;

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

        static List<laptimes> Lap_Time_Chart(List<List<Laps>> Laps, int current_lap)
        {
            List<laptimes> Lap_Time = new List<laptimes>();
            for (int i = 0; i < current_lap; i++)
            {
                if (Laps[0][i].lap_duration == null || Laps[0][i].is_pit_out_lap == true)
                {

                }
                else
                {
                    Lap_Time.Add(new laptimes(Laps[0][i].driver_number.ToString(), Laps[0][i].lap_number, double.Parse(Laps[0][i].lap_duration.ToString())) { });
                }
            }
            return Lap_Time;
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
            panel.Controls.Add(Settings_Panel);
        }

        private void Add_Chart_Click(object? sender, EventArgs e)
        {
            Button button = (Button)sender;
            Panel panel = button.Parent as Panel;
            Graph_Panel.Location = new Point(30, 5);
            Graph_Panel.Visible = true;
            panel.Controls.Add(Graph_Panel);

        }

        private void Create_Layout(string Current_Layout)
        {
            DashP.Controls.Clear();
            DashPanels.Clear();
            FlexCharts.Clear();
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


            for (int i = 0; i < Laps.Count; i++)
            {
                if (Laps[i].Count() > Total_Laps)
                {
                    Total_Laps = Laps[i].Count();
                }
            }
            Race_Timer.Start();
            Current_Lap = 0;
            foreach (Drivers Driver in Drivers)
            {
                Selected_Drivers_Check.Items.Add(string.Join("", Driver));
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

            FlexCharts[0].DataSource = Lap_Time_Chart(Laps, Current_Lap);

            FlexCharts[0].Series.Clear();

            //Selecting chart's type 
            this.FlexCharts[0].ChartType = C1.Chart.ChartType.LineSymbols;

            //Setting chart's Header and styling it
            this.FlexCharts[0].Header.Content = "DateWise Revenue";

            //Adding a Series to chart and binding it (AxisY) to 'Revenue' field of DataCollection
            this.FlexCharts[0].Series.Add(new C1.Win.Chart.Series
            {
                //Name property specifies the string to be displayed corresponding to this Series in Legend
                Name = "LapTimes",
                Binding = "laptime"
            });



            //Binding chart's AxisX to 'Date' so Dates are shown in Horizontal axis
            this.FlexCharts[0].BindingX = "lapnumber";


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
            C1.Win.Chart.FlexChart Line_Chart = new C1.Win.Chart.FlexChart();
            Line_Chart.Width = Width - 55;
            Line_Chart.Height = Height - 20;
            Line_Chart.Location = new Point(30, 10);
            FlexCharts.Add(Line_Chart);
            Area.Controls.Add(Line_Chart);
        }

        private void Bar_Chart_Click(object sender, EventArgs e)
        {

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

        }

        private void Selected_Drivers_Check_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
