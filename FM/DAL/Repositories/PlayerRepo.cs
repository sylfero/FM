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

    static class PlayerRepo
    {
        public static List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3", connection);
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
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.league = \"Bundesliga\" ", connection);
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
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.league = \"Premier League\" ", connection);
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

        public static List<Player> GetPlayersFromClub(string clubName)
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.name = \"{clubName}\" ", connection);
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
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and n.name = \"{nationality}\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())                {
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
                double teamSalaryBudget = Convert.ToDouble(reader["salaryBudget"].ToString());
                command = new SQLiteCommand($"select salary from players where id = {playerId}", connection);
                reader = command.ExecuteReader();
                double playerSalary = Convert.ToDouble(reader["salary"].ToString());
                if (teamSalaryBudget >= (newSalary - playerSalary))
                {
                    command = new SQLiteCommand($"update players set salary = {newSalary}, contract_terminates = {contractLength} where id = {playerId}", connection);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand($"update club set salaryBudget = salaryBudget - {newSalary - playerSalary} where id = (select p.club from players p where p.id = {playerId})", connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
