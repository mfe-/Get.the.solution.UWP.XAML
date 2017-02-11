using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get.the.solution.UWP.Test.App
{
    public class Transport : BindableBase
    {
        private String _Name;

        public String Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value, nameof(Name)); }
        }
    }
}
