using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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
        }
        public DelegateCommand<object> DragCommand { get; set; }

        protected void OnDragCommand(object param)
        {

        }
        protected bool CanDragCommandExecuted()
        {
            return true;
        }
    }
}
