using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;
    using Renci.SshNet.Messages;
    using System.Windows;
    using FM.Model;
    using Google.Protobuf.WellKnownTypes;
    using System.Collections.ObjectModel;

    static class PlayerRepo
    {
        public static List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, p.club as clubId, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential, pass, gk, isJunior, isRetiring, currPosition from players p, country n, club c where p.club = c.id and p.nationality = n.id", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static List<Player> GetBundesligaPlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, p.club as clubId, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential, pass, gk, isJunior, isRetiring, currPosition from players p, country n, club c where p.club = c.id and p.nationality = n.id and c.league = \"Bundesliga\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static List<Player> GetPremierLeaguePlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, p.club as clubId, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential, pass, gk, isJunior, isRetiring, currPosition from players p, country n, club c where p.club = c.id and p.nationality = n.id and c.league = \"Premier League\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static ObservableCollection<Player> GetPlayersFromClub(int clubId)
        {
            ObservableCollection<Player> players = new ObservableCollection<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, p.club as clubId, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential, value, salary, pass, gk, isJunior, isRetiring, currPosition from players p, country n, club c where p.club = c.id and p.nationality = n.id and c.id = {clubId} order by position asc, overall desc", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static List<Player> GetPlayersFromNationality(string nationality)
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, surname, p.club as clubId, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential, pass, gk, isJunior, isRetiring, currPosition from players p, country n, club c where p.club = c.id and p.nationality = n.id and n.name = \"{nationality}\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read()) {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static void PlayerTransfer(int playerId, int newSalary, string contractLength)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE players set salary = {newSalary}, contract_terminates = \"{contractLength}\", club = {ClubStatus.ClubId} where id = {playerId}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void PlayerNewContract(int playerId, int newSalary, string contractLength)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select salaryBudget from club where id = (select p.club from players p where p.id = {playerId})", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                double teamSalaryBudget = 0;
                while (reader.Read())
                {
                    teamSalaryBudget = Convert.ToDouble(reader["salaryBudget"].ToString());
                }
                reader.Close();
                command = new SQLiteCommand($"select salary from players where id = {playerId}", connection);
                reader = command.ExecuteReader();
                double playerSalary = 0;
                while (reader.Read())
                    playerSalary = Convert.ToDouble(reader["salary"].ToString());
                if (teamSalaryBudget >= (newSalary - playerSalary))
                {
                    var r = new Random();
                    int szansa = r.Next(1, 100);
                    if ((szansa < 90 && newSalary >= 1.5 * playerSalary) || (szansa < 55 && newSalary > 1.1 * playerSalary) || (szansa < 20 && newSalary < 1.1 * playerSalary))
                    {
                        command = new SQLiteCommand($"update players set salary = {newSalary}, contract_terminates = \"{contractLength}\" where id = {playerId}", connection);
                        command.ExecuteNonQuery();
                        command = new SQLiteCommand($"update club set salaryBudget = salaryBudget - {newSalary - playerSalary} where id = (select p.club from players p where p.id = {playerId})", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Congratulations. You extended contract with this player");
                    }
                    else
                        MessageBox.Show("This player doesnt want to extend his player");
                }
                else
                {
                    MessageBox.Show("You can't afford extend this player contract");
                }
                connection.Close();
            }
        }

        public static List<Player> GetJuniors()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select *, club as clubId from players where isJunior = 1 and club = {ClubStatus.ClubId}", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public static void SignJunior(int playerId)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE players set salary = 500, contract_terminates = \"{ClubStatus.SeasonEnd.Year + 1}-{ClubStatus.SeasonEnd.ToString("MM-dd")}\", isJunior = 0 where id = {playerId}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SignFreePlayer(int playerId, int salary, int contractLength)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE players set salary = {salary}, contract_terminates = \"{contractLength}\", club = {ClubStatus.ClubId} where id = {playerId}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void GeneratePlayer(int club, bool isJunior, int nationality, string name, string surname, string position, int overall, int potential)
        {
            DateTime date = new DateTime(ClubStatus.SeasonEnd.Year - 16, 1, 1);
            Random rnd = new Random();
            date.AddDays(rnd.Next(365));
            Dictionary<string, int> stats = Calculation.GetStats(overall, position);
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"INSERT or ignore INTO players (name, surname, dateofbirth, nationality, position, club, value, salary, contract_terminates, offense, defence, overall, potential, pass, gk, isJunior, isRetiring) VALUES (\"{name}\", \"{surname}\", \"{date:yyyy-MM-dd}\", {nationality}, \"{position}\", {club}, {Calculation.GetValue(date, overall, potential, position)}, 500, \"{ClubStatus.SeasonEnd.Year + 2}-{ClubStatus.SeasonEnd.ToString("MM-dd")}\", {stats["atk"]}, {stats["def"]}, {overall}, {potential}, {stats["pas"]}, {stats["kep"]}, {Convert.ToInt32(isJunior)}, 0)", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SwapPosition(int id1, string pos1, int id2, string pos2)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update players set currPosition = {(pos1 == null ? "null" : "\"" + pos1 + "\"")} where id = {id2}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command = new SQLiteCommand($"update players set currPosition = {(pos2 == null ? "null" : "\"" + pos2 + "\"")} where id = {id1}", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<int> GetRet()
        {
            List<int> players = new List<int>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select id from players where isRetiring = 1", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(int.Parse(reader["id"].ToString()));
                }
                connection.Close();
            }

            return players;
        }

        public static void UpdatePot()
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update players set offense = offense + 4, deffense = deffense + 4, pass = pass + 4, gk = gk + 4, potential = potential + 2 where (potential - overall) > 0", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static Dictionary<string, int> GetStats(int overall, string position)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            Random rnd = new Random();

            if (position.Equals("Defender"))
            {
                stats.Add("atk", (int)(0.3 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.5 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position.Equals("Midfielder"))
            {
                stats.Add("atk", (int)(0.7 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.7 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position.Equals("Striker"))
            {
                stats.Add("atk", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.3 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.5 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position.Equals("Goalkeeper"))
            {
                stats.Add("atk", rnd.Next(3, 10));
                stats.Add("def", rnd.Next(3, 10));
                stats.Add("pas", rnd.Next(3, 10));
                stats.Add("kep", overall);
            }

            return stats;
        }

        public static int GetValue(DateTime birth, int overall, int potential, string position)
        {
            var date = ClubStatus.CurrentDate;
            int age = date.Year - birth.Year;

            if ((birth.Month > date.Month) || (birth.Month == date.Month && birth.Day > DateTime.Now.Day))
                age--;

            int potDif = potential - overall + 1;
            double val = ((overall - 50) * 100_000 * (overall - 50) / (age * 0.1) * (potDif > 5 && potDif < 11 ? 2 : potDif > 10 && potDif < 21 ? 3 : potDif > 20 ? 5 : 1) * (position.Equals("Striker") ? 1.1 : position.Equals("Goalkeeper") ? 0.8 : 0.9));
            val = val < 1_000_000 ? Math.Round(val / 10_000) * 10_000 : val < 10_000_000 ? Math.Round(val / 100_000) * 100_000 : Math.Round(val / 1_000_000) * 1_000_000;
            return (int)val;
        }

        public static void UpdateVal()
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand read = new SQLiteCommand($"select * from players", connection);
                connection.Open();
                var reader = read.ExecuteReader();
                while (reader.Read())
                {
                    int value = Calculation.GetValue(Convert.ToDateTime(reader["dateofbirth"].ToString()), Convert.ToInt32(reader["overall"].ToString()), Convert.ToInt32(reader["potential"].ToString()), reader["position"].ToString());
                    SQLiteCommand update = new SQLiteCommand($"update players set value = {value} where id = {reader["id"].ToString()}", connection);
                    update.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public static void UpdateOve()
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand read = new SQLiteCommand($"select * from players where isRetiring = 1", connection);
                connection.Open();
                var reader = read.ExecuteReader();
                while (reader.Read())
                {
                    var stats = Calculation.GetStats(Convert.ToInt32(reader["overall"].ToString()), reader["position"].ToString());
                    SQLiteCommand update = new SQLiteCommand($"update players set offense = {stats["atk"]}, defence = {stats["def"]}, pass = {stats["pas"]}, gk ={stats["kep"]} where id = {Convert.ToInt32(reader["id"].ToString())}", connection);
                    update.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
