using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoBattleSessionStats_DBSS_
{
    public class Meta
    {
        public int count { get; set; }
    }

    public class Clan
    {
        public int spotted { get; set; }
        public int max_frags_tank_id { get; set; }
        public int hits { get; set; }
        public int frags { get; set; }
        public int max_xp { get; set; }
        public int max_xp_tank_id { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int capture_points { get; set; }
        public int battles { get; set; }
        public int damage_dealt { get; set; }
        public int damage_received { get; set; }
        public int max_frags { get; set; }
        public int shots { get; set; }
        public int frags8p { get; set; }
        public int xp { get; set; }
        public int win_and_survived { get; set; }
        public int survived_battles { get; set; }
        public int dropped_capture_points { get; set; }
    }

    public class Rating
    {
        public int spotted { get; set; }
        public int calibration_battles_left { get; set; }
        public int hits { get; set; }
        public int frags { get; set; }
        public int recalibration_start_time { get; set; }
        public double mm_rating { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public bool is_recalibration { get; set; }
        public int capture_points { get; set; }
        public int battles { get; set; }
        public int current_season { get; set; }
        public int damage_dealt { get; set; }
        public int damage_received { get; set; }
        public int shots { get; set; }
        public int frags8p { get; set; }
        public int xp { get; set; }
        public int win_and_survived { get; set; }
        public int survived_battles { get; set; }
        public int dropped_capture_points { get; set; }
    }

    public class All
    {
        public int spotted { get; set; }
        public int max_frags_tank_id { get; set; }
        public int hits { get; set; }
        public int frags { get; set; }
        public int max_xp { get; set; }
        public int max_xp_tank_id { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int capture_points { get; set; }
        public int battles { get; set; }
        public int damage_dealt { get; set; }
        public int damage_received { get; set; }
        public int max_frags { get; set; }
        public int shots { get; set; }
        public int frags8p { get; set; }
        public int xp { get; set; }
        public int win_and_survived { get; set; }
        public int survived_battles { get; set; }
        public int dropped_capture_points { get; set; }
    }

    public class Statistics
    {
        public Clan clan { get; set; }
        public Rating rating { get; set; }
        public All all { get; set; }
        public object frags { get; set; }
    }

    public class Info
    {
        public Statistics statistics { get; set; }
        public int account_id { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public object @private { get; set; }
        public int last_battle_time { get; set; }
        public string nickname { get; set; }
    }

    public class Data
    {
        public Info info { get; set; }
    }

    public class Account
    {
        public string status { get; set; }
        public Meta meta { get; set; }
        public Data data { get; set; }
    }
}
