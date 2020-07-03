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
            Players = ClubStatus.ClubFirstSquad;
            CurrentClub = ClubRepo.GetYourClub(ClubStatus.ClubName);
        }

        private Player selectedPlayer;
        private ObservableCollection<Player> players;
        private Visibility visibility = Visibility.Hidden;
        private System.Windows.Media.Brush bkgColor;
        private bool isOpen = false;
        private string contractLength;
        private int contractValue;
        private int selectedTeam;
        private int selectedPlayerIndex;
        private Club currentClub;

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

        public ObservableCollection<Player> Players
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
                            CurrentClub = ClubRepo.GetYourClub(ClubStatus.ClubName);
                            IsOpen = false;
                            Players = ClubStatus.ClubFirstSquad;
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
                            Players = ClubStatus.ClubFirstSquad;
                            SelectedTeam = 0;
                        },
                        arg => SelectedPlayer != null
                        );
                }

                return clear;
            }
        }

        private ICommand teamChanged = null;
        public ICommand TeamChanged 
        {
            get
            {
                if(teamChanged == null)
                {
                    teamChanged = new RelayCommand(
                        arg => {
                            if (SelectedTeam == 0)
                                Players = ClubStatus.ClubFirstSquad;
                            else if (SelectedTeam == 1)
                                Players = PlayerRepo.GetPlayersFromClub(ClubStatus.ClubName);
                            Visibility = Visibility.Hidden;
                            SelectedPlayer = null;
                        },
                        arg => true
                        );
                }

                return teamChanged;
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
                        },
                        arg => SelectedPlayerIndex != -1 && SelectedTeam != 0 && ClubStatus.ClubFirstSquad.Count < 11 && ClubStatus.ClubFirstSquad.Any(item => item.Id == SelectedPlayer.Id) != true
                        );
                }

                return addToSquad;
            }
        }

        private ICommand removeFromSquad = null;
        public ICommand RemoveFromSquad
        {
            get
            {
                if(removeFromSquad == null)
                {
                    removeFromSquad = new RelayCommand(
                        arg =>
                        {
                            ClubStatus.ClubFirstSquad.RemoveAt(SelectedPlayerIndex);
                            Visibility = Visibility.Hidden;
                            SelectedPlayerIndex = -1;
                            SelectedPlayer = null;
                        },
                        arg => SelectedPlayerIndex != -1 && SelectedTeam != 1 && ClubStatus.ClubFirstSquad.Count > 0
                        );
                }

                return removeFromSquad;
            }
        }

        private ICommand saveSquad = null;
        public ICommand SaveSquad
        {
            get
            {
                if (saveSquad == null)
                {
                    saveSquad = new RelayCommand(
                        arg => {
                            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Player>));
                            TextWriter textWriter = new StreamWriter(ClubStatus.ClubPath);
                            serializer.Serialize(textWriter, ClubStatus.ClubFirstSquad);
                            textWriter.Close();
                        },
                        arg => ClubStatus.ClubFirstSquad.Count == 11
                        );
                }

                return saveSquad;
            }
        }
    }
}
