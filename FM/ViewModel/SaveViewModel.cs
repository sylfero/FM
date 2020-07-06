using FM.DAL;
using FM.DAL.Entity;
using FM.DAL.Repositories;
using FM.Model;
using FM.Pages;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace FM.ViewModel
{
    class SaveViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        public SaveViewModel()
        {
            saves = new ObservableCollection<string>(File.ReadAllLines("SavesConfig.txt"));
            if (saves.Count > 3)
                for (int i = saves.Count - 1; i > 2; i--)
                    saves.RemoveAt(i);
        }

        private string currentSave;
        public string CurrentSave
        {
            get => currentSave;
            set => SetProperty(ref currentSave, value);
        }

        private ObservableCollection<string> saves;
        public ObservableCollection<string> Saves
        {
            get => saves;
            set => SetProperty(ref saves, value);
        }

        private bool addPop = false;
        public bool AddPop
        {
            get => addPop;
            set => SetProperty(ref addPop, value);
        }

        private bool removePop = false;
        public bool RemovePop
        {
            get => removePop;
            set => SetProperty(ref removePop, value);
        }

        private string newSave;
        public string NewSave
        {
            get => newSave;
            set => SetProperty(ref newSave, value);
        }

        public List<League> Leagues => LeagueRepo.GetAllLeagues();

        private League currentLeague;
        public League CurrentLeague
        {
            get => currentLeague;
            set
            {
                SetProperty(ref currentLeague, value);
                Clubs = currentLeague != null ? ClubRepo.GetAllClubsIn(currentLeague.Id) : null;
                CurrentClub = null;
            }
        }

        private List<Club> clubs;
        public List<Club> Clubs
        {
            get => clubs;
            set => SetProperty(ref clubs, value);
        }

        private Club currentClub;
        public Club CurrentClub
        {
            get => currentClub;
            set => SetProperty(ref currentClub, value);
        }

        private string manager;
        public string Manager
        {
            get => manager;
            set => SetProperty(ref manager, value);
        }

        private ICommand load;
        public ICommand Load
        {
            get
            {
                if (load == null)
                {
                    load = new RelayCommand(x => {
                        DAL.DBConnection.Instance.SetDatabase($@"Saves\{currentSave}\FMDatabase.db");
                        ClubStatus.LoadSave($@"Saves\{currentSave}\Properties.txt");
                        mainViewModel.SwapPage("game");
                    }, x => !string.IsNullOrEmpty(CurrentSave));
                }
                return load;
            }
        }

        private ICommand add;
        public ICommand Add
        {
            get
            {
                if (add == null)
                {
                    add = new RelayCommand(x => AddPop = true, x => Saves.Count() < 3);
                }
                return add;
            }
        }

        private ICommand remove;
        public ICommand Remove
        {
            get
            {
                if (remove == null)
                {
                    remove = new RelayCommand(x => RemovePop = true, x => !string.IsNullOrEmpty(CurrentSave));
                }
                return remove;
            }
        }

        private ICommand confirmRemove;
        public ICommand ConfirmRemove
        {
            get
            {
                if (confirmRemove == null)
                {
                    confirmRemove = new RelayCommand(x => { 
                        RemovePop = false;
                        DirectoryInfo directory = new DirectoryInfo("Saves\\" + CurrentSave);
                        directory.Delete(true);
                        Saves.Remove(currentSave);
                        File.WriteAllLines("SavesConfig.txt", Saves);
                    }
                    );
                }
                return confirmRemove;
            }
        }

        private ICommand denyRemove;
        public ICommand DenyRemove
        {
            get
            {
                if (denyRemove == null)
                {
                    denyRemove = new RelayCommand(x => RemovePop = false);
                }
                return denyRemove;
            }
        }

        private ICommand confirmAdd;
        public ICommand ConfirmAdd
        {
            get
            {
                if (confirmAdd == null)
                {
                    confirmAdd = new RelayCommand(x => { 
                        AddPop = false; 
                        Directory.CreateDirectory("Saves\\" + NewSave);
                        File.Copy("BaseDB.db", $@"Saves\{NewSave}\FMDataBase.db");
                        Saves.Add(NewSave);
                        File.WriteAllLines("SavesConfig.txt", Saves);
                        using(StreamWriter writer = new StreamWriter($@"Saves\{NewSave}\Properties.txt"))
                        {
                            writer.WriteLine(Manager);
                            writer.WriteLine(CurrentLeague.Id);
                            writer.WriteLine(CurrentClub.Id);
                            writer.WriteLine(CurrentLeague.Name);
                            writer.WriteLine(CurrentClub.Name);
                            writer.WriteLine(CurrentLeague.Id == 1 ? "2019-07-19" : "2019-08-19");
                            writer.WriteLine("2019-07-01");
                            writer.WriteLine("2020-06-30");
                            writer.WriteLine(CurrentLeague.Id == 1 ? 1 : 5);
                            writer.WriteLine(-1);
                            writer.WriteLine(0);
                            writer.WriteLine(0);
                        }
                        DBConnection.Instance.SetDatabase($@"Saves\{NewSave}\FMDataBase.db");
                        ClubRepo.SetManager(CurrentClub.Id, Manager);
                        Calculation.SetSquad(CurrentClub.Id);
                        if (CurrentLeague.Id == 2)
                        {
                            for (int i = 1; i < 5; i++)
                            {
                                Simulation.SimulateRound(i);
                            }
                        }
                        DBConnection.Instance.SetDefault();
                        NewSave = null;
                        CurrentLeague = null;
                        Manager = null;
                    }, x => !string.IsNullOrEmpty(NewSave) && !Saves.Any(y => y == NewSave) && !string.IsNullOrEmpty(Manager) && CurrentClub != null);
                }
                return confirmAdd;
            }
        }
    }
}
