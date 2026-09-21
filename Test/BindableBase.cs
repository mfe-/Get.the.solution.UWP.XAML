using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Get.the.solution.UWP.Test.App
{
    /// <summary>
    /// Minimal INotifyPropertyChanged base replacing the former Prism.Mvvm.BindableBase.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
