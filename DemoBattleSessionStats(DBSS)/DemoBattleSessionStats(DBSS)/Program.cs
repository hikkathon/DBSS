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

                float wins = (float)(account.data.info.statistics.all.wins - accountStartSession.data.info.statistics.all.wins);
                float battles = (float)(account.data.info.statistics.all.battles - accountStartSession.data.info.statistics.all.battles);
                float frags = (float)(account.data.info.statistics.all.frags - accountStartSession.data.info.statistics.all.frags);
                float survived_battles = (float)(account.data.info.statistics.all.survived_battles - accountStartSession.data.info.statistics.all.survived_battles);
                float hits = (float)(account.data.info.statistics.all.hits - accountStartSession.data.info.statistics.all.hits);
                float shots = (float)(account.data.info.statistics.all.shots - accountStartSession.data.info.statistics.all.shots);
                float deaths = (float)(battles - survived_battles);
                float damage_dealt = (float)(account.data.info.statistics.all.damage_dealt - accountStartSession.data.info.statistics.all.damage_dealt);
                float damage_received = (float)(account.data.info.statistics.all.damage_received - accountStartSession.data.info.statistics.all.damage_received);
                float spotted = (float)(account.data.info.statistics.all.spotted - accountStartSession.data.info.statistics.all.spotted);
                float dropped_capture_points = (float)(account.data.info.statistics.all.dropped_capture_points - accountStartSession.data.info.statistics.all.dropped_capture_points);
                float capture_points = (float)(account.data.info.statistics.all.capture_points - accountStartSession.data.info.statistics.all.capture_points);
                float winRate = (float)((wins / battles) * 100);
                update_time = sw.ElapsedMilliseconds;
                sw.Stop();
                Console.WriteLine($"User nickname:\t{account.data.info.nickname}\n" +
                    $"Wins/Battles:\t{wins} ({battles})\t({Math.Round((wins / battles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Kills:\t\t{frags}\t({Math.Round(frags / battles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Deaths:\t\t{deaths}\t({Math.Round((deaths / battles) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Hits/Shots:\t{hits}/{shots}\t({Math.Round((hits / shots) * 100, 2).ToString().Replace("не число", "0")}%)\n" +
                    $"Damage Dealt:\t{damage_dealt}\t({Math.Round(damage_dealt / battles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Damage Received:{damage_received}\t({Math.Round(damage_received / battles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Spotted:\t{spotted}\t({Math.Round(spotted / battles, 2).ToString().Replace("не число", "0")})\n" +
                    $"Defence:\t{dropped_capture_points}\t({Math.Round(dropped_capture_points / battles, 0).ToString().Replace("не число", "0")})\n" +
                    $"Capture:\t{capture_points}\t({Math.Round(capture_points / battles, 0).ToString().Replace("не число", "0")})\n\n\n\n" +"update: "+ count + $" ({update_time} ms.)");
                Console.WriteLine("\n\n\nPowered by hikkathon");
                Console.WriteLine("\n\n\n(C)BLITZBURY & BEST OF BLITZ | Моды WoT Blitz\n\thttps://vk.com/bestofblitz\n\t\t2021г.");
            }
        }
    }
}
