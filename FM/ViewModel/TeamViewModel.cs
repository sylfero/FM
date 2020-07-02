using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.ViewModel
{
    using FM.ViewModel.BaseClasses;
    using FM.DAL.Entity;
    using FM.DAL.Repositories;
    using FM.Model;
    using System.Windows.Input;
    using System.Windows;
    using System.Drawing;
    using System.Windows.Media;

    class TeamViewModel : ViewModelBase
    {
        public TeamViewModel()
        {
            Players = PlayerRepo.GetPlayersFromClub(ClubStatus.ClubName);
        }

        private Player selectedPlayer;
        private List<Player> players;
        private Visibility visibility = Visibility.Hidden;
        private System.Windows.Media.Brush bkgColor;
        private bool isOpen = false;
        private string contractLength;
        private int contractValue;

        public string ContractLength
        {
            get => contractLength;
            set
            {
                contractLength = value;
                OnPropertyChanged(nameof(ContractLength));
            }
        }

        public int ContractValue
        {
            get => contractValue;
            set
            {
                contractValue = value;
                OnPropertyChanged(nameof(ContractValue));
            }
        }

        public bool IsOpen
        {
            get => isOpen;
            set
            {
                isOpen = value;
                OnPropertyChanged(nameof(IsOpen));
            }
        }

        public List<Player> Players
        {
            get => players;
            set
            {
                players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public Player SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public System.Windows.Media.Brush BkgColor
        {
            get => bkgColor;
            set
            {
                bkgColor = value;
                OnPropertyChanged(nameof(BkgColor));
            }
        }

        private ICommand playerChanged = null;
        public ICommand PlayerChanged
        {
            get
            {
                if(playerChanged == null)
                {
                    playerChanged = new RelayCommand(
                        arg =>
                        {
                            if (Visibility == Visibility.Hidden)
                                Visibility = Visibility.Visible;
                            if(selectedPlayer != null)
                            {
                                if (selectedPlayer.Overall >= 75)
                                    BkgColor = new SolidColorBrush(Colors.Gold);
                                else if (selectedPlayer.Overall >= 65)
                                    BkgColor = new SolidColorBrush(Colors.Silver);
                                else
                                    BkgColor = new SolidColorBrush(Colors.SandyBrown);
                            }
                        },
                        arg => true
                        );
                }

                return playerChanged;
            }
        }

        private ICommand openPopUp = null;
        public ICommand OpenPopUp
        {
            get
            {
                if(openPopUp == null)
                {
                    openPopUp = new RelayCommand(
                        arg => { IsOpen = true; },
                        arg => true
                        );
                }

                return openPopUp;
            }
        }

        private ICommand newContract = null;
        public ICommand NewContract
        {
            get
            {
                if(newContract == null)
                {
                    newContract = new RelayCommand(
                        arg => {
                            PlayerRepo.PlayerNewContract(selectedPlayer.Id, ContractValue, ContractLength);
                            IsOpen = false;
                            Players = PlayerRepo.GetPlayersFromClub(ClubStatus.ClubName);
                            Visibility = Visibility.Hidden;
                        },
                        arh => ContractLength != null
                        );
                }

                return newContract;
            }
        }



    }
}
