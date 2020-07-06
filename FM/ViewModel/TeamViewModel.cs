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
    using FM.Pages;
    using System.Windows.Controls;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using System.IO;

    class TeamViewModel : ViewModelBase
    {
        public TeamViewModel()
        {
            CurrentClub = ClubRepo.GetYourClub(ClubStatus.ClubName);
        }

        private Player selectedPlayer;
        private Visibility visibility = Visibility.Hidden;
        private System.Windows.Media.Brush bkgColor;
        private bool isOpen = false;
        private string contractLength;
        private int contractValue;
        private int selectedTeam;
        private int selectedPlayerIndex;
        private Club currentClub;
        private bool swaping = false;

        public Club CurrentClub
        {
            get => currentClub;
            set
            {
                currentClub = value;
                currentClub.Coach = ClubStatus.Manager;
                OnPropertyChanged(nameof(CurrentClub));
            }
        }
        
        public int SelectedPlayerIndex
        {
            get => selectedPlayerIndex;
            set
            {
                selectedPlayerIndex = value;
                OnPropertyChanged(nameof(SelectedPlayerIndex));
            }
        }

        
        public int SelectedTeam
        {
            get => selectedTeam;
            set
            {
                selectedTeam = value;
                OnPropertyChanged(nameof(SelectedTeam));
            }
        }

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

        public ObservableCollection<Player> players = PlayerRepo.GetPlayersFromClub(ClubStatus.ClubId);
        public ObservableCollection<Player> Players
        {
            get => players;
            set => SetProperty(ref players, value);
        }

        public Player SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                if (swaping)
                {
                    swaping = false;
                    PlayerRepo.SwapPosition(selectedPlayer.Id, selectedPlayer.CurrPosition, value.Id, value.CurrPosition);
                    Players = PlayerRepo.GetPlayersFromClub(ClubStatus.ClubId);
                    Visibility = Visibility.Hidden;
                    SetProperty(ref selectedPlayer, null);
                }
                else
                {
                    SetProperty(ref selectedPlayer, value);
                    if (Visibility == Visibility.Hidden)
                        Visibility = Visibility.Visible;
                    if (selectedPlayer != null)
                    {
                        if (selectedPlayer.Overall >= 75)
                            BkgColor = new SolidColorBrush(Colors.Gold);
                        else if (selectedPlayer.Overall >= 65)
                            BkgColor = new SolidColorBrush(Colors.Silver);
                        else
                            BkgColor = new SolidColorBrush(Colors.SandyBrown);
                    }
                }
                IsOpen = false;
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
                            CurrentClub = ClubRepo.GetYourClub(ClubStatus.ClubName);
                            IsOpen = false;
                            Visibility = Visibility.Hidden;
                            SelectedPlayer = null;
                            SelectedTeam = 0;
                        },
                        arh => ContractLength != null
                        );
                }

                return newContract;
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
                            SelectedPlayer = null;
                            Visibility = Visibility.Hidden;
                            SelectedTeam = 0;
                        },
                        arg => SelectedPlayer != null
                        );
                }

                return clear;
            }
        }

        private ICommand addToSquad = null;
        public ICommand AddToSquad
        {
            get
            {
                if(addToSquad == null)
                {
                    addToSquad = new RelayCommand(
                        arg => {
                            ClubStatus.ClubFirstSquad.Add(Players[SelectedPlayerIndex]);
                            Visibility = Visibility.Hidden;
                            SelectedPlayerIndex = -1;
                            SelectedPlayer = null;
                        }
                        );
                }

                return addToSquad;
            }
        }

        private ICommand swap;
        public ICommand Swap
        {
            get
            {
                if (swap == null)
                {
                    swap = new RelayCommand(x => swaping = true, x => SelectedPlayer != null);
                }

                return swap;
            }
        }
    }
}
