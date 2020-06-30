using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Entity
{
    class League
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public League(SQLiteDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Name = reader["name"].ToString();
        }

        public League(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
