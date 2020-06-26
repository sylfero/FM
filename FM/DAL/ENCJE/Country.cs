using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.ENCJE
{
    class Country
    {
        public int Id { get; set; }
        public string Land { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public int NumCode { get; set; }

        public Country(int id, string land, string iso, string iso3, int numCode)
        {
            Id = id;
            Land = land;
            Iso = iso;
            Iso3 = iso3;
            NumCode = numCode;
        }

        public Country(SQLiteDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Land = reader["name"].ToString();
            Iso = reader["iso"].ToString();
            Iso3 = reader["iso3"].ToString();
            NumCode = Convert.ToInt32(reader["numcode"].ToString());
        }
    }
}
