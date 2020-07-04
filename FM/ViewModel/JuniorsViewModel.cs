using FM.DAL.Entity;
using FM.DAL.Repositories;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.ViewModel
{
    class JuniorsViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        public IEnumerable<int> Number
        {
            get
            {
                for (int i = 0; i < 5; i++)
                {
                    yield return i;
                }
            }
        }

        private int? selecteNumber = null;
        public int? SelectedNumber
        {
            get => selecteNumber;
            set => SetProperty(ref selecteNumber, value);
        }

        public List<Player> Players => PlayerRepo.GetJuniors();

        private Player currentPlayer;
        public Player CurrentPlayer
        {
            get => currentPlayer;
            set => SetProperty(ref currentPlayer, value);
        }

        public List<Country> Countries => CountryRepo.GetLeagueCountries();

        private Country currentCountry;
        public Country CurrentCountry
        {
            get => currentCountry;
            set => SetProperty(ref currentCountry, value);
        }
    }
}
