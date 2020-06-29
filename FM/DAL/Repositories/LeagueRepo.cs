using FM.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    static class LeagueRepo
    {
        public static List<League> GetAllLeagues()
        {
            List<League> leagues = new List<League>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select * from league", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    leagues.Add(new League(reader));
                }
                connection.Close();
            }

            return leagues;
        }
    }
}
