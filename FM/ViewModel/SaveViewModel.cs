using FM.Pages;
using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ICommand load;
        public ICommand Load
        {
            get
            {
                if (load == null)
                {
                    load = new RelayCommand(x => { DAL.DBConnection.Instance.SetDatabase(currentSave + "\\Database.db");  mainViewModel.SwapPage("game"); }, x => !string.IsNullOrEmpty(CurrentSave));
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
                        NewSave = null;
                    }, x => !string.IsNullOrEmpty(NewSave) && !Saves.Any(y => y == NewSave));
                }
                return confirmAdd;
            }
        }
    }
}
