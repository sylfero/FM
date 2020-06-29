using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;
    class CountryRepo
    {
        public List<Country> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            using(var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select * from country", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.HasRows)
                {
                    countries.Add(new Country(reader));
                }
                connection.Close();
            }

            return countries;
        }
    }
}
