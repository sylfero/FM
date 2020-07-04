using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Model
{
    static class Calculation
    {
        public static Dictionary<string, int> GetStats(int overall, string position)
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            Random rnd = new Random();

            if (position == "Defender")
            {
                stats.Add("atk", (int)(0.3 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.5 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position == "Midfielder")
            {
                stats.Add("atk", (int)(0.7 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.7 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position == "Striker")
            {
                stats.Add("atk", (int)(0.9 * overall + rnd.Next(0, 10)));
                stats.Add("def", (int)(0.3 * overall + rnd.Next(0, 10)));
                stats.Add("pas", (int)(0.5 * overall + rnd.Next(0, 10)));
                stats.Add("kep", rnd.Next(3, 10));
            }
            else if (position == "Goalkeeper")
            {
                stats.Add("atk", rnd.Next(3, 10));
                stats.Add("def", rnd.Next(3, 10));
                stats.Add("pas", rnd.Next(3, 10));
                stats.Add("kep", overall);
            }

            return stats;
        }

        public static int GetValue(DateTime birth, int overall, int potential, string position)
        {
            int age = ClubStatus.CurrentDate.Year - birth.Year;

            if ((birth.Month > ClubStatus.CurrentDate.Month) || (birth.Month == ClubStatus.CurrentDate.Month && birth.Day > DateTime.Now.Day))
                age--;
            return (int)((overall - 50) * 100_000 * (overall - 50) / (age * 0.1) * (potential - overall) * (position == "Striker" ? 1.1 : position == "Goalkeeper" ? 0.8 : 0.9));
        }
    }
}
