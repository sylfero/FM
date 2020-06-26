using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.ENCJE
{
    class LeagueTable
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int Played { get; set; }
        public int ScoredGoals { get; set; }
        public int LostGoals { get; set; }
        public int Wins { get; set; }
        public int Lost { get; set; }
        public int Draws { get; set; }

        public LeagueTable(SQLiteDataReader reader)
        {
            Points = Convert.ToInt32(reader["points"].ToString());
            Played = Convert.ToInt32(reader["played"].ToString());
            ScoredGoals = Convert.ToInt32(reader["scored_goals"].ToString());
            LostGoals = Convert.ToInt32(reader["lost_goals"].ToString());
            Wins = Convert.ToInt32(reader["wins"].ToString());
            Lost = Convert.ToInt32(reader["lost"].ToString());
            Draws = Convert.ToInt32(reader["draws"].ToString());
        }

        public LeagueTable(int id, string name, int points, int played, int scoredGoals, int lostGoals, int wins, int lost, int draws)
        {
            Id = id;
            Points = points;
            Played = played;
            ScoredGoals = scoredGoals;
            LostGoals = lostGoals;
            Wins = wins;
            Lost = lost;
            Draws = draws;
        }

    }
}
