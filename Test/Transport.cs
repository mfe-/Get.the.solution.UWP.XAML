using System;

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
