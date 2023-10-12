using League_Tracker.API;
using League_Tracker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace League_Tracker.ViewModels
{
    public partial class SummonerWindow : Window
    {
        // variables :
        const string configPath = "./Config/Config.Json";
        const string historyPath = "./Config/SummonerHistory.txt";
        Random random = new Random();
        Config? config;

        public SummonerWindow()
        {
            InitializeComponent();
            if (File.Exists(configPath)) InitializeWindow();
            if (File.Exists(historyPath)) SummonerHistorySetup();
        }
        

        // Interface methods :
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        // Main methods :
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveSummonerName();

                var selectedItem = (ComboBoxItem)Combobox_Region.SelectedValue;
                var region = selectedItem.Content.ToString();
                if (config == null || region == null) return;
                var api = new RiotAPI(config.API_KEY, region);

                // Summoner :
                var summoner = api.GetRiotAPIResultSummonerAsync(textbox_SummonerName.Text);
                if (summoner != null) ProfileSetup(summoner);
                else ResetSummoner();

                // Rank :
                if (summoner == null) return;
                var league = api.GetRiotAPIResultLeagueAsync(summoner.id);
                if (league != null && league.Count > 0) RankSetup(league[0]);
                else ResetRank();

                // Champion mastery :
                var champion = api.GetRiotAPIResultChampionAsync(summoner.id);
                if (champion != null && champion.Count > 0)
                {
                    var list = api.GetRiotAPIResultCHampionListAsync();
                    var champion1 = list.data.Where(x => x.Value.key == champion[0].championId.ToString()).Select(x => x.Value.name).FirstOrDefault();
                    var champion2 = list.data.Where(x => x.Value.key == champion[1].championId.ToString()).Select(x => x.Value.name).FirstOrDefault();
                    var champion3 = list.data.Where(x => x.Value.key == champion[2].championId.ToString()).Select(x => x.Value.name).FirstOrDefault();
                    if(champion1 != null && champion2 != null && champion3 != null) ChampionSetup(champion, champion1, champion2, champion3);
                }
                else ResetChampion();

                // Last match :
                var matches = api.GetRiotAPIResultMatchesAsync(summoner.puuid);
                if (matches != null && matches.info != null)
                {
                    var player = matches.info.participants.Where(x => x.puuid == summoner.puuid).FirstOrDefault();
                    var championList = api.GetRiotAPIResultCHampionListAsync();
                    var champions = championList.data.Where(x => x.Value.key == player.championId.ToString()).Select(x => x.Value.name).FirstOrDefault();

                    champions = champions.Replace(" ", "");
                    var spells = api.GetRiotAPIResultSpellsAsync(champions);
                    if(player != null && champions != null && spells != null) GameSetup(spells, champions, player, matches.info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void Combobox_Memoire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combobox_Memoire.SelectedIndex >= 0 && Combobox_Memoire.SelectedIndex < Combobox_Memoire.Items.Count)
            {
                textbox_SummonerName.Text = Combobox_Memoire.SelectedItem.ToString();
            }
        }


        // Sub methods :
        private void SummonerHistorySetup()
        {
            Combobox_Memoire.Items.Clear();
            foreach (string line in File.ReadAllLines(historyPath)) Combobox_Memoire.Items.Add(line);
        }
        private void InitializeWindow()
        {
            config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));
            if (config != null) textbox_SummonerName.Text = config.DefaultUser;

            Image_Logo.Source = GetRelativeBitmap("./Icons/logo.png");
            Image_rank.Source = GetRelativeBitmap("./Icons/IRON.png");
            Image_Champ1.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_Champ2.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_Champ3.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_spell1.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_spell2.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_spell3.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_spell4.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_Profile.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_GameChamp.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");

            Txt_summonerName.Text = "Undefined";
            Txt_Ratio.Text = "Ratio : 0%";
            Txt_Loose.Text = "Loose : 0";
            Txt_rank.Text = "Undefined";
            Txt_Win.Text = "Win : 0";
            Txt_lp.Text = "0 LP";

            Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85746b"));
        }
        private void SaveSummonerName()
        {
            if (File.Exists(historyPath))
            {
                int maxLines = 10;
                string summoner = textbox_SummonerName.Text;
                string[] lines = File.ReadAllLines(historyPath);

                foreach (string line in lines) if (line == summoner) return;

                if (lines.Length == 0) using (StreamWriter writer = new StreamWriter(historyPath)) writer.WriteLine(summoner);
                else
                {
                    lines[0] = summoner + "\n" + lines[0];
                    File.WriteAllLines(historyPath, lines.Take(maxLines));
                }
            }
            SummonerHistorySetup();
        }
        private string TimerConvertor(long value)
        {
            if (value != 0)
            {
                long minutes = value / 60;
                long seconds = value % 60;
                return $"{minutes:00}:{seconds:00}";
            }
            return "00:00";
        }
        private ImageSource GetBitmap(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
        private ImageSource GetRelativeBitmap(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        private void RankSetup(League league)
        {
            try
            {
                Image_rank.Source = GetRelativeBitmap("./Icons/" + league.tier + ".png");

                Txt_lp.Text = league.leaguePoints + " LP";
                Txt_rank.Text = league.tier + " " + league.rank;
                Txt_Win.Text = "Win : " + league.wins.ToString();
                Txt_Loose.Text = "Loose : " + league.losses.ToString();
                Txt_Ratio.Text = "Ratio : " + (int)(((double)league.wins / ((double)league.losses + (double)league.wins)) * 100) + " %";

                if (league.tier == "IRON") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85746b"));
                if (league.tier == "BRONZE") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b06238"));
                if (league.tier == "SILVER") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b5b5b5"));
                if (league.tier == "GOLD") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5b145"));
                if (league.tier == "PLATINUM") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#39a380"));
                if (league.tier == "EMERALD") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4da339"));
                if (league.tier == "DIAMOND") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2085e3"));
                if (league.tier == "MASTER") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b74bbd"));
                if (league.tier == "GRANDMASTER") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bd4b4b"));
                if (league.tier == "CHALLENGER") Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f5b145"));
            }
            catch { ResetRank(); }

        }
        private void ProfileSetup(Summoner? summoner)
        {
            try
            {
                if(summoner == null)
                {
                    Image_Profile.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/profileicon/" + summoner.profileIconId + ".png");
                    Txt_Lvl.Text = summoner.summonerLevel.ToString();
                    Txt_summonerName.Text = summoner.name;
                }
            }
            catch { ResetSummoner(); }
        }
        private void ChampionSetup(List<Champion> champion, string name1, string name2, string name3)
        {
            try
            {
                name1 = name1.Replace(" ", "");
                name2 = name2.Replace(" ", "");
                name3 = name3.Replace(" ", "");

                Image_Champ1.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/champion/" + name1 + ".png");
                Image_Champ2.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/champion/" + name2 + ".png");
                Image_Champ3.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/champion/" + name3 + ".png");

                Txt_champ1.Text = champion[0].championPoints.ToString("#,0") + " pts";
                Txt_champ2.Text = champion[1].championPoints.ToString("#,0") + " pts";
                Txt_champ3.Text = champion[2].championPoints.ToString("#,0") + " pts";
            }
            catch { ResetChampion(); }
        }
        private void GameSetup(List<string> spells, string name, participantsDto player, InfoDto gameInfo)
        {
            try
            {
                Image_GameChamp.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/champion/" + name + ".png");
                Image_spell1.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/spell/" + spells[0] + ".png");
                Image_spell2.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/spell/" + spells[1] + ".png");
                Image_spell3.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/spell/" + spells[2] + ".png");
                Image_spell4.ImageSource = GetBitmap("https://ddragon.leagueoflegends.com/cdn/13.19.1/img/spell/" + spells[3] + ".png");

                Txt_Kda.Text = player.kills.ToString() + " / " + player.deaths.ToString() + " / " + player.assists.ToString();
                Txt_Victory.Text = (player.win == true ? "Victory" : "Defeat");
                Txt_wards.Text = "Score " + player.visionScore.ToString();
                Txt_Timer.Text = TimerConvertor(gameInfo.gameDuration);
                Txt_spell1.Text = "x" + player.spell1Casts.ToString();
                Txt_spell2.Text = "x" + player.spell2Casts.ToString();
                Txt_spell3.Text = "x" + player.spell3Casts.ToString();
                Txt_spell4.Text = "x" + player.spell4Casts.ToString();
                Txt_GameLvl.Text = player.champLevel.ToString();
            }
            catch { ResetLastGame(); }
        }


        // Reset methods :
        private void ResetSummoner()
        {
            Image_Profile.ImageSource = null;

            Txt_summonerName.Text = "User not found";
            Txt_Lvl.Text = "0";
        }
        private void ResetChampion()
        {
            Image_Champ1.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_Champ2.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");
            Image_Champ3.ImageSource = GetRelativeBitmap("./Icons/icon" + random.Next(1, 5) + ".png");

            Txt_champ1.Text = "0 pts";
            Txt_champ2.Text = "0 pts";
            Txt_champ3.Text = "0 pts";
        }
        private void ResetLastGame()
        {
            Image_GameChamp.ImageSource = null;
            Image_spell1.ImageSource = null;
            Image_spell2.ImageSource = null;
            Image_spell3.ImageSource = null;
            Image_spell4.ImageSource = null;

            Txt_spell1.Text = "x0";
            Txt_spell2.Text = "x0";
            Txt_spell3.Text = "x0";
            Txt_spell4.Text = "x0";
            Txt_GameLvl.Text = "0";
            Txt_Kda.Text = "0 / 0 / 0";
            Txt_Timer.Text = "00:00";
            Txt_Victory.Text = "Undefined";
            Txt_wards.Text = "0";
        }
        private void ResetRank()
        {
            Image_rank.Source = GetRelativeBitmap("./Icons/IRON.png");

            Txt_rank.Text = "undefined";
            Txt_lp.Text = "0 LP";
            Txt_Win.Text = "Win : 0";
            Txt_Loose.Text = "Loose : 0";
            Txt_Ratio.Text = "Ratio : 0%";

            Txt_rank.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#85746b"));
        }






    }
}
