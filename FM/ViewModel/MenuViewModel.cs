using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MenuViewModel : ViewModelBase
    {
        MainViewModel m = new MainViewModel();
        private ICommand open;
        public ICommand Open
        {
            get
            {
                if (open == null)
                {
                    open = new RelayCommand(x => m.SwapPage(new Pages.Save()));
                }
                return open;
            }
        }
    }
}
