using FM.ViewModel.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FM.ViewModel
{
    class MainGameViewModel : ViewModelBase
    {
        private MainViewModel mainViewModel = new MainViewModel();

        private ICommand team;
        public ICommand Team
        {
            get
            {
                if (team == null)
                {
                    team = new RelayCommand(x => mainViewModel.SwapPage("team"));
                }
                return team;
            }
        }

        private ICommand messages;
        public ICommand Messages
        {
            get
            {
                if (messages == null)
                {
                    messages = new RelayCommand(x => mainViewModel.SwapPage("messages"));
                }
                return messages;
            }
        }

        private ICommand transfers;
        public ICommand Transfers
        {
            get
            {
                if (transfers == null)
                {
                    transfers = new RelayCommand(x => mainViewModel.SwapPage("transfers"));
                }
                return transfers;
            }
        }

        private ICommand juniors;
        public ICommand Juniors
        {
            get
            {
                if (juniors == null)
                {
                    juniors = new RelayCommand(x => mainViewModel.SwapPage("juniors"));
                }
                return juniors;
            }
        }

        private ICommand tables;
        public ICommand Tables
        {
            get
            {
                if (tables == null)
                {
                    tables = new RelayCommand(x => mainViewModel.SwapPage("tables"));
                }
                return tables;
            }
        }

        private ICommand schedule;
        public ICommand Schedule
        {
            get
            {
                if (schedule == null)
                {
                    schedule = new RelayCommand(x => mainViewModel.SwapPage("schedule"));
                }
                return schedule;
            }
        }
    }
}
