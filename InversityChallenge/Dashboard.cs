using C1.Win.Chart;
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
        public async Task<List<Meetings>> GetMeetings(string uri)//Function that Calls to the API
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

        public async Task<List<Sessions>> GetSessions(string uri)//Function that Calls to the API
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

        private async void Dashboard_Load(object sender, EventArgs e)
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
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            label1.Text = this.Size.ToString();
        }

        private async void Race_Select_SelectedIndexChanged(object sender, EventArgs e)
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

        private void Layout_Button_Click(object sender, EventArgs e)
        {

        }

        private void Choose_Graph_Click(object sender, EventArgs e)
        {
            FlowLayoutPanel Chart_Type = new FlowLayoutPanel();
            Chart_Type.Name = "Chart_Type";
            Chart_Type.Width = 106;
            Chart_Type.Height = 96;
            Chart_Type.BorderStyle = BorderStyle.FixedSingle;
            Chart_Type.Location = new Point(Choose_Graph.Width -1, 5);
            
            Button Line = new Button();
            Line.Width = 100;
            Line.Height = 40;
            Line.Text = "Line Graph";
            Line.Click += Line_Click;
            Button Pie = new Button();
            Pie.Width = 100;
            Pie.Height = 40;
            Pie.Text = "Pie Chart";
            Pie.Click += Pie_Click;
            Chart_Type.Controls.Add(Pie);
            Chart_Type.Controls.Add(Line);
            Test_Panel.Controls.Add(Chart_Type);
        }

        private void Line_Click(object? sender, EventArgs e)
        {
            for(int i =0;i<Test_Panel.Controls.Count;i++)
            {
                if (Test_Panel.Controls[i].Name == "Chart_Type")
                {
                    Test_Panel.Controls.Remove(Test_Panel.Controls[i]);
                }
            }
            FlexChart flexChart = new FlexChart();
            flexChart.Width = Test_Panel.Width - 40;
            flexChart.Height = Test_Panel.Height - 10;
            flexChart.Location = new Point(Choose_Graph.Width + 5,5);
            Test_Panel.Controls.Add(flexChart);
        }

        private void Pie_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void treeMap1_Click(object sender, EventArgs e)
        {

        }
    }
}
