using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;

    class ClubRepo
    {
        public static List<Club> GetAllClubs()
        {
            List<Club> clubs = new List<Club>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select * from club", connection);
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
                SQLiteCommand command = new SQLiteCommand("select * from club where league = 2 ", connection);
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select * from club where league = 1 ", connection);
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

        public void TransferToClub(int clubId, int transferCost, int playerSalary)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select budget, salaryBudget from club where id = {clubId}", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                double clubBudget = Convert.ToDouble(reader["budget"].ToString());
                double clubSalaryBudget = Convert.ToDouble(reader["salaryBudget"].ToString());

                if(transferCost <= clubBudget && playerSalary <= clubSalaryBudget)
                {
                    SQLiteCommand command2 = new SQLiteCommand($"UPDATE club set budget = budget - {transferCost}, salaryBudget = salaryBudget - {playerSalary} where id = {clubId}", connection);
                    command2.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void TransferFromClub(int clubId, int transferCost, int playerSalary)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"UPDATE club set budget = budget + {transferCost}, salaryBudget = salaryBudget + {playerSalary} where id = {clubId}", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
