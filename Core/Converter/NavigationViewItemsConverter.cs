using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class NavigationViewItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value!=null && value as IEnumerable<MenuItem> !=null)
            {
                IEnumerable<MenuItem> menuItems = value as IEnumerable<MenuItem>;
                List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();
                foreach (MenuItem item in menuItems)
                {
                    navigationViewItems.Add(MenuItemToNavigationViewItem(item));
                }
                return navigationViewItems;
            }
            else if(value != null && value as MenuItem != null)
            {
                MenuItem menuItem = value as MenuItem;
                return MenuItemToNavigationViewItem(menuItem);
            }
            return value;
        }
        public MenuItem NavigationViewItemToMenuItem(NavigationViewItem navigationViewItem)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Name = navigationViewItem.Content?.ToString();
            menuItem.PageType = navigationViewItem.Tag as Type;
            menuItem.Icon = (navigationViewItem.Icon as SymbolIcon).Symbol;
            return menuItem;
        }
        public NavigationViewItem MenuItemToNavigationViewItem(MenuItem menuItem)
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();
            navigationViewItem.Content = menuItem?.Name;
            navigationViewItem.Tag = menuItem?.PageType;
            navigationViewItem.Icon = new SymbolIcon() { Symbol = menuItem.Icon };
            return navigationViewItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value as NavigationViewItem != null)
            {
                NavigationViewItem navigationViewItem = value as NavigationViewItem;
                return NavigationViewItemToMenuItem(navigationViewItem);
            }
            return value;
        }
    }
}
