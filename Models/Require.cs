using System.Collections.Generic;

namespace FiscalizaDashboard.Models
{
    public class Require
    {
        public List<Datum> data { get; set; }
    }

    public class SingleRequire
    {
        public Datum data { get; set; }
    }

    public class Datum
    {
        public string description { get; set; }
        public float id { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public int user { get; set; }
        public int status { get; set; }
        public string statusDescription { get; set; }
        public string statusColor { get; set; }
    }
}




