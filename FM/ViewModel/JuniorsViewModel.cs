using FM.DAL.Entity;
using FM.DAL.Repositories;
using FM.Model;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FM.ViewModel
{
    class JuniorsViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        public IEnumerable<int> Number
        {
            get
            {
                for (int i = 1; i <= 5; i++)
                {
                    yield return i;
                }
            }
        }

        private int? selecteNumber = null;
        public int? SelectedNumber
        {
            get => selecteNumber;
            set
            {
                SetProperty(ref selecteNumber, value);
                Price = selecteNumber * 100_000;
            }
        }

        public List<Player> players = PlayerRepo.GetJuniors();
        public List<Player> Players
        {
            get => players;
            set => SetProperty(ref players, value);
        }

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

        private int? price;
        public int? Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        private ICommand send;
        public ICommand Send
        {
            get
            {
                if (send == null)
                {
                    send = new RelayCommand(x => { ClubStatus.RoundsToJunior = 4; ClubStatus.Junior = (int)SelectedNumber; ClubStatus.JuniorCountry = CurrentCountry.Id; /*ClubRepo.UpdateBudget(ClubStatus.ClubId, (int)selecteNumber * 100_000);*/ } ,x => SelectedNumber != null && CurrentCountry != null && ClubStatus.RoundsToJunior == -1 && ClubRepo.GetBudget(ClubStatus.ClubId) >= selecteNumber * 100_000);
                }
                return send;
            }
        }

        private ICommand contract;
        public ICommand Contract
        {
            get
            {
                if (contract == null)
                {
                    contract = new RelayCommand(x => { PlayerRepo.SignJunior(CurrentPlayer.Id); Players = PlayerRepo.GetJuniors(); /*ClubRepo.UpdateSalary(ClubStatus.ClubId, 500);*/ }, x => CurrentPlayer != null && ClubRepo.GetSalary(ClubStatus.ClubId) >= 500);
                }
                return contract;
            }
        }
    }
}
