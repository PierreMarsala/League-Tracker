using League_Tracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace League_Tracker.API
{
    public class RiotAPI
    {
        private string key { get; set; }
        private string region { get; set; }

        public RiotAPI(string key, string region)
        {
            this.key = key;
            this.region = region;
        }

        // Summoner :
        public Summoner? GetRiotAPIResultSummonerAsync(string summoner)
        {
            try
            {
                var url = "https://" + region + ".api.riotgames.com/lol/summoner/v4/summoners/by-name/" + summoner + "?api_key=" + key;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<Summoner>(result);
                }
            }
            catch { return new Summoner(); }
        }

        // Rank :
        public List<League>? GetRiotAPIResultLeagueAsync(string summoner)
        {
            try
            {
                var url = "https://" + region + ".api.riotgames.com/lol/league/v4/entries/by-summoner/" + summoner + "?api_key=" + key;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<League>>(result);
                }
            }
            catch { return new List<League>(); }
        }

        // Champion :
        public List<Champion>? GetRiotAPIResultChampionAsync(string summoner)
        {
            try
            {
                var url = "https://" + region + ".api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/" + summoner + "/top" + "?api_key=" + key;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<List<Champion>>(result);
                }
            }
            catch { return new List<Champion>(); }
        }

        // Matches :
        public Matches? GetRiotAPIResultMatchesAsync(string summoner)
        {
            try
            {
                var url = "https://EUROPE.api.riotgames.com/lol/match/v5/matches/by-puuid/" + summoner + "/ids" + "?api_key=" + key;
                string lastGame = string.Empty;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    List<string>? list = JsonSerializer.Deserialize<List<string>>(result);
                    if (list != null && list.Count > 0) lastGame = list[0];
                }

                url = "https://europe.api.riotgames.com/lol/match/v5/matches/" + lastGame + "?api_key=" + key;
                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<Matches>(result);
                }
            }
            catch { return new Matches(); }
        }

        // Spells
        public List<string> GetRiotAPIResultSpellsAsync(string champion)
        {
            try
            {
                var url = "https://ddragon.leagueoflegends.com/cdn/13.19.1/data/en_US/champion/" + champion + ".json?api_key=" + key;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    var document = JsonDocument.Parse(result);
                    JsonElement rakanSpells = document.RootElement
                                .GetProperty("data")
                                .GetProperty(champion)
                                .GetProperty("spells");

                    List<string> spellList = new List<string>();
                    foreach (JsonElement spell in rakanSpells.EnumerateArray())
                    {
                        spellList.Add(spell.GetProperty("id").ToString());
                    }
                    return spellList;
                }
            }
            catch
            {
                return null;
            }
        }

        // Get champion list :
        public ChampionData? GetRiotAPIResultCHampionListAsync()
        {
            try
            {
                var url = "http://ddragon.leagueoflegends.com/cdn/13.19.1/data/en_US/champion.json" + "?api_key=" + key;

                using (HttpClient client = new HttpClient())
                {
                    var result = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return JsonSerializer.Deserialize<ChampionData>(result);
                }
            }
            catch { return new ChampionData(); }
        }
    }
}
