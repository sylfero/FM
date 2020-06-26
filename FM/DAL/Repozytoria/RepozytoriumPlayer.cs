﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repozytoria
{
    using ENCJE;
    using MySql.Data.MySqlClient;
    class RepozytoriumPlayer
    {
        public static List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.connection)
            {
                MySqlCommand command = new MySqlCommand("select p.id as id, p.name as name, surname, c.club as club, dateofbirth, nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, club c where p.club = c.id", connection);
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
                MySqlCommand command = new MySqlCommand("select p.id as id, p.name as name, surname, c.club as club, dateofbirth, nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, club c where p.club = c.id and c.league = \"Bundesliga\" ", connection);
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
                MySqlCommand command = new MySqlCommand("select p.id as id, p.name as name, surname, c.club as club, dateofbirth, nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, club c where p.club = c.id and c.league = \"Premier League\" ", connection);
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
                MySqlCommand command = new MySqlCommand($"select p.id as id, p.name as name, surname, c.club as club, dateofbirth, nationality, position, contract_terminates, offense, defence, p.overall as overall, potential from players p, club c where p.club = c.id and c.name = \"{clubName}\" ", connection);
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
                MySqlCommand command = new MySqlCommand($"UPDATE players set salary = {newSalary}, contract_terminates = {contractLength}, club = (select c.id from club c where c.name = \"{newClub}\") where p.id = {playerId}", connection);
                connection.Open();
                var reader = command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}