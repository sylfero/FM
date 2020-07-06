using FM.DAL;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Model
{
    static class Calculation
    {
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
            int age = ClubStatus.CurrentDate.Year - birth.Year;

            if ((birth.Month > ClubStatus.CurrentDate.Month) || (birth.Month == ClubStatus.CurrentDate.Month && birth.Day > DateTime.Now.Day))
                age--;
            int potDif = potential - overall + 1;
            double val = ((overall - 50) * 100_000 * (overall - 50) / (age * 0.1) * (potDif > 5 && potDif < 11 ? 2 : potDif > 10 && potDif < 21 ? 3 : potDif > 20 ? 5 : 1) * (position.Equals("Striker") ? 1.1 : position.Equals("Goalkeeper") ? 0.8 : 0.9));
            val = val < 1_000_000 ? Math.Round(val / 10_000) * 10_000 : val < 10_000_000 ? Math.Round(val / 100_000) * 100_000 : Math.Round(val / 1_000_000) * 1_000_000;
            return (int)val;
        }

        public static void SetSquad(int id)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"update players set currPosition = \"Defender\" where id in (select id from players where position = \"Defender\" and club = {id} order by overall desc limit 4)", connection);
                command.ExecuteNonQuery();
                command = new SQLiteCommand($"update players set currPosition = \"Midfielder\" where id in (select id from players where position = \"Midfielder\" and club = {id} order by overall desc limit 3)", connection);
                command.ExecuteNonQuery();
                command = new SQLiteCommand($"update players set currPosition = \"Striker\" where id in (select id from players where position = \"Striker\" and club = {id} order by overall desc limit 3)", connection);
                command.ExecuteNonQuery();
                command = new SQLiteCommand($"update players set currPosition = \"Goalkeeper\" where id in (select id from players where position = \"Goalkeeper\" and club = {id} order by overall desc limit 1)", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
<<<<<<< HEAD
=======

        public static Dictionary<string, double> GetBotSquad(int id)
        {
            Dictionary<string, double> squad = new Dictionary<string, double>();
            using (var connection = DBConnection.Instance.Connection)
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"select avg(defence) as defence from (select defence from players where club = {id} and position = \"Defender\" order by defence desc limit 4)", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("def", double.Parse(reader["defence"].ToString()));
                }

                command = new SQLiteCommand($"select avg(offense) as offense from (select offense from players where club = {id} and position = \"Striker\" order by offense desc limit 3)", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("st", double.Parse(reader["offense"].ToString()));
                }

                command = new SQLiteCommand($"select avg(pass) as pass from (select pass from players where club = {id} and position = \"Midfielder\" order by pass desc limit 3)", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("mid", double.Parse(reader["pass"].ToString()));
                }

                command = new SQLiteCommand($"select gk from players where club = {id} and position = \"Goalkeeper\" order by gk desc limit 1", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("gk", double.Parse(reader["gk"].ToString()));
                }
                connection.Close();
            }
            return squad;
        }

        public static Dictionary<string, double> GetSquad()
        {
            Dictionary<string, double> squad = new Dictionary<string, double>();
            using (var connection = DBConnection.Instance.Connection)
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"select avg(defence) as defence from (select defence from players where club = {ClubStatus.ClubId} and currPosition = \"Defender\")", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("def", double.Parse(reader["defence"].ToString()));
                }

                command = new SQLiteCommand($"select avg(offense) as offense from (select offense from players where club = {ClubStatus.ClubId} and currPosition = \"Striker\")", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("st", double.Parse(reader["offense"].ToString()));
                }

                command = new SQLiteCommand($"select avg(pass) as pass from (select pass from players where club = {ClubStatus.ClubId} and currPosition = \"Midfielder\")", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("mid", double.Parse(reader["pass"].ToString()));
                }

                command = new SQLiteCommand($"select gk from players where club = {ClubStatus.ClubId} and currPosition = \"Goalkeeper\"", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    squad.Add("gk", double.Parse(reader["gk"].ToString()));
                }
                connection.Close();
            }
            return squad;
        }
>>>>>>> 1d530f0c28602f836d646a7ce44059498b1f1062
    }
}
