using System;
using System.Collections.Generic;

namespace League_Tracker.Models
{

    public class Champion
    {
        public string? puuid { get; set; }
        public long championPointsUntilNextLevel { get; set; }
        public Boolean chestGranted { get; set; }
        public long championId { get; set; }
        public long lastPlayTime { get; set; }
        public int championLevel { get; set; }
        public string? summonerId { get; set; }
        public int championPoints { get; set; }
        public long championPointsSinceLastLevel { get; set; }
        public int tokensEarned { get; set; }
    }
    public class ChampionData
    {
        public Dictionary<string, templateChampion>? data { get; set; }
    }

    public class templateChampion
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? key { get; set; }
    }
}

