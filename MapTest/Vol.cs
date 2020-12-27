using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapTest
{
    public class VolLive
    {
        public double latitude{ get; set; }
        public double longitude{ get; set; }
        public double altitude{ get; set; }
        public int direction{ get; set; }
    }

    public class Vol
    {
        public DateTime flight_date{ get; set; }
        public string flight_status { get; set; }
        public VolLive live { get; set; }
    }

    public class ListeVol
    {
        public List<Vol> data { get; set; } = new List<Vol>();
    }
}