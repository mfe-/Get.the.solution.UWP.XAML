using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class NavigationViewItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<MenuItem> menuItems)
            {
                List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();
                foreach (MenuItem item in menuItems)
                {
                    navigationViewItems.Add(MenuItemToNavigationViewItem(item));
                }
                return navigationViewItems;
            }
            else if (value is MenuItem menuItem)
            {
                return MenuItemToNavigationViewItem(menuItem);
            }
            return value;
        }
        public MenuItem NavigationViewItemToMenuItem(NavigationViewItem navigationViewItem)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Name = navigationViewItem.Content?.ToString();
            menuItem.PageType = navigationViewItem.Tag as Type;
            if (navigationViewItem.Icon is SymbolIcon symbolIcon)
            {
                menuItem.Icon = symbolIcon.Symbol.ToString();
            }
            return menuItem;
        }
        public NavigationViewItem MenuItemToNavigationViewItem(MenuItem menuItem)
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();
            navigationViewItem.Content = menuItem?.Name;
            navigationViewItem.Tag = menuItem?.PageType;
            Symbol symbol = Symbol.Accept;
            if (menuItem != null)
            {
                Enum.TryParse<Symbol>(menuItem.Icon, out symbol);
            }
            navigationViewItem.Icon = new SymbolIcon() { Symbol = symbol };
            return navigationViewItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is NavigationViewItem navigationViewItem)
            {
                return NavigationViewItemToMenuItem(navigationViewItem);
            }
            return value;
        }
    }
}
