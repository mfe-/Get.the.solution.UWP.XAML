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
