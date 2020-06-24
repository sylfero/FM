using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FM.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private static Page page = new Pages.Menu();
        public static Page Page
        {
            get => page;
            set
            {
                page = value;
                OnPageChanged(EventArgs.Empty);
            }
        }

        public void SwapPage(Page page)
        {
            Page = page;
        }

        public static event EventHandler PageChanged;
        protected static void OnPageChanged(EventArgs e)
        {
            PageChanged?.Invoke(null, e);
        }
    }
}
