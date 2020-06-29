using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.Model
{
    static class ClubStatus
    {
        public static string Manager { get; set; }
        public static int LeagueId { get; set; }
        public static int ClubId { get; set; }
        
        public static void LoadSave(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Manager = lines[0];
            LeagueId = int.Parse(lines[1]);
            ClubId = int.Parse(lines[2]);
        }
    }
}
