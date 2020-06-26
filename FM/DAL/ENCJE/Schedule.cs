using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.ENCJE
{
    class Schedule
    {
        public int Id { get; set; }
        public int Host { get; set; }
        public int Visitor { get; set; }
        public int? HostGoals { get; set; }
        public int? VisitorGoals { get; set; }
        public int Matchday { get; set; }
        public string League { get; set; }

        public Schedule(int id, int host, int visitor, int? hostGoals, int? visitorGoals, int matchday, string league)
        {
            Id = id;
            Host = host;
            Visitor = visitor;
            HostGoals = hostGoals;
            VisitorGoals = visitorGoals;
            Matchday = matchday;
            League = league;
        }
    }
}
