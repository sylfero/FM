using FM.DAL.ENCJE;
using FM.DAL.Repozytoria;
using FM.ViewModel.BaseClasses;
using Org.BouncyCastle.X509;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MarketViewModel : ViewModelBase
    {

        #region Składowe prywatne

        private string name;
        private string surname;
        private Country nationality;
        private string league;
        private Club club;
        private string position;
        private string[] leagues = new string[2] { "Bundesliga", "Premier League" };
        private List<Country> countries = new List<Country>();
        private List<Club> clubs = new List<Club>();
        private string[] positions = new string[4] { "Bramkarz", "Obrońca", "Pomocnik", "Napastnik" };

        #endregion

        #region Własności
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public Country Nationality
        {
            get => nationality;
            set
            {
                nationality = value;
                OnPropertyChanged(nameof(Nationality));
            }
        }

        public string League
        {
            get => league;
            set
            {
                league = value;
                OnPropertyChanged(nameof(League));
            }
        }

        public Club Club
        {
            get => club;
            set
            {
                club = value;
                OnPropertyChanged(nameof(Club));
            }
        }

        public string Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public string[] Leagues
        {
            get => leagues;
        }

        public List<Country> Countries
        {
            get => countries;
        }

        public List<Club> Clubs
        {
            get => clubs;
            set
            {
                clubs = value;
                OnPropertyChanged(nameof(Clubs));
            }
        }

        public string[] Positions
        {
            get => positions;
        }


        #endregion

    }
}
