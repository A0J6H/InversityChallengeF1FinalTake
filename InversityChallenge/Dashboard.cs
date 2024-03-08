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
                Race_Select.Items.Add(Meetings[i].meeting_name);
                //Adds the 2023 Races to the combobox
            }
        }

        private void Dashboard_Resize(object sender, EventArgs e)
        {
            label1.Text = this.Size.ToString();
        }
    }
}
