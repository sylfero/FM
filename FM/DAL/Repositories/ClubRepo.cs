using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;
    using System.Windows;
    using System.ComponentModel;

    static class ClubRepo
    {
        public static List<Club> GetAllClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.Connection)
            {

                SQLiteCommand command = new SQLiteCommand("select club.id as id, club.name as name, l.name as league, overall, budget, salaryBudget, coach from club, league l where club.league = l.id", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.Read())
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select club.id as id, club.name as name, club.league as leagueId, l.name as league, overall, budget, salaryBudget, coach from club, league l where club.league = l.id and l.name = \"Bundesliga\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clubs.Add(new Club(reader));
                }
                connection.Close();
            }

            return clubs;
        }
                
        public static List<Club> GetAllClubsIn(int id)
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select * from club where league = {id}", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clubs.Add(new Club(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString(), leagueId: int.Parse(reader["league"].ToString())));
                }
                connection.Close();
            }

            return clubs;
        }

        public static List<Club> GetPremierLeagueClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select club.id as id, club.name as name, club.league as leagueId, l.name as league, overall, budget, salaryBudget, coach from club, league l where club.league = l.id and l.name = \"Premier League\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    clubs.Add(new Club(reader));
                }
                connection.Close();
            }

            return clubs;
        }

        public static void TransferToClub(int playerId, string oldClub, string newClubName, int transferCost, int playerSalary, string playerContract, int playerValue, int playerActuallSalary)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select budget, salaryBudget from club where name = \"{newClubName}\"", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                double clubBudget = 0;
                double clubSalaryBudget = 0;
                while(reader.Read())
                {
                    clubBudget = Convert.ToDouble(reader["budget"].ToString());
                    clubSalaryBudget = Convert.ToDouble(reader["salaryBudget"].ToString());
                }
                reader.Close();

                if (transferCost <= clubBudget && playerSalary <= clubSalaryBudget)
                {
                    var r = new Random();
                    int szansa = r.Next(1, 100);
                    if((szansa <= 90  && transferCost >= 1.5*playerValue) || (szansa <= 55 && transferCost >= 1.1 * playerValue) || (szansa <= 20 && transferCost < 1.1*playerValue))
                    {
                        szansa = r.Next(1, 100);
                        if((szansa <= 90 && playerSalary >= 1.5 * playerActuallSalary) || (szansa <= 55 && playerSalary >= 1.1 * playerActuallSalary) || (szansa <= 20 && playerSalary < 1.1 * playerActuallSalary))
                        {
                            TransferFromClub(oldClub, transferCost, playerSalary);
                            PlayerRepo.PlayerTransfer(playerId, playerSalary, newClubName, playerContract);
                            SQLiteCommand command2 = new SQLiteCommand($"UPDATE club set budget = budget - {transferCost}, salaryBudget = salaryBudget - {playerSalary} where name = \"{newClubName}\"", connection);
                            command2.ExecuteNonQuery();
                            MessageBox.Show("Congratulations, You bought this player!!!");
                        }
                        else
                        {
                            MessageBox.Show("This player doesn't want to go to your club");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"{oldClub} doesn't want to sell this player");
                    }
                    
                }
                else
                    MessageBox.Show("You can't afford this player");
                connection.Close();
            }
        }

        public static void TransferFromClub(string clubName, int transferCost, int playerSalary)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE club set budget = budget + {transferCost}, salaryBudget = salaryBudget + {playerSalary} where name = \"{clubName}\"", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
