using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.ENCJE
{
    class Schedule
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string Visitor { get; set; }
        public int? HostGoals { get; set; }
        public int? VisitorGoals { get; set; }
        public int Matchday { get; set; }
        public string League { get; set; }

        public Schedule(int id, string host, string visitor, int? hostGoals, int? visitorGoals, int matchday, string league)
        {
            Id = id;
            Host = host;
            Visitor = visitor;
            HostGoals = hostGoals;
            VisitorGoals = visitorGoals;
            Matchday = matchday;
            League = league;
        }

        public Schedule(SQLiteDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Host = reader["host"].ToString();
            Visitor = reader["visitor"].ToString();
            HostGoals = Convert.ToInt32(reader["host_goals"].ToString());
            VisitorGoals = Convert.ToInt32(reader["visitor_goals"].ToString());
            Matchday = Convert.ToInt32(reader["matchday"].ToString());
            League = reader["league"].ToString();
        }
    }
}
