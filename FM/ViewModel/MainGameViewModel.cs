using FM.DAL.Entity;
using FM.DAL.Repositories;
using FM.Model;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MainGameViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        public MainGameViewModel()
        {

            if (ClubStatus.Round == 39)
                SeasonEnd = true;
        }

        private bool scorePop;
        public bool ScorePop
        {
            get => scorePop;
            set => SetProperty(ref scorePop, value);
        }

        private bool seasonEnd;
        public bool SeasonEnd
        {
            get => seasonEnd;
            set => SetProperty(ref seasonEnd, value);
        }

        private string host;
        public string Host
        {
            get => host;
            set => SetProperty(ref host, value);
        }

        private string visitor;
        public string Visitor
        {
            get => visitor;
            set => SetProperty(ref visitor, value);
        }

        private int hostGoals;
        public int HostGoals
        {
            get => hostGoals;
            set => SetProperty(ref hostGoals, value);
        }

        private int visitorGoals;
        public int VisitorGoals
        {
            get => visitorGoals;
            set => SetProperty(ref visitorGoals, value);
        }

        private ICommand team;
        public ICommand Team
        {
            get
            {
                if (team == null)
                {
                    team = new RelayCommand(x => mainViewModel.SwapPage("team"));
                }
                return team;
            }
        }

        private ICommand exit;
        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(x => Application.Current.MainWindow.Close());
                }
                return exit;
            }
        }

        private ICommand transfers;
        public ICommand Transfers
        {
            get
            {
                if (transfers == null)
                {
                    transfers = new RelayCommand(x => mainViewModel.SwapPage("transfers"));
                }
                return transfers;
            }
        }

        private ICommand juniors;
        public ICommand Juniors
        {
            get
            {
                if (juniors == null)
                {
                    juniors = new RelayCommand(x => mainViewModel.SwapPage("juniors"));
                }
                return juniors;
            }
        }

        private ICommand tables;
        public ICommand Tables
        {
            get
            {
                if (tables == null)
                {
                    tables = new RelayCommand(x => mainViewModel.SwapPage("tables"));
                }
                return tables;
            }
        }

        private ICommand schedule;
        public ICommand Schedule
        {
            get
            {
                if (schedule == null)
                {
                    schedule = new RelayCommand(x => mainViewModel.SwapPage("schedule"));
                }
                return schedule;
            }
        }

        private ICommand play;
        public ICommand Play
        {
            get
            {
                if (play == null)
                {
                    play = new RelayCommand(x =>
                    {
                        Simulation.Simulate();
                        /*(string, string, int, int) score = ScheduleRepo.GetScore(ClubStatus.ClubId, ClubStatus.Round - 1);
                        Host = score.Item1;
                        Visitor = score.Item2;
                        HostGoals = score.Item3;
                        VisitorGoals = score.Item4;
                        ScorePop = true;*/
                        if (ClubStatus.Round == 39)
                            SeasonEnd = true;
                    }, x => ClubStatus.Round < 39);
                }
                return play;
            }
        }

        private ICommand closePop;
        public ICommand ClosePop
        {
            get
            {
                if (closePop == null)
                {
                    closePop = new RelayCommand(x => ScorePop = false);
                }
                return closePop;
            }
        }

        private ICommand nextSeason;
        public ICommand NextSeason
        {
            get
            {
                if (nextSeason == null)
                {
                    nextSeason = new RelayCommand(x => {
                        Simulation.NextSeason();
                        SeasonEnd = false;
                    });
                }
                return nextSeason;
            }
        }
    }
}
