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
        private Page page = new Pages.Menu();
        public Page Page
        {
            get => page;
            set => SetProperty(ref page, value);
        }
    }
}
