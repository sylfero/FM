using FM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FM.Model
{
    class Simulation
    {
        public static void Simulate()
        {
            SimulateRound(ClubStatus.Round);
            ClubStatus.Round++;
            if (ClubStatus.Round < 39)
                ClubStatus.CurrentDate = Convert.ToDateTime(ScheduleRepo.GetDate(ClubStatus.Round));
            else
                ClubStatus.CurrentDate = ClubStatus.SeasonEnd;

            if (ClubStatus.RoundsToJunior == 0)
            {
                Scout.Send(ClubStatus.Junior, CountryRepo.GetCountry(ClubStatus.JuniorCountry));
                ClubStatus.RoundsToJunior--;
            }
            else if (ClubStatus.RoundsToJunior > 0)
                ClubStatus.RoundsToJunior--;
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
    }
}
