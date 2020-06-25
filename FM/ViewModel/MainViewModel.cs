using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private static Page previousPage;
        private static Page menu = new Pages.Menu();
        private static Page mainGame = new Pages.MainGame();

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
