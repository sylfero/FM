using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MenuViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        private ICommand open;
        public ICommand Open
        {
            get
            {
                if (open == null)
                {
                    open = new RelayCommand(x => mainViewModel.SwapPage("game"));
                }
                return open;
            }
        }


        private ICommand options;
        public ICommand Options
        {
            get
            {
                if (options == null)
                {
                    options = new RelayCommand(x => mainViewModel.SwapPage("options"));
                }
                return options;
            }
        }


        private ICommand exit;
        public ICommand Exit
        {
            get
            {
                if (exit == null)
                {
                    exit = new RelayCommand(x => Application.Current.MainWindow.Close());
                }
                return exit;
            }
        }
    }
}
