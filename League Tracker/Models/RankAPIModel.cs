﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace League_Tracker.Models
{
    public class League
    {
        public string? leagueId { get; set; }
        public string? queueType { get; set; }
        public string? tier { get; set; }
        public string? rank { get; set; }
        public string? summonerId { get; set; }
        public string? summonerName { get; set; }
        public int leaguePoints { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public bool veteran { get; set; }
        public bool inactive { get; set; }
        public bool freshBlood { get; set; }
        public bool hotStreak { get; set; }
    }
}

