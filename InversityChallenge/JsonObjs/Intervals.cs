using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversityF1.JsonObjs
{
    public class Intervals
    {

        public DateTime date { get; set; }
        public int driver_number { get; set; }
        public string? gap_to_leader { get; set; }
        public string? interval { get; set; }
        public int meeting_key { get; set; }
        public int session_key { get; set; }

    }
}
