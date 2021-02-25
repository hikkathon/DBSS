using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoBattleSessionStats_DBSS_
{
    class Program
    {
        public static bool check = true;
        public static int count = 0;
        public static string account_id;
        static float update_time = 0f;
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            Console.Title = "[DEMO] Session stat for WoTBlitz";
            Console.Write("Enter WGID: ");
            account_id = Console.ReadLine();
            while (true)
            {
                Task.Run(() => MainAsync());
                Thread.Sleep(10000);
                count++;
                sw.Restart();
                Console.Clear();
            }

        }

        static async Task MainAsync()
        {
            sw.Start();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.wotblitz.ru/");
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("application_id", "8c4eecab18df2fd980424d5e35dba7bd"),
                new KeyValuePair<string, string>("account_id", account_id),
                new KeyValuePair<string, string>("extra", "statistics.rating")
            });
                var result = await client.PostAsync("/wotb/account/info/", content);
                string json = await result.Content.ReadAsStringAsync();
                int startIndex = json.IndexOf(@"""data"":{""") + 9;
                int endIndex = json.LastIndexOf(@""":{""statistics""");
                var str = json.Remove(startIndex, endIndex - startIndex).Insert(startIndex, "info");
                var account = JsonConvert.DeserializeObject<Account>(str);
                var accountStartSession = File.Exists("account.json") ? JsonConvert.DeserializeObject<Account>(File.ReadAllText("account.json")) : JsonConvert.DeserializeObject<Account>(str);

                while (check)
                {
                    File.WriteAllText("account.json", JsonConvert.SerializeObject(account));
                    check = false;
                }

                #region Статистика "Рандомные бои"
                float allWins = (float)(account.data.info.statistics.all.wins - accountStartSession.data.info.statistics.all.wins);
                float allBattles = (float)(account.data.info.statistics.all.battles - accountStartSession.data.info.statistics.all.battles);
                float allFrags = (float)(account.data.info.statistics.all.frags - accountStartSession.data.info.statistics.all.frags);
                float allSurvived_battles = (float)(account.data.info.statistics.all.survived_battles - accountStartSession.data.info.statistics.all.survived_battles);
                float allHits = (float)(account.data.info.statistics.all.hits - accountStartSession.data.info.statistics.all.hits);
                float allShots = (float)(account.data.info.statistics.all.shots - accountStartSession.data.info.statistics.all.shots);
                float allDeaths = (float)(allBattles - allSurvived_battles);
                float allDamage_dealt = (float)(account.data.info.statistics.all.damage_dealt - accountStartSession.data.info.statistics.all.damage_dealt);
                float allDamage_received = (float)(account.data.info.statistics.all.damage_received - accountStartSession.data.info.statistics.all.damage_received);
                float allSpotted = (float)(account.data.info.statistics.all.spotted - accountStartSession.data.info.statistics.all.spotted);
                float allDropped_capture_points = (float)(account.data.info.statistics.all.dropped_capture_points - accountStartSession.data.info.statistics.all.dropped_capture_points);
                float allCapture_points = (float)(account.data.info.statistics.all.capture_points - accountStartSession.data.info.statistics.all.capture_points);
                #endregion

                #region Статистика "Рейтинговые бои"
                float ratingWins = (float)(account.data.info.statistics.rating.wins - accountStartSession.data.info.statistics.rating.wins);
                float ratingBattles = (float)(account.data.info.statistics.rating.battles - accountStartSession.data.info.statistics.rating.battles);
                float ratingFrags = (float)(account.data.info.statistics.rating.frags - accountStartSession.data.info.statistics.rating.frags);
                float ratingSurvived_battles = (float)(account.data.info.statistics.rating.survived_battles - accountStartSession.data.info.statistics.rating.survived_battles);
                float ratingHits = (float)(account.data.info.statistics.rating.hits - accountStartSession.data.info.statistics.rating.hits);
                float ratingShots = (float)(account.data.info.statistics.rating.shots - accountStartSession.data.info.statistics.rating.shots);
                float ratingDeaths = (float)(ratingBattles - ratingSurvived_battles);
                float ratingDamage_dealt = (float)(account.data.info.statistics.rating.damage_dealt - accountStartSession.data.info.statistics.rating.damage_dealt);
                float ratingDamage_received = (float)(account.data.info.statistics.rating.damage_received - accountStartSession.data.info.statistics.rating.damage_received);
                float ratingSpotted = (float)(account.data.info.statistics.rating.spotted - accountStartSession.data.info.statistics.rating.spotted);
                float ratingDropped_capture_points = (float)(account.data.info.statistics.rating.dropped_capture_points - accountStartSession.data.info.statistics.rating.dropped_capture_points);
                float ratingCapture_points = (float)(account.data.info.statistics.rating.capture_points - accountStartSession.data.info.statistics.rating.capture_points);
                #endregion

                float winRate = (float)((allWins / allBattles) * 100);
                update_time = sw.ElapsedMilliseconds;
                sw.Stop();
                Console.WriteLine("ОБЫЧНЫЙ");
                Console.WriteLine($"User nickname:\t{account.data.info.nickname}\n" +
                    $"Wins/Battles:\t{allWins} ({allBattles})\t({Math.Round((allWins / allBattles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Kills:\t\t{allFrags}\t({Math.Round(allFrags / allBattles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Deaths:\t\t{allDeaths}\t({Math.Round((allDeaths / allBattles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Hits/Shots:\t{allHits}/{allShots}\t({Math.Round((allHits / allShots) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Damage Dealt:\t{allDamage_dealt}\t({Math.Round(allDamage_dealt / allBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Damage Received:{allDamage_received}\t({Math.Round(allDamage_received / allBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Spotted:\t{allSpotted}\t({Math.Round(allSpotted / allBattles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Defence:\t{allDropped_capture_points}\t({Math.Round(allDropped_capture_points / allBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Capture:\t{allCapture_points}\t({Math.Round(allCapture_points / allBattles, 0).ToString().Replace("не число", "0")})");
                Console.WriteLine("\nРЕЙТИНГОВЫЙ");
                Console.WriteLine($"Wins/Battles:\t{ratingWins} ({ratingBattles})\t({Math.Round((ratingWins / ratingBattles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Kills:\t\t{ratingFrags}\t({Math.Round(ratingFrags / ratingBattles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Deaths:\t\t{ratingDeaths}\t({Math.Round((ratingDeaths / ratingBattles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Hits/Shots:\t{ratingHits}/{ratingShots}\t({Math.Round((ratingHits / ratingShots) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Damage Dealt:\t{ratingDamage_dealt}\t({Math.Round(ratingDamage_dealt / ratingBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Damage Received:{ratingDamage_received}\t({Math.Round(ratingDamage_received / ratingBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Spotted:\t{ratingSpotted}\t({Math.Round(ratingSpotted / ratingBattles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Defence:\t{ratingDropped_capture_points}\t({Math.Round(ratingDropped_capture_points / ratingBattles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Capture:\t{ratingCapture_points}\t({Math.Round(ratingCapture_points / ratingBattles, 0).ToString().Replace("не число", "0")})\n\n\n\n");
                Console.WriteLine($"update: {count} ({update_time} ms.)");
                Console.WriteLine("\n\n\nPowered by hikkathon");
                Console.WriteLine("\n\n\n(C)BLITZBURY & BEST OF BLITZ | Моды WoT Blitz\n\thttps://vk.com/bestofblitz\n\t\t2021г.");
            }
        }
    }
}
