using FM.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FM.Model
{
    static class ClubStatus
    {
        public static string Manager { get; set; }
        public static int LeagueId { get; set; }
        public static int ClubId { get; set; }
        public static string LeagueName { get; set; }
        public static string ClubName { get; set; }
        public static string ClubPath { get; set; }

        public static ObservableCollection<Player> ClubFirstSquad {get; set;}
        
        public static void LoadSave(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Manager = lines[0];
            LeagueId = int.Parse(lines[1]);
            ClubId = int.Parse(lines[2]);
            LeagueName = lines[3];
            ClubName = lines[4];
            ClubFirstSquad = new ObservableCollection<Player>();
            ClubPath = Path.GetDirectoryName(path) + $@"\firstsquad.xml";
            if (File.Exists(Path.GetDirectoryName(path)+$@"\firstsquad.xml") == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Player>));

                using (Stream reader = new FileStream(Path.GetDirectoryName(path) + $@"\firstsquad.xml", FileMode.Open))
                {
                    ClubFirstSquad = (ObservableCollection<Player>)serializer.Deserialize(reader);
                }
            } 
        }
    }
}
