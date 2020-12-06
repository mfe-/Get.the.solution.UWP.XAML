using Get.the.solution.UWP.XAML;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App1
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            MenueItems = new List<MenuItem>()
            {
                new MenuItem() { Name = "Page 1"}
                ,new MenuItem() { Name = "Page 2"}
            };

            string s = AppHelper.GetWindowsVersion();
            string l = AppHelper.GetWindowsBuildNumber(AppHelper.GetWindowsDeviceVersion()).ToString();
        }


        private List<MenuItem> _MenueItems;
        public List<MenuItem> MenueItems
        {
            get { return _MenueItems; }
            set
            {
                _MenueItems = value;
                OnPropertyChanged(nameof(MenueItems));
            }
        }

        private ICommand _CtrlOpenCommand;
        public ICommand CtrlOpenCommand => _CtrlOpenCommand ?? (_CtrlOpenCommand = new DelegateCommand<object>(OnCtrlOpenCommandAsync));

        protected void OnCtrlOpenCommandAsync(object param)
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
