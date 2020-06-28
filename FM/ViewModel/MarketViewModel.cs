using FM.DAL.ENCJE;
using FM.DAL.Repozytoria;
using FM.ViewModel.BaseClasses;
using Org.BouncyCastle.X509;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private int age;
        private int contract;
        private List<Country> countries = new List<Country>();
        private List<Club> clubs = new List<Club>();

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

        public int Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public int Contract
        {
            get => contract;
            set
            {
                contract = value;
                OnPropertyChanged(nameof(Contract));
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
            get
            {
                string[] leagues = new string[2] { "Bundesliga", "Premier League" };
                return leagues;
            }
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
            get
            {
                string[] positions = new string[4] { "Bramkarz", "Obrońca", "Pomocnik", "Napastnik" };
                return positions;
            }
        }

        public int[] AgeList
        {
            get
            {
                int[] ageList = new int[27];
                for(int i=16; i<=42; i++)
                {
                    ageList[i - 16] = i;
                }

                return ageList;
            }
        }


        #endregion

        private ICommand leagueChanged = null;

        public ICommand LeagueChanged
        {
            get
            {
                if(leagueChanged == null)
                {
                    leagueChanged = new RelayCommand(
                        arg =>
                        {
                            if (league == "Bundesliga")
                            {
                                Clubs = new List<Club>();
                                Clubs = RepozytoriumClub.GetBundesligaClubs();
                            }
                            else if (league == "Premier League")
                            {
                                Clubs = new List<Club>();
                                Clubs = RepozytoriumClub.GetPremierLeagueClubs();
                            }
                            else
                            {
                                Clubs = new List<Club>();
                                Clubs = RepozytoriumClub.GetAllClubs();
                            }
                        },
                        arg => true
                        );
                }

                return leagueChanged;
            }
        }

        private ICommand search = null;
        public ICommand Search
        {
            get
            {
                if(search == null)
                {
                    search = new RelayCommand(
                        arg => {
                            MessageBox.Show(Test());
                            Name = null; Surname = null; Position = null; League = null; Club = null; Age = 0; Nationality = null;
                            Contract = 0;
                        },
                        arg => true
                        );
                }

                return search;
            }
        }

        private string Test()
        {
            string command = "select p.name as name, surname, dateofbirth, n.nationality as nationality, position, c.club as club, value, salary, contract_terminates, overall, offense, defence from players p, country n, city c where p.club = c.id and p.nationality = n.iso3";
            if (name != null)
                command += $" and name like \"%{name}%\"";
            if (surname != null)
                command += $" and surname like \"%{surname}%\"";
            if (league != null)
                command += $" and league like \"%{league}%\"";
            if (club != null)
                command += $" and c.name like \"%{club.Name}%\"";
            if (nationality != null)
                command += $" and n.name like \"%{nationality.Land}%\"";
            if (position != null)
                command += $" and position like \"%{position}%\"";
            if (age != 0)
                command += $" and Year(dateofbirth) - Year(Now()) <= {age}";

            command += " order by overall";

            return command;
        }

    }
}
