using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace League_Tracker.Models
{
    public class Summoner
    {
        public string? accountId { get; set; }
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public string? name { get; set; }
        public string? id { get; set; }
        public string? puuid { get; set; }
        public long summonerLevel { get; set; }
    }
}

