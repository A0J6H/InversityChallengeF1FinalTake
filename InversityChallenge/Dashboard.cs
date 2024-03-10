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
        //Public Lists To Contain the Data from the API//
        List<Meetings> Meetings;
        List<Sessions> Sessions;
        List<Drivers> Drivers;
        List<Intervals> Intervals;
        List<Stints> Stints;
        List<List<Laps>> Laps = new List<List<Laps>>();
        List<List<Car_Data>> Car_Data = new List<List<Car_Data>>();
        ///////////////////////////////////////////////////////////////////
        /////Path for images//////////Hopefully this works on other peoples devices as i have no way of checking XD//
        string Resources_Path = Path.Combine(Directory.GetParent(Application.StartupPath).Parent.Parent.Parent.FullName, "Resources"); //Gets the Path to the resources folder

        string Layout = "1";
        int Current_Lap = 0;
        int Total_Laps;

        ////////Lists for Containing info about graphs etc
        List<Panel> DashPanels = new List<Panel>();
        List<List<C1.Win.Chart.FlexChart>> FlexCharts = new List<List<C1.Win.Chart.FlexChart>>();
        List<List<string>> Selected_Drivers = new List<List<string>>();
        List<List<string>> Data_Types = new List<List<string>>();

        string Selected_Session_Key;

        private Panel Add_Driver_Panel(string Driver_Number,Panel Add_Panel)//Creates a panel for the dropdown elemets when selecting drivers and data 
        {
            Panel panel = new Panel();
            panel.Size = new Size(120, 30);
            Add_Panel.Controls.Add(panel);
            return panel;
        }
        private Label Add_Driver_Label(string Driver_Name)//Creates the labels for the drivers name and the datatypes for the dropdowns
        {
            Label label = new Label();
            string fullName = Driver_Name;
            string[] parts = fullName.Split(' ');
            string lastName = parts[parts.Length - 1];
            label.Text = lastName;
            label.Location = new Point(0, 10);
            return label;
        }

        private Button Add_Driver_Button(string Driver_Number)//Creates the buttons that allow you to select drivers
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

        private void Add_Driver_Click(object? sender, EventArgs e)//Adds the driver to the list and changes the buttons colors
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
        private Button Add_Data_Button(string Name)//Same as the driver button except it is used for the Data types rather than the Driver selection
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

        private void Add_Data_Click(object? sender, EventArgs e)//Makes sure that there is only one color and that there isnt more than one data type selected 
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

        public class laptimes//Class that stores the information for the laptimes graph
        {
            public string drivernumber { get; set; }
            public int lapnumber { get; set; }
            public double laptime { get; set; }

            public laptimes(string Drivernumber, int Lapnumber, double Laptime)//Constructor for the class
            {
                this.drivernumber = Drivernumber;
                this.lapnumber = Lapnumber;
                this.laptime = Laptime;
            }
        }

        static List<List<laptimes>> Lap_Time_Chart(List<List<Laps>> Laps, int current_lap, List<string> Driver_selection)//Formats and gets all of the relevent data into the class so it is correctly outputted onto the graph
        {

            List<List<laptimes>> Lap_Times = new List<List<laptimes>>();
            List<Laps> Temp_Lap = new List<Laps>();//Temporary list so that it creates a 2D List
            for (int d = 0; d < Driver_selection.Count; d++)
            {
                List<laptimes> Lap_Time = new List<laptimes>();
                for (int l = 0; l < Laps.Count(); l++)
                {
                    try
                    {
                        if (Laps[l][0].driver_number.ToString() == Driver_selection[d].ToString())
                        {
                            Temp_Lap = Laps[l];//Checks to see if it is the correct driver and then sets the variable
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
                        //Creates a new instence of the class and then adds it to the temporary list
                    }
                }
                Lap_Times.Add(Lap_Time);//Adds all of the temporary lists up to 
            }
            return Lap_Times;//returns the list
        }

        public class Speed//Class for the speed graph
        {
            public string drivernumber { get; set; }
            public double time { get; set; }
            public double speed { get; set; }

            public Speed(string Drivernumber, double Time, double Speed)//Constructor for the class
            {
                this.drivernumber = Drivernumber;
                this.time = Time;
                this.speed = Speed;
            }
        }

        static List<List<Speed>> Speed_Chart(List<List<Car_Data>> Car_Data, List<List<Laps>> Lap_Info, int Current_Lap, List<string> Driver_Selection)//Formats and gets all of the relevent data into the class so it is correctly outputted onto the graph
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
                        {//this checks wether it is the correct driver and the current lap
                            
                            Start = DateTime.Parse(Lap_Info[k][l].date_start.ToString());
                            End = Start.AddMilliseconds(double.Parse(Lap_Info[k][l].lap_duration.ToString())*1000);
                            // These two variables then create a boundary for the time
                        }
                    }
                }

                for(int i = 0; i < Car_Data.Count(); i++)
                {
                    for(int j = 0; j < Car_Data[i].Count(); j++)
                    {
                        if (Car_Data[i][j].driver_number.ToString() == Driver_Selection[d].ToString() && Car_Data[i][j].date>Start && Car_Data[i][j].date < End)
                        {//checks again to make sure its the correct driver as we are dealing with a seperate list and then checks to see if the time is within the laps start and finish
                            c++;
                            Temp_Speeds.Add(new Speed(Car_Data[i][j].driver_number.ToString(),c, Car_Data[i][j].speed));
                            //creates a new instance and then adds it to the temp data
                        }
                    }
                }

                Speeds.Add(Temp_Speeds);
                //adds all the temp datas together to create the 2D list 
            }
            return Speeds;//Returns the list
        }

        public class Stint//Final class for the graphs 
        {
            public int laps { get; set; }
            public string compound { get; set; }

            public Stint(int Laps,string Compound)//constructor for the graph
            {
                this.laps = Laps;
                this.compound = Compound;
            }
        }

        static List<Stint> Stint_Chart(List<Stints> stints,int Current_Lap)
        {
            List<Stint> Stints = new List<Stint>();
            int Soft = 0;//Count for laps
            int SC = 0;//Count for stints
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
                        case "SOFT"://cases check for the tyre compound and then
                            if (stints[i].lap_end > Current_Lap) 
                            {
                                SC++;//add a count to the SOFT COUNT
                                Soft += Current_Lap - stints[i].lap_start;//add the total amount of laps to the variable
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
            Stints.Add(new Stint(Soft / SC, "Soft"));//creates the instance and adds it to the list
            Stints.Add(new Stint(Medium / MC, "Medium"));
            Stints.Add(new Stint(Hard / HC, "Hard"));
            return Stints;//returns the list
            
        }


        private void Update_Chart(int Index)//This function updates all of the charts!
            {
            if (FlexCharts[Index][0].ChartType == ChartType.LineSymbols)//checks what type of chart
            {
                if (Data_Types[Index][0] == "Lap_Times")//checks what data the chart has
                {
                    FlexCharts[Index][0].Series.Clear();//clears the past series

                    //Selecting chart's type 
                    this.FlexCharts[Index][0].ChartType = C1.Chart.ChartType.LineSymbols;

                    //Setting chart's Header and styling it
                    this.FlexCharts[Index][0].Header.Content = "Lap Times";
                    this.FlexCharts[Index][0].AxisX.Title = "Lap Times (Seconds)";
                    this.FlexCharts[Index][0].AxisY.Title = "Lap Number";

                    //Adding a Series to chart (Y Axis)
                    for (int j = 0; j < Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index]).Count(); j++)
                    {
                        C1.Win.Chart.Series series = new C1.Win.Chart.Series();
                        series.DataSource = Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index])[j];
                        series.Name = $"{Lap_Time_Chart(Laps, Current_Lap, Selected_Drivers[Index])[j][0].drivernumber}";
                        series.Binding = "laptime";
                        series.ChartType = C1.Chart.ChartType.LineSymbols;
                        FlexCharts[Index][0].Series.Add(series);
                    }
                    
                    //Adding the x Axis
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

                    //Setting the Xaxis on the chart
                    this.FlexCharts[Index][0].BindingX = "time";
                }
            }
            else if(FlexCharts[Index][0].ChartType == ChartType.Bar)//if the chart is a bar chart...
            {
                if (Data_Types[Index][0] == "Tyre_Comp")
                {
                    FlexCharts[Index][0].DataSource = Stint_Chart(Stints, Current_Lap);//setting the datasource since it is a 1D list then Y Axis does not need to be dynamic as there is always 3 compounds

                    FlexCharts[Index][0].Series.Clear();

                    //Selecting chart's type 
                    this.FlexCharts[Index][0].ChartType = C1.Chart.ChartType.Bar;

                    //Setting chart's Header and styling it
                    this.FlexCharts[Index][0].Header.Content = "Tyre Degridation";
                    this.FlexCharts[Index][0].AxisX.Title = "Average Laps";
                    this.FlexCharts[Index][0].AxisY.Title = "Tyre Compounds";
                    


                    this.FlexCharts[Index][0].Series.Add(new C1.Win.Chart.Series
                    {
                    //Name property specifies the string to be displayed 
                        Name = "Average Laps",
                        Binding = "laps"
                    });

                    //Binding chart's AxisX 
                    this.FlexCharts[Index][0].BindingX = "compound";

                    
                }
            }
        }

        private Color Sector_Color(List<string> Colors)//Sets the colors of the sector buttons
        {
            int Yellow = 0;
            int Green = 0;
            int Purple = 0;
            Color Return_Color = new Color();

            try
            {
                foreach (string i in Colors)
                {
                    switch (i)//adds the colors counts up
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

                if (Green + Purple == Colors.Count())//This probably isnt very accurate however it seems to give alright results
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
        private string Find_Closest_Interval(DateTime Target, string Driver_Number)//Finds the closest time to a target
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
        private void Update_LeaderBoard(List<Laps> LapInfo)//this updates the whole leaderboard
        {
            LeaderboardPanel.Controls.Clear();//clear the controls every time 

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


                label.Width = 300;//setting the labels width

                Label Sector_Times = new Label();
                Sector_Times.Text = "Sector Times :";
                Sector_Times.Height = 20;


                FlowLayoutPanel Driver_Panel = new FlowLayoutPanel();//panels to hold the controls (buttons)
                Driver_Panel.FlowDirection = FlowDirection.TopDown;
                Driver_Panel.Size = new Size(310, 20);
                FlowLayoutPanel Sector_Panel = new FlowLayoutPanel();//Flow layout so the are automatically spaced out
                Sector_Panel.FlowDirection = FlowDirection.LeftToRight;
                Sector_Panel.Height = 25;
                Sector_Panel.Width = 280;

                string Sector1_Text = string.Join("", LapInfo[i].duration_sector_1);//creating the sector time buttons
                Button Sector1 = new Button();
                Sector1.BackColor = Sector_Color(LapInfo[i].segments_sector_1.ToList());//gets the ouput from the color function
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

                //Add the buttons to the First panel
                Sector_Panel.Controls.Add(Sector1);
                Sector_Panel.Controls.Add(Sector2);
                Sector_Panel.Controls.Add(Sector3);

                //Add the, all to the next panel
                Driver_Panel.Controls.Add(label);

                //Add them all onto the leaderboard panel
                LeaderboardPanel.Controls.Add(Driver_Panel);
                LeaderboardPanel.Controls.Add(Sector_Times);
                LeaderboardPanel.Controls.Add(Sector_Panel);
            }
        }
        private Button Add_Chart_Buttons()//Adds the + button in the top left corner of every box
        {
            Button Add_Chart = new Button();
            Add_Chart.Size = new Size(25, 25);
            Add_Chart.Text = "+";
            Add_Chart.Location = new Point(3, 3);
            Add_Chart.Click += Add_Chart_Click;
            return Add_Chart;
        }

        private Button Add_Settings_Buttons(int Width)// this is the X button on the top right of every box
        {
            Button Settings = new Button();
            Settings.Size = new Size(25, 25);
            Settings.Text = "X";
            Settings.Location = new Point(Width - 28, 3);
            Settings.Click += Settings_Click;
            return Settings;
        }

        private void Settings_Click(object? sender, EventArgs e)//This is the logic for what happens when you press the top right button on all the boxes
        {
            Button button = (Button)sender;//gets the sender as a button so that it knows where the button is and what version of itself it is
            Panel panel = button.Parent as Panel;//This means that i now have the panel that contains the button

            //placing all of the forms and panels
            Settings_Panel.Location = new Point(panel.Width - 131, 5);
            Settings_Panel.Visible = true;
            Select_Drivers_Panel.Location = new Point(panel.Width - Settings_Panel.Width - 143 - 35, 5);
            Select_Drivers_Panel.Visible = false;
            Select_Data_Panel.Location = new Point(panel.Width - Settings_Panel.Width - 143 - 35, 5);
            Select_Data_Panel.Visible = false;

            //Add everything to the new panel that the user has clicked
            panel.Controls.Add(Settings_Panel);
            panel.Controls.Add(Select_Drivers_Panel);
            panel.Controls.Add(Select_Data_Panel);
            //bring to front so that they are above the graph
            Select_Drivers_Panel.BringToFront();
            Select_Data_Panel.BringToFront();   
            Settings_Panel.BringToFront();

            ///this for loop ensures that all of the different box's driver choices are how they are supposed to be 
            for (int i = 0; i < Driver_Panel.Controls.Count; i++)
            {
                if (Selected_Drivers[int.Parse(panel.Name)].Count() == 0)
                {
                    Driver_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                    //turns the buttons all red if no driver has been selected
                }
                else
                {
                    bool Found = false;
                    //checks to see if the driver has been selected
                    for (int j = 0; j < Selected_Drivers[int.Parse(panel.Name)].Count(); j++)
                    {
                        if (Driver_Panel.Controls[i].Controls[1].Name == Selected_Drivers[int.Parse(panel.Name)][j])
                        {
                            Driver_Panel.Controls[i].Controls[1].BackColor = Color.Green;
                            Found = true;
                            //turns the button Green if the driver has been selected
                        }
                        else if (Found == false)
                        {
                            Driver_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                            //if the driver hasnt it gets turned to Red
                        }
                    }
                }
            }

            //this makes sure that there is only 1 datatype selected at all times per box
            for(int i = 0; i < Data_Panel.Controls.Count; i++)
            {
                bool Found = false;
                //checks to see if it has been selected
                for(int j = 0; j < Data_Types[int.Parse(panel.Name)].Count(); j++)
                {
                    if (Data_Panel.Controls[i].Controls[1].Name == Data_Types[int.Parse(panel.Name)][0])
                    {
                        Found = true;
                        Data_Panel.Controls[i].Controls[1].BackColor= Color.Green;
                        //turns green if it has beenn selected
                    }
                    else if (Found == false)
                    {
                        Data_Panel.Controls[i].Controls[1].BackColor = Color.Red;
                        //and red if it hasnt
                    }
                }
            }
        }

        private void Add_Chart_Click(object? sender, EventArgs e)//This changes the location of the graph_panel
        {
            Button button = (Button)sender;
            Panel panel = button.Parent as Panel;
            Graph_Panel.Location = new Point(30, 5);
            Graph_Panel.Visible = true;
            panel.Controls.Add(Graph_Panel);
            Graph_Panel.BringToFront();//so that the graph is not infront of the control

        }

        private void Create_Layout(string Current_Layout)//This function Creates the layout of the Dashboard
        {
            DashP.Controls.Clear();
            DashPanels.Clear();
            FlexCharts.Clear();
            Selected_Drivers.Clear();
            List<Point> Points = new List<Point>() { };
            //clearing everything out when changed so that there are no problems when changing layout
            int Panel_Height = DashP.Height;
            int Panel_Width = DashP.Width;

            switch (Current_Layout)
            {
                case "1"://all of these cases are similar however they are too different to make a function for them which is annoying 
                    Points.Add(new Point(0, 0));
                    Points.Add(new Point(Panel_Width / 2, 0));
                    Points.Add(new Point(0, Panel_Height / 2));
                    Points.Add(new Point(Panel_Width / 2, Panel_Height / 2));//adds all the points to a list so that they can easily be placed without errors
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

                        //Setting a lot of things to their default state to make sure there are no errors when changin layout
                        List<string> Panels = new List<string>();
                        Selected_Drivers.Add(Panels);
                        List<string> Data = new List<string>() { "Lap_Times" };
                        Data_Types.Add(Data);
                        List<C1.Win.Chart.FlexChart> Chart = new List<C1.Win.Chart.FlexChart>();
                        FlexCharts.Add(Chart);
                    }
                    break;//See case 1 for explanation
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
                case "3"://See case 1 for explanation
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
                case "4"://See case 1 for explanation
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

        public async Task<List<Laps>> GetLaps(string uri)//Function that Calls to the API to get all the laps in the session
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

        public async Task<List<Intervals>> GetIntervals(string uri)//Function that Calls to the API to get all the interval data for the session
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
                return JsonConvert.DeserializeObject<List<Intervals>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Car_Data>> GetCar_Data(string uri)//Function that Calls to the API to get all the car data for the session
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
                return JsonConvert.DeserializeObject<List<Car_Data>>(body);//Decerialize the String into Json 
            }
        }

        public async Task<List<Stints>> GetStint(string uri)//Function that Calls to the API to get all the tyre stints in the session
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

            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1w.png"));//gets the paths for the layot buttons

            List<string> Temp_Data_Types = new List<string>() {"Lap_Times","Speed","Tyre_Comp" };//Plan to add many more options in the futiure but time constraints and scope creep are something

            for (int i = 0;i < Temp_Data_Types.Count();i++)
            {
                Panel panel = Add_Driver_Panel(i.ToString(), Data_Panel);
                panel.Controls.Add(Add_Driver_Label(Temp_Data_Types[i]));
                panel.Controls.Add(Add_Data_Button(Temp_Data_Types[i]));
            }

            Create_Layout(Layout);//runs the layout creator
        }

        private void Dashboard_Resize(object sender, EventArgs e)//Used for testing dont look
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

        private void Choose_Graph_Click(object sender, EventArgs e)//Used for testing dont look
        {

        }

        private void Graph_Panel_MouseEnter(object sender, EventArgs e)//Used for testing dont look
        {
            Console.WriteLine("ON");
        }

        private void Graph_Panel_MouseLeave(object sender, EventArgs e)//Used for testing dont look
        {
            Console.WriteLine("OFF");
        }

        private void pictureBox2_Click(object sender, EventArgs e)//Should be called Layout1 But isnt and doesnt like me changing it lol
        {
            //sets all of the images to the correct image using the path
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1w.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3b.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4b.png"));
            Layout = "1";
            Create_Layout(Layout);
        }

        private void Layout4_Click(object sender, EventArgs e)
        {
            //sets all of the images to the correct image using the path
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1b.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3b.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4w.png"));
            Layout = "4";
            Create_Layout(Layout);
        }

        private void Layout3_Click(object sender, EventArgs e)
        {
            //sets all of the images to the correct image using the path
            Layout1.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout1b.png"));
            Layout2.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout2b.png"));
            Layout3.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout3w.png"));
            Layout4.Image = Image.FromFile(Path.Combine(Resources_Path, "Layout4b.png"));
            Layout = "3";
            Create_Layout(Layout);
        }

        private void Layout2_Click(object sender, EventArgs e)
        {
            //sets all of the images to the correct image using the path
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

        

        private async void Race_Start_Click(object sender, EventArgs e)//Lights out button
        {
            Race_Start.Enabled = false;
            Resume.Enabled = true;
            Pause.Enabled = true;
            //enables and disables things so that users cant break stuff so easily
            LeaderboardPanel.Controls.Clear();
            List<Laps> Temp_Laps = new List<Laps>();
            
            for (int i = 0; i < Drivers.Count; i++)//gets all of the drivers in the session
            {
                Temp_Laps = await GetLaps($"https://api.openf1.org/v1/laps?session_key={Selected_Session_Key}&driver_number={Drivers[i].driver_number}");
                Laps.Add(Temp_Laps);
            }

            Intervals = await GetIntervals($"https://api.openf1.org/v1/intervals?session_key={Selected_Session_Key}");//gets all of the intervals in the session

            Stints = await GetStint($"https://api.openf1.org/v1/stints?session_key={Selected_Session_Key}");//gets all of the Stints in the session
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
            for (int i = 0; i < Drivers.Count; i++)//Adds all of the dropdown items for selecting drivers
            {
                Panel panel = Add_Driver_Panel(Drivers[i].driver_number.ToString(),Select_Drivers_Panel);
                panel.Controls.Add(Add_Driver_Label(Drivers[i].full_name));
                panel.Controls.Add(Add_Driver_Button(Drivers[i].driver_number.ToString()));
                Driver_Panel.Controls.Add(panel);//Adds them to the panel
            }
            List<Car_Data> Temp_Car_Data = new List<Car_Data>();
            for (int i = 0;i < Drivers.Count; i++)
            {
                Temp_Car_Data = await GetCar_Data($"https://api.openf1.org/v1/car_data?session_key={Selected_Session_Key}&speed>0&driver_number={Drivers[i].driver_number}");
                Car_Data.Add( Temp_Car_Data );//gets all the cardata for the session when the car is moving
            }
            

        }

        private void Race_Timer_Tick(object sender, EventArgs e)//Race timer 
        {
            Current_Lap++;
            //adds the lap
            Lap_Counter.Text = $"{Current_Lap}/{Total_Laps}";
            //lap counter
            List<Laps> Current_LapInfo_Temp = new List<Laps>();
            List<Laps> Current_LapInfo = new List<Laps>();
            for (int i = 0; i < Laps.Count; i++)//gets all the current lap intfo
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

            Current_LapInfo = Current_LapInfo_Temp.OrderBy(lap => lap.date_start).ToList();
           
            Update_LeaderBoard(Current_LapInfo);
            //update the leaderboard


            for(int i = 0;i < FlexCharts.Count; i++)//make sure there is a chart to update
            {
                if (FlexCharts[i].Count() == 0)
                { }
                else { Update_Chart(i); }//update the charts
            }

            if (Current_Lap >= Total_Laps)
            {
                Race_Timer.Stop();//Checkered flag
            }

        }

        private void Resume_Click(object sender, EventArgs e)//resumes the timer
        {
            Race_Timer.Start();
            Race_Start.Enabled = false;
        }

        private void Pause_Click(object sender, EventArgs e)//stops the timer
        {
            Race_Timer.Stop();
            Race_Start.Enabled = true;
        }

        private void Line_Graph_Click(object sender, EventArgs e)//When you add a line graph to the box
        {
            Panel Area = Line_Graph.Parent.Parent as Panel;
            int Width = Area.Width;
            int Height = Area.Height;
            //Creates the graph to fit snug inside the boc
            C1.Win.Chart.FlexChart Chart = new C1.Win.Chart.FlexChart();
            Chart.Width = Width - 55;
            Chart.Height = Height - 20;
            Chart.Location = new Point(30, 10);
            Chart.ChartType = ChartType.LineSymbols;//line chart
            
            //checks to see if there is already a chart
            if (FlexCharts[int.Parse(Area.Name)].Count() == 1)
            {
                C1.Win.Chart.FlexChart Old_Chart = FlexCharts[int.Parse(Area.Name)][0];//make the old chart into a chart
                Old_Chart.Dispose();//the kill it 
                FlexCharts[int.Parse(Area.Name)].Clear();
                FlexCharts[int.Parse(Area.Name)].Add(Chart);//add the new chart
            }
            else//if not just add it 
            {
                FlexCharts[int.Parse(Area.Name)].Add(Chart);
            }
            Area.Controls.Add(Chart);

        }

        private void Bar_Chart_Click(object sender, EventArgs e)//exactly the same as the function above :) THE END
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

        private void Close_Graph_Click(object sender, EventArgs e)//////// All the rest is just hiding things when buuttons are pressed 
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
//FIN