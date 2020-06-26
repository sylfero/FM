using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repozytoria
{
    using ENCJE;
    using MySql.Data.MySqlClient;

    class RepozytoriumClub
    {
        public static List<Club> GetAllClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select * from club", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.HasRows)
                {
                    clubs.Add(new Club(reader));
                }
                connection.Close();
            }

            return clubs;
        }

        public static List<Club> GetBundesligaClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select * from club where league = \"Bundesliga\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    clubs.Add(new Club(reader));
                }
                connection.Close();
            }

            return clubs;
        }

        public static List<Club> GetPremierLeagueClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select * from club where league = \"Premier League\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    clubs.Add(new Club(reader));
                }
                connection.Close();
            }

            return clubs;
        }

        public static List<Club> GetBundesligaTable()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select name, points, played, scored_goals, lost_goals, wins, lost, draws from club where league = \"Bundesliga\" order by points desc, scored_goals desc, lost_goals asc", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    clubs.Add(
                        new Club(
                            reader["name"].ToString(),
                            Convert.ToInt32(reader["points"].ToString()),
                            Convert.ToInt32(reader["played"].ToString()),
                            Convert.ToInt32(reader["scored_goals"].ToString()),
                            Convert.ToInt32(reader["lost_goals"].ToString()),
                            Convert.ToInt32(reader["wins"].ToString()),
                            Convert.ToInt32(reader["lost"].ToString()),
                            Convert.ToInt32(reader["draws"].ToString())
                            ));
                }
                connection.Close();
            }

            return clubs;
        }

        public static List<Club> GetPremierLeagueTable()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select name, points, played, scored_goals, lost_goals, wins, lost, draws from club where league = \"Premier League\" order by points desc, scored_goals desc, lost_goals asc", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    clubs.Add(
                        new Club(
                            reader["name"].ToString(),
                            Convert.ToInt32(reader["points"].ToString()),
                            Convert.ToInt32(reader["played"].ToString()),
                            Convert.ToInt32(reader["scored_goals"].ToString()),
                            Convert.ToInt32(reader["lost_goals"].ToString()),
                            Convert.ToInt32(reader["wins"].ToString()),
                            Convert.ToInt32(reader["lost"].ToString()),
                            Convert.ToInt32(reader["draws"].ToString())
                            ));
                }
                connection.Close();
            }

            return clubs;
        }

        public void ClubWins(int id, int scoredGoals, int lostGoals)
        {
            string update = $"UPDATE club set played = played + 1, points = points + 3, scored_goals = scored_goals + {scoredGoals}, lost_goals = lost_goals + {lostGoals}, wins = wins + 1 where id = {id}";
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand(update, connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ClubLoses(int id, int scoredGoals, int lostGoals)
        {
            string update = $"UPDATE club set played = played + 1, scored_goals = scored_goals + {scoredGoals}, lost_goals = lost_goals + {lostGoals}, lost = lost + 1 where id = {id}";
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand(update, connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ClubsDraws(int hostId, int visitorId, int scoredGoals, int lostGoals)
        {
            string update = $"UPDATE club set played = played + 1, points = points + 1, scored_goals = scored_goals + {scoredGoals}, lost_goals = lost_goals + {lostGoals}, draws = draws + 1 where id = {hostId} or id = {visitorId}";
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand(update, connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void TransferToClub(int clubId, int transferCost, int playerSalary)
        {
            double clubBudget = 0;
            double clubSalaryBudget = 0;
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand($"select budget, salaryBudget from club where id = {clubId}", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                clubBudget = Convert.ToDouble(reader["budget"].ToString());
                clubSalaryBudget = Convert.ToDouble(reader["salaryBudget"].ToString());

                if(transferCost <= clubBudget && playerSalary <= clubSalaryBudget)
                {
                    MySqlCommand command2 = new MySqlCommand($"UPDATE club set budget = budget - {transferCost}, salaryBudget = salaryBudget - {playerSalary} where id = {clubId}", connection);
                    var reader2 = command2.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void TransferFromClub(int clubId, int transferCost, int playerSalary)
        {
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE club set budget = budget + {transferCost}, salaryBudget = salaryBudget + {playerSalary} where id = {clubId}", connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
