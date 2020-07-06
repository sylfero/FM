using FM.DAL.Entity;
using FM.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Model
{
    static class Scout
    {
        public static void Send(int players, Country country)
        {
            Random rnd = new Random();
            string[] tmp = null;
            string[] tmp2 = null;

            if (country.Id == 226)
            {
                tmp = File.ReadAllLines("uk_first.csv");
                tmp2 = File.ReadAllLines("uk_last.csv");
            }
            else if (country.Id == 50)
            {
                tmp = File.ReadAllLines("de_first.csv");
                tmp2 = File.ReadAllLines("de_last.csv");
            }

            for (int i = 0; i < players; i++)
            {
                int pos = rnd.Next(4);
                string position = pos == 0 ? "Striker" : pos == 1 ? "Midfielder" : pos == 2 ? "Defender" : "Goalkeeper";
                string firstName = tmp[rnd.Next(tmp.Length)];
                string lastName = tmp2[rnd.Next(tmp2.Length)];
                int prob = rnd.Next(100);
                int potential = prob < 60 ? rnd.Next(60, 71) : prob >= 60 && prob < 85 ? rnd.Next(71, 85) : prob >= 85 && prob < 95 ? rnd.Next(85, 92) : rnd.Next(92, 99);
                PlayerRepo.GeneratePlayer(ClubStatus.ClubId, true, country.Id, firstName, lastName, position, rnd.Next(50, 61), potential);
            }
        }
    }
}
