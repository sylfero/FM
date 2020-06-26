using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repozytoria
{
    using ENCJE;
    using System.Data.SQLite;
    class RepozytoriumPlayer
    {
        public static List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
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
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.league = \"Bundesliga\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
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
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand("select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.league = \"Premier League\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public List<Player> GetPlayersFromClub(string clubName)
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and c.name = \"{clubName}\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public List<Player> GetPlayersFromNationality(string nationality)
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select p.id as id, p.name as name, surname, c.name as club, dateofbirth, n.name as nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, country n, club c where p.club = c.id and p.nationality = n.iso3 and n.name = \"{nationality}\" ", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.HasRows)
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        public void PlayerTransfer(int playerId, int newSalary, string newClub, DateTime contractLength)
        {
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE players set salary = {newSalary}, contract_terminates = {contractLength}, club = (select c.id from club c where c.name = \"{newClub}\") where p.id = {playerId}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void PlayerNewContract(int playerId, int newSalary, DateTime contractLength)
        {
            using (var connection = DBConnection.Instance.connection)
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
