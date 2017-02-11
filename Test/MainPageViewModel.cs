using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get.the.solution.UWP.Test.App
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel()
        {
            DragCommand = new DelegateCommand<object>(OnDragCommand);
            Transports = new ObservableCollection<Transport>();
            Transports.Add(new Car() { Name = "Car" });
            Transports.Add(new Car() { Name = "Car 1" });
            Transports.Add(new Bike() { Name = "Bike 1" });


        }
        public DelegateCommand<object> DragCommand { get; set; }

        protected void OnDragCommand(object param)
        {

        }
        protected bool CanDragCommandExecuted()
        {
            return true;
        }


        private ObservableCollection<Transport> _Transports;

        public ObservableCollection<Transport> Transports
        {
            get { return _Transports; }
            set { SetProperty(ref _Transports, value, nameof(Transports)); }
        }
    }
}
