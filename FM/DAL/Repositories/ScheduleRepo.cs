using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Repositories
{
    using Entity;
    using FM.Model;
    using System.Data.SQLite;
    static class ScheduleRepo
    {
        public static List<Schedule> GetBundesligaSchedule()
        {
            List<Schedule> schedule = new List<Schedule>();
            using(var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"Bundesliga\" order by matchday", connection);
                connection.Open();
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand("select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"Premier League\" order by matchday", connection);
                connection.Open();
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"Bundesliga\" and matchday = {matchday}", connection);
                connection.Open();
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"Premier League\" and matchday = {matchday}", connection);
                connection.Open();
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
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update schedule set host_goals = {hostGoals}, visitor_goals = {visitorGoals} where host = (select c.id from club c where c.name = {hostName}) and visitor = (select c.id from club c where c.name = {visitorName})", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<Schedule> GetLeagueSchedule(string leagueName)
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"{leagueName}\" order by matchday", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<Schedule> GetLeagueMatchday(string leagueName, int matchday)
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and l.name = \"{leagueName}\" and matchday = {matchday}", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<Schedule> GetYourClubSchedule()
        {
            List<Schedule> schedule = new List<Schedule>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select s.id, c.name as host, c1.name as visitor, host_goals, visitor_goals, matchday, l.name as league, date from schedule s, club c, club c1, league l where s.host = c.id and s.visitor = c1.id and s.league = l.id and (host = {ClubStatus.ClubId} or visitor = {ClubStatus.ClubId}) order by matchday", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    schedule.Add(new Schedule(reader));
                }
                connection.Close();
            }

            return schedule;
        }

        public static List<int> GetNumberOfMatchdays(string leagueName)
        {
            List<int> number = new List<int>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"select count(distinct matchday) as number from schedule s, league l where s.league = l.id and l.name = \"{leagueName}\"", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                int readerValue = 0;
                while (reader.Read())
                {
                    readerValue = Convert.ToInt32(reader["number"].ToString());
                }
                connection.Close();

                for (int i = 1; i <= readerValue; i++)
                    number.Add(i);

                return number;
            }
        }
    }
}
