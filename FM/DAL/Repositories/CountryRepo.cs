using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;
    static class CountryRepo
    {
        public static List<Country> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            using(var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select * from country order by name", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    countries.Add(new Country(reader));
                }
                connection.Close();
            }

            return countries;
        }

        public static List<Country> GetLeagueCountries()
        {
            List<Country> countries = new List<Country>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM country as c inner join league as l on c.id = l.country", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    countries.Add(new Country(reader));
                }
                connection.Close();
            }

            return countries;
        }
    }
}
