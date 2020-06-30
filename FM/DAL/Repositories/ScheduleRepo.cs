using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using System.Data.SQLite;
    static class ScheduleRepo
    {
        public static List<Schedule> GetBundesligaSchedule()
        {
            List<Schedule> schedule = new List<Schedule>();
            using(var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand("select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, s.league from schedule s, club c, club c1 where s.host = c.id and s.visitor = c1.id and s.league = \"Bundesliga\" order by matchday", connection);
                connection.Close();
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<Schedule> GetPremierLeagueSchedule()
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand("select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, s.league from schedule s, club c, club c1 where s.host = c.id and s.visitor = c1.id and s.league = \"Premier League\" order by matchday", connection);
                connection.Close();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<Schedule> GetBundesligaMatchday(int matchday)
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, s.league from schedule s, club c, club c1 where s.host = c.id and s.visitor = c1.id and s.league = \"Bundesliga\" and matchday = {matchday}", connection);
                connection.Close();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<Schedule> GetPremierLeagueMatchday(int matchday)
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, s.league from schedule s, club c, club c1 where s.host = c.id and s.visitor = c1.id and s.league = \"Premier League\" and matchday = {matchday}", connection);
                connection.Close();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static void AddGameScore(string hostName, string visitorName, int hostGoals, int visitorGoals)
        {
            using (var connection = DBConnection.Instance.connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update schedule set host_goals = {hostGoals}, visitor_goals = {visitorGoals} where host = (select c.id from club c where c.name = {hostName}) and visitor = (select c.id from club c where c.name = {visitorName})", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
