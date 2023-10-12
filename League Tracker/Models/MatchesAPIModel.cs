using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace League_Tracker.Models
{
    public class Matches
    {
        public List<string>? matches { get; set; }
        public MetadataDto? metadata { get; set; }
        public InfoDto? info { get; set; }
    }

    public class MetadataDto 
    {
        public string? dataVersion { get; set; }
        public string? matchId { get; set; }
        public List<string>? participants { get; set; }
    }

    public class InfoDto
    {
        public long gameCreation { get; set; }
        public long gameDuration { get; set; }
        public long gameEndTimestamp { get; set; }
        public long gameId { get; set; }
        public string? gameMode { get; set; }
        public string? gameName { get; set; }
        public long gameStartTimestamp { get; set; }
        public string? gameType { get; set; }
        public string? gameVersion { get; set; }
        public int mapId { get; set; }
        public List<participantsDto>? participants { get; set; }
        public string? platformId { get; set; }
        public int queueId { get; set; }
    }

    public class participantsDto
    {
        public int championId { get; set; }
        public string? puuid { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int detectorWardsPlaced { get; set; }
        public int assists { get; set; }
        public int champLevel { get; set; }
        public int killingSprees { get; set; }
        public int spell1Casts { get; set; }
        public int spell2Casts { get; set; }
        public int spell3Casts { get; set; }
        public int spell4Casts { get; set; }
        public int summoner1Casts { get; set; }
        public int summoner2Casts { get; set; }
        public int summoner1Id { get; set; }
        public int summoner2Id { get; set; }
        public int visionScore { get; set; }
        public bool win { get; set; }

    }
}

