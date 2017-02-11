using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Represents a MenuItem for the HamburgerMenu from the UWPToolkit
    /// <seealso cref="https://developer.microsoft.com/en-us/windows/uwp-community-toolkit/controls/hamburgermenu"/>
    /// </summary>
    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }
    }
}
