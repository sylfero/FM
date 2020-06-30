using FM.DAL.Entity;
using FM.DAL.Repositories;
using FM.Model;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.ViewModel
{
    class TableViewModel : ViewModelBase
    {
        public TableViewModel()
        {
            CurrentLeague = new League(ClubStatus.LeagueId, ClubStatus.LeagueName);
        }

        public List<League> Leagues => LeagueRepo.GetAllLeagues();

        private League currentLeague;
        public League CurrentLeague
        {
            get => currentLeague;
            set
            {
                SetProperty(ref currentLeague, value);
                Clubs = LeagueTableRepo.GetTableFor(currentLeague.Id);
            }
        }

        private List<LeagueTable> clubs;
        public List<LeagueTable> Clubs
        {
            get => clubs;
            set => SetProperty(ref clubs, value);
        }
    }
}
