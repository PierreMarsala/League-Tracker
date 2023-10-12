using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace League_Tracker.Models
{
    public class Config
    {
        public required string API_KEY { get; set; }
        public required int RateLimit_Per10s { get; set; }
        public required int RateLimit_Per10m { get; set; }
        public required string DefaultUser { get; set; }
    }
}
