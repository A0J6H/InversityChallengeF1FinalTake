using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversityChallenge.JsonObjs
{
    public class Stints
    {
        public string compound { get; set; }
        public int driver_number { get; set; }
        public int lap_end { get; set; }
        public int lap_start { get; set; }
        public int meeting_key { get; set; }
        public int session_key { get; set; }
        public int stint_number { get; set; }
        public int tyre_age_at_start { get; set; }
    }
}
