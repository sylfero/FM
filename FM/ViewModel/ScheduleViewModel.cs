using FM.ViewModel.BaseClasses;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.ViewModel
{
    using FM.DAL.Entity;
    using FM.DAL.Repositories;
    using Org.BouncyCastle.Asn1.Mozilla;
    using Org.BouncyCastle.Bcpg.OpenPgp;
    using System.Security.RightsManagement;
    using System.Windows;
    using System.Windows.Input;

    class ScheduleViewModel : ViewModelBase
    {
        private List<Schedule> schedule = ScheduleRepo.GetYourClubSchedule();
        private List<League> leagues = LeagueRepo.GetAllLeagues();
        private List<int> matchdayNumber = new List<int>();
        private League selectedLeague;
        private int? selectedMatchday;
        private Visibility visibility = Visibility.Hidden;

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public League SelectedLeague
        {
            get => selectedLeague;
            set
            {
                selectedLeague = value;
                OnPropertyChanged(nameof(SelectedLeague));
            }
        }

        public int? SelectedMatchday
        {
            get => selectedMatchday;
            set
            {
                selectedMatchday = value;
                OnPropertyChanged(nameof(SelectedMatchday));
            }
        }

        public List<Schedule> Schedule
        {
            get => schedule;
            set
            {
                schedule = value;
                OnPropertyChanged(nameof(Schedule));
            }
        }

        public List<League> Leagues
        {
            get => leagues;
        }

        public List<int> MatchdayNumber
        {
            get => matchdayNumber;
            set
            {
                matchdayNumber = value;
                OnPropertyChanged(nameof(MatchdayNumber));
            }
        }

        private ICommand leagueChanged = null;
        public ICommand LeagueChanged
        {
            get
            {
                if(leagueChanged == null)
                {
                    leagueChanged = new RelayCommand(
                        arg => {
                            Visibility = Visibility.Visible;
                            Schedule = ScheduleRepo.GetLeagueSchedule(selectedLeague.Name);
                            MatchdayNumber = ScheduleRepo.GetNumberOfMatchdays(selectedLeague.Name);
                            SelectedMatchday = null;
                        },
                        arg => SelectedLeague != null
                        );
                }

                return leagueChanged;
            }
        }

        private ICommand matchdayChanged = null;
        public ICommand MatchdayChanged
        {
            get
            {
                if(matchdayChanged == null)
                {
                    matchdayChanged = new RelayCommand(
                        arg => {
                            Schedule = ScheduleRepo.GetLeagueMatchday(selectedLeague.Name, (int)selectedMatchday);
                        },
                        arg => SelectedLeague != null && selectedMatchday != null
                        );
                }

                return matchdayChanged;
            }
        }

        private ICommand clear = null;
        public ICommand Clear
        {
            get
            {
                if(clear == null)
                {
                    clear = new RelayCommand(
                        arg => {
                            SelectedLeague = null;
                            SelectedMatchday = null;
                            Schedule = ScheduleRepo.GetYourClubSchedule();
                            MatchdayNumber = new List<int>();
                            Visibility = Visibility.Hidden;
                        },
                        arg => true
                        );
                }

                return clear;
            }
        }

    }
}
