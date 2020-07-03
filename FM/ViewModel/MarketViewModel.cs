using FM.DAL;
using FM.DAL.Entity;
using FM.DAL.Repositories;
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
using System.Runtime.InteropServices;
using Google.Protobuf.WellKnownTypes;
using System.Drawing;
using System.Windows.Media;
using System.Data.SQLite;
using FM.Model;

namespace FM.ViewModel
{
    class MarketViewModel : ViewModelBase
    {

        #region Filtrowanie piłkarzy

        #region Składowe prywatne

        private string name;
        private string surname;
        private Country nationality;
        private string league;
        private Club club;
        private string position;
        private int? age;
        private int? maxValue;
        private List<Country> countries = CountryRepo.GetAllCountries();
        private List<Club> clubs = ClubRepo.GetAllClubs();
        private List<Player> searchedPlayers = new List<Player>();

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

        public int? Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public int? MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                OnPropertyChanged(nameof(MaxValue));
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
                string[] positions = new string[4] { "Goalkeeper", "Defender", "Midfielder", "Striker" };
                return positions;
            }
        }

        public int[] AgeList
        {
            get
            {
                int[] ageList = new int[27];
                for (int i = 16; i <= 42; i++)
                {
                    ageList[i - 16] = i;
                }

                return ageList;
            }
        }

        public List<Player> SearchedPlayers
        {
            get => searchedPlayers;
            set
            {
                searchedPlayers = value;
                OnPropertyChanged(nameof(SearchedPlayers));
            }
        }

        #endregion

        private ICommand leagueChanged = null;

        public ICommand LeagueChanged
        {
            get
            {
                if (leagueChanged == null)
                {
                    leagueChanged = new RelayCommand(
                        arg =>
                        {
                            if (league == "Bundesliga")
                            {
                                Clubs = new List<Club>();
                                Clubs = ClubRepo.GetBundesligaClubs();
                            }
                            else if (league == "Premier League")
                            {
                                Clubs = new List<Club>();
                                Clubs = ClubRepo.GetPremierLeagueClubs();
                            }
                            else
                            {
                                Clubs = new List<Club>();
                                Clubs = ClubRepo.GetAllClubs();
                            }
                        },
                        arg => true
                        );
                }

                return leagueChanged;
            }
        }

        private ICommand clearFiltres = null;
        public ICommand ClearFiltres
        {
            get
            {
                if (clearFiltres == null)
                {
                    clearFiltres = new RelayCommand(
                        arg => {
                            Name = null; Surname = null; Position = null; League = null;
                            Club = null; Age = null; Nationality = null; MaxValue = null;
                            SearchedPlayers = new List<Player>();
                            Visibility = System.Windows.Visibility.Hidden;
                            SelectedPlayer = null;
                        },
                        arg => true
                        );
                }

                return clearFiltres;
            }
        }

        private ICommand search = null;
        public ICommand Search
        {
            get
            {
                if (search == null)
                {
                    search = new RelayCommand(
                        arg => {
                            SearchedPlayers = GetPlayersFiltres();
                        },
                        arg => true
                        );
                }

                return search;
            }
        }

        private List<Player> GetPlayersFiltres()
        {
            string command1 = $"select p.id as id, p.name as name, surname, dateofbirth, n.name as nationality, position, c.name as club, value, salary, contract_terminates, p.overall as overall, offense, defence, potential from players p, country n, club c, league l where p.club = c.id and p.nationality = n.iso3 and c.league = l.id and p.club != {ClubStatus.ClubId}";
            if (name != null)
                command1 += $" and p.name like \"%{name}%\"";
            if (surname != null)
                command1 += $" and surname like \"%{surname}%\"";
            if (league != null)
                command1 += $" and l.name like \"%{league}%\"";
            if (club != null)
                command1 += $" and c.name like \"%{club.Name}%\"";
            if (nationality != null)
                command1 += $" and n.name like \"%{nationality.Land}%\"";
            if (position != null)
                command1 += $" and position like \"%{position}%\"";
            if (age != null)
                command1 += $" and Year(Now()) - Year(dateofbirth) <= {age}";
            if (maxValue != null)
                command1 += $" and p.value <= {maxValue}";

            command1 += " order by p.overall desc";
            List<Player> players = new List<Player>();
            using (var connection = DBConnection.Instance.Connection)
            {
                SQLiteCommand command = new SQLiteCommand(command1, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(new Player(reader));
                }
                connection.Close();
            }

            return players;
        }

        #endregion

        #region Zmiana Wybranego Piłkarza z listy

        private System.Windows.Media.Brush bkgColor;
        private Visibility visibility = System.Windows.Visibility.Hidden;
        private Player selectedPlayer;
        private string playerName;
        private string playerSurname;
        private string playerClub;
        private string playerNationality;
        private int playerOverall;
        private int playerDefence;
        private int playerOffence;
        private int playerPotential;
        private string playerPosition;
        private DateTime playerAge;
        private DateTime playerContract;
        private int playerValue;
        private int playerSalary;

        public System.Windows.Media.Brush BkgColor
        {
            get => bkgColor;
            set
            {
                bkgColor = value;
                OnPropertyChanged(nameof(BkgColor));
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

        public Player SelectedPlayer
        {
            get => selectedPlayer;
            set
            {
                selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        public string PlayerName
        {
            get => playerName;
            set
            {
                playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        public string PlayerSurname
        {
            get => playerSurname;
            set
            {
                playerSurname = value;
                OnPropertyChanged(nameof(PlayerSurname));
            }
        }

        public string PlayerClub
        {
            get => playerClub;
            set
            {
                playerClub = value;
                OnPropertyChanged(nameof(PlayerClub));
            }
        }

        public string PlayerPosition
        {
            get => playerPosition;
            set
            {
                playerPosition = value;
                OnPropertyChanged(nameof(PlayerPosition));
            }
        }

        public string PlayerNationality
        {
            get => playerNationality;
            set
            {
                playerNationality = value;
                OnPropertyChanged(nameof(PlayerNationality));
            }
        }

        public int PlayerOverall
        {
            get => playerOverall;
            set{
                playerOverall = value;
                OnPropertyChanged(nameof(PlayerOverall));
            }
        }

        public int PlayerDefence
        {
            get => playerDefence;
            set{
                playerDefence = value;
                OnPropertyChanged(nameof(PlayerDefence));
            }
        }

        public int PlayerOffence
        {
            get => playerOffence;
            set
            {
                playerOffence = value;
                OnPropertyChanged(nameof(PlayerOffence));
            }
        }

        public int PlayerPotential
        {
            get => playerPotential;
            set
            {
                playerPotential = value;
                OnPropertyChanged(nameof(PlayerPotential));
            }
        }

        public int PlayerValue
        {
            get => playerValue;
            set
            {
                playerValue = value;
                OnPropertyChanged(nameof(PlayerValue));
            }
        }

        public int PlayerSalary
        {
            get => playerSalary;
            set
            {
                playerSalary = value;
                OnPropertyChanged(nameof(PlayerSalary));
            }
        }

        public DateTime PlayerAge
        {
            get => playerAge;
            set
            {
                playerAge = value;
                OnPropertyChanged(nameof(PlayerAge));
            }
        }

        public DateTime PlayerContract
        {
            get => playerContract;
            set
            {
                playerContract = value;
                OnPropertyChanged(nameof(PlayerContract));
            }
        }

        private ICommand playerSelectionChanged = null;
        public ICommand PlayerSelectionChanged
        {
            get
            {
                if(playerSelectionChanged == null)
                {
                    playerSelectionChanged = new RelayCommand(
                        arg =>
                        {
                            PlayerAge = selectedPlayer.DateOfBirth;
                            PlayerClub = SelectedPlayer.Club;
                            PlayerContract = selectedPlayer.ContractTerminates;
                            PlayerDefence = SelectedPlayer.Defence;
                            PlayerName = SelectedPlayer.Name;
                            PlayerNationality = SelectedPlayer.Nationality;
                            PlayerOffence = SelectedPlayer.Offense;
                            PlayerOverall = SelectedPlayer.Overall;
                            PlayerPosition = SelectedPlayer.Position;
                            PlayerPotential = SelectedPlayer.Potential;
                            PlayerSalary = SelectedPlayer.Salary;
                            PlayerSurname = SelectedPlayer.Surname;
                            PlayerValue = SelectedPlayer.Value;
                            Visibility = System.Windows.Visibility.Visible;
                            if (PlayerOverall >= 75)
                                BkgColor = new SolidColorBrush(Colors.Gold);
                            else if (PlayerOverall < 75 && PlayerOverall > 64)
                                BkgColor = new SolidColorBrush(Colors.Silver);
                            else
                                BkgColor = new SolidColorBrush(Colors.SandyBrown);
                        },
                        arg => selectedPlayer != null
                        );
                }

                return playerSelectionChanged;
            }
        }

        #endregion

        #region Transfery

        private string transferValue;
        private string transferSalary;
        private string transferContract;

        public string TransferValue
        {
            get => transferValue;
            set
            {
                transferValue = value;
                OnPropertyChanged(nameof(TransferValue));
            }
        }

        public string TransferSalary
        {
            get => transferSalary;
            set
            {
                transferSalary = value;
                OnPropertyChanged(nameof(TransferSalary));
            }
        }

        public string TransferContract
        {
            get => transferContract;
            set
            {
                transferContract = value;
                OnPropertyChanged(nameof(TransferContract));
            }
        }

        private ICommand playerTransfer = null;
        public ICommand PlayerTransfer
        {
            get
            {
                if(playerTransfer == null)
                {
                    playerTransfer = new RelayCommand(
                        arg => {
                            ClubRepo.TransferToClub(SelectedPlayer.Id, SelectedPlayer.Club, Convert.ToInt32(transferValue), Convert.ToInt32(transferSalary), transferContract, selectedPlayer.Value, selectedPlayer.Salary);
                            SearchedPlayers = GetPlayersFiltres();
                            Visibility = System.Windows.Visibility.Hidden;
                            SelectedPlayer = null;
                            TransferContract = null;
                            TransferSalary = null;
                            TransferValue = null;
                        },
                        arg => TransferContract != null && TransferSalary != null && TransferValue != null
                        );
                }

                return playerTransfer;
            }
        }

        #endregion

    }
}
