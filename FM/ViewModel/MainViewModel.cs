using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private static Page previousPage;
        private static Page menu = new Pages.Menu();
        private static Page options = new Pages.Options();
        private static Page save = new Pages.Save();
        private static Page mainGame = new Pages.MainGame();
        private static Page matches = new Pages.Matches();
        private static Page market = new Pages.Market();
        private static Page messages = new Pages.Messages();
        private static Page juniors = new Pages.Juniors();
        private static Page table = new Pages.Table();
        private static Page team = new Pages.Team();

        private static Page page = menu;
        public static Page Page
        {
            get => page;
            set
            {
                page = value;
                StaticPropertyChanged?.Invoke(null, PagePropertyEventArgs);
            }
        }

        private static bool canGoBack = false;
        public static bool CanGoBack
        {
            get => canGoBack;
            set
            {
                canGoBack = value;
                StaticPropertyChanged?.Invoke(null, BackPropertyEventArgs);
            }
        }

        private ICommand back;
        public ICommand Back
        {
            get
            {
                if (back == null)
                {
                    back = new RelayCommand(x => { Page tmp = previousPage; previousPage = Page; Page = tmp; CanGoBack = false; }, x => CanGoBack);
                }
                return back;
            }
        }

        public void SwapPage(string page)
        {
            previousPage = Page;
            switch (page)
            {
                case "menu":
                    Page = menu;
                    CanGoBack = false;
                    break;
                case "game":
                    Page = mainGame;
                    CanGoBack = false;
                    break;
                case "team":
                    Page = team;
                    CanGoBack = true;
                    break;
                case "messages":
                    Page = messages;
                    CanGoBack = true;
                    break;
                case "transfers":
                    Page = market;
                    CanGoBack = true;
                    break;
                case "juniors":
                    Page = juniors;
                    CanGoBack = true;
                    break;
                case "tables":
                    Page = table;
                    CanGoBack = true;
                    break;
                case "schedule":
                    Page = matches;
                    CanGoBack = true;
                    break;
                case "save":
                    Page = save;
                    CanGoBack = true;
                    break;
                case "options":
                    Page = options;
                    CanGoBack = true;
                    break;
                default:
                    break;
            }
        }

        private static readonly PropertyChangedEventArgs BackPropertyEventArgs = new PropertyChangedEventArgs(nameof(CanGoBack));
        private static readonly PropertyChangedEventArgs PagePropertyEventArgs = new PropertyChangedEventArgs(nameof(Page));
        public static event PropertyChangedEventHandler StaticPropertyChanged;
    }
}
