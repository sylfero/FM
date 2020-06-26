using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.ENCJE
{
    class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string League { get; set; }
        public int Overall { get; set; }
        public double Budget { get; set; }
        public double SalaryBudget { get; set; }
        public string Coach { get; set; }
        public int Points { get; set; }
        public int Played { get; set; }
        public int ScoredGoals { get; set; }
        public int LostGoals { get; set; }
        public int Wins { get; set; }
        public int Lost { get; set; }
        public int Draws { get; set; }

        public Club(MySqlDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Name = reader["name"].ToString();
            League = reader["league"].ToString();
            Overall = Convert.ToInt32(reader["overall"].ToString());
            Budget = Convert.ToDouble(reader["budget"].ToString());
            SalaryBudget = Convert.ToDouble(reader["salarybudget"].ToString());
            Coach = reader["coach"].ToString();
            Points = Convert.ToInt32(reader["points"].ToString());
            Played = Convert.ToInt32(reader["played"].ToString());
            ScoredGoals = Convert.ToInt32(reader["scored_goals"].ToString());
            LostGoals = Convert.ToInt32(reader["lost_goals"].ToString());
            Wins = Convert.ToInt32(reader["wins"].ToString());
            Lost = Convert.ToInt32(reader["lost"].ToString());
            Draws = Convert.ToInt32(reader["draws"].ToString());
        }

        public Club(int id, string name, string league, int overall, double budget, double salaryBudget, string coach, int points, int played, int scoredGoals, int lostGoals, int wins, int lost, int draws)
        {
            Id = id;
            Name = name;
            League = league;
            Overall = overall;
            Budget = budget;
            SalaryBudget = salaryBudget;
            Coach = coach;
            Points = points;
            Played = played;
            ScoredGoals = scoredGoals;
            LostGoals = lostGoals;
            Wins = wins;
            Lost = lost;
            Draws = draws;
        }

        public Club(string name, int points, int played, int scoredGoals, int lostGoals, int wins, int lost, int draws)
        {
            Name = name;
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
