using FM.DAL;
using FM.DAL.Entity;
using FM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FM.Model
{
    static class Simulation
    {
        public static void Simulate()
        {
            SimulateRound(ClubStatus.Round);
            ClubStatus.Round++;
            if (ClubStatus.Round < 39)
                ClubStatus.CurrentDate.AddDays(7);
            //ClubStatus.CurrentDate = Convert.ToDateTime(ScheduleRepo.GetDate(ClubStatus.Round));
            else
                ClubStatus.CurrentDate = ClubStatus.SeasonEnd;

            if (ClubStatus.RoundsToJunior == 0)
            {
                var country = CountryRepo.GetCountry(ClubStatus.JuniorCountry);
                Scout.Send(ClubStatus.Junior, country);
                ClubStatus.RoundsToJunior--;
            }
            else if (ClubStatus.RoundsToJunior > 0)
                ClubStatus.RoundsToJunior--;
            ClubStatus.SerializeSave();
        }

        public static void NextSeason()
        {
            Contract();
            ClubStatus.Round = ClubStatus.LeagueId == 2 ? 5 : 1;
            ClubStatus.SeasonEnd.AddYears(1);
            ClubStatus.SeasonStart.AddYears(1);
            ClubStatus.CurrentDate = ClubStatus.LeagueId == 2 ? new DateTime(ClubStatus.SeasonStart.Year, 8, 19) : new DateTime(ClubStatus.SeasonStart.Year, 7, 19);
            NewSchedule(1);
            NewSchedule(2);
            PlayerRepo.UpdateOve();
            Regen();
            Retire();
            ClubRepo.Reset();
            PlayerRepo.UpdateVal();
            ClubStatus.SerializeSave();
        }

        public static void SimulateRound(int round)
        {
            List<(int id, int host, int visitor)> games = ScheduleRepo.GetRound(round);
            Random rnd = new Random();

            foreach (var game in games)
            {
                double visitorLuck = rnd.Next(8, 15) / 10.0;
                double hostLuck = rnd.Next(8, 15) / 10.0;

                Dictionary<string, double> host = game.host == ClubStatus.ClubId ? Calculation.GetSquad() : Calculation.GetBotSquad(game.host);
                Dictionary<string, double> visitor = game.visitor == ClubStatus.ClubId ? Calculation.GetSquad() : Calculation.GetBotSquad(game.visitor);

                int hostChances = (int)((host["mid"] * 2 - visitor["def"]) * 1.1 * hostLuck / 11);
                int visitorChances = (int)((visitor["mid"] * 2 - host["def"]) * visitorLuck / 11);

                double hostGoalChance = (host["st"] * 2 - visitor["gk"]) * 1.1 * hostLuck / 300;
                double visitorGoalChance = (visitor["st"] * 2 - host["gk"]) * visitorLuck / 300;

                int hostGoals = (int)(Math.Round((hostChances < 0 ? 0 : hostChances) * (hostGoalChance < 0.20 ? 0 : hostGoalChance)));
                int visitorGoals = (int)(Math.Round((visitorChances < 0 ? 0 : visitorChances) * (visitorGoalChance < 0.20 ? 0 : visitorGoalChance)));

                ScheduleRepo.UpdateGame(game.id, hostGoals, visitorGoals);
                ClubRepo.UpdateTable(game.host, hostGoals, visitorGoals);
                ClubRepo.UpdateTable(game.visitor, visitorGoals, hostGoals);
            }
        }

        private static void NewSchedule(int leagueId)
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                connection.Open();
                /*int numberOfMatchdays = 0;
                SQLiteCommand command = new SQLiteCommand($"select count(distinct matchday) as number from schedule where league = {leagueId}", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                    numberOfMatchdays = Convert.ToInt32(reader["number"].ToString());
                reader.Close();

                List<int> wylosowane = new List<int>();
                for (int i = 1; i <= numberOfMatchdays; i++)
                    wylosowane.Add(i);
                var r = new Random();
                for (int i = 0; i < numberOfMatchdays / 2; i++)
                {
                    int wylosowana1 = r.Next(0, wylosowane.Count);
                    int wylosowana2 = r.Next(0, wylosowane.Count);
                    while (wylosowana1 == wylosowana2)
                    {
                        wylosowana1 = r.Next(0, wylosowane.Count);
                        wylosowana2 = r.Next(0, wylosowane.Count);
                    }


                    wylosowana1 = wylosowane[wylosowana1];
                    wylosowana2 = wylosowane[wylosowana2];

                    wylosowane.Remove(wylosowana1);
                    wylosowane.Remove(wylosowana2);

                    if (wylosowana1 > wylosowana2)
                        command = new SQLiteCommand($"select schedule.date as date from schedule where (matchday = {wylosowana1} or matchday = {wylosowana2}) and league = {leagueId} group by 1 order by matchday desc", connection);
                    else
                        command = new SQLiteCommand($"select schedule.date as date from schedule where (matchday = {wylosowana1} or matchday = {wylosowana2}) and league = {leagueId} group by 1 order by matchday", connection);

                    reader = command.ExecuteReader();
                    List<DateTime> dates = new List<DateTime>();
                    while (reader.Read())
                    {
                        dates.Add(Convert.ToDateTime(reader["date"].ToString()));
                    }
                    reader.Close();

                    int suma = wylosowana1 + wylosowana2;

                    command = new SQLiteCommand($"update schedule set matchday = {suma} - matchday where league = {leagueId} and (matchday = {wylosowana1} or matchday = {wylosowana2})", connection);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand($"update schedule set date = \"{dates[1].Year + 1}-{dates[1]:MM-dd}\" where matchday = {wylosowana2} and league = {leagueId}", connection);
                    command.ExecuteNonQuery();
                    command = new SQLiteCommand($"update schedule set date = \"{dates[0].Year + 1}-{dates[0]:MM-dd}\" where matchday = {wylosowana1} and league = {leagueId}", connection);
                    command.ExecuteNonQuery();
                }*/
                var command = new SQLiteCommand($"update schedule set host_goals = null, Visitor_goals = null", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Retire()
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update players set isRetiring = 1 where (strftime(\"{ClubStatus.SeasonStart:yyyy-MM-dd}\") - strftime(dateofbirth)) > 35", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Contract()
        {
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand($"update players set contract_terminates = \"{ClubStatus.SeasonEnd.Year + 1}-{ClubStatus.SeasonEnd:MM-dd}\" where contract_terminates = \"{ClubStatus.SeasonEnd:yyyy-MM-dd}\"", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Regen()
        {
            var players = PlayerRepo.GetRet();
            using (var connection = DBConnection.Instance.Connection)
            {
                foreach (int player in players)
                {
                    DateTime date = new DateTime(ClubStatus.SeasonEnd.Year - 16, 1, 1);
                    Random rnd = new Random();
                    date.AddDays(rnd.Next(365));
                    SQLiteCommand command = new SQLiteCommand($"update players set dateofbirth = \"{date:yyyy-MM-dd}\", contract_terminates = \"{ClubStatus.SeasonEnd.Year + 3}-{ClubStatus.SeasonEnd:MM-dd}\", overall = {rnd.Next(51, 65)}, isRetiring = 0 where id = {player}", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
