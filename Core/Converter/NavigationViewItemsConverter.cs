using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    /// <summary>
    /// Converts a <seealso cref="MenuItem"/> to a NavigationViewItem so it can be used for the NavigationView.
    /// If an other type than <seealso cref="MenuItem"/> is provided it tries to create the <seealso cref="MenuItem"/> for you.
    /// Just make sure the necessary properties are there.
    /// </summary>
    /// <example>
    /// <NavigationView 
    ///                 MenuItemsSource="{Binding Path=Items,Converter={StaticResource NavigationViewItemsConverter}}"
    ///                 SelectedItem="{Binding 
    ///                 Path=SelectedMenuItem,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged,
    ///                 Converter={StaticResource NavigationViewItemsConverter}}">
    /// </example>
    public class NavigationViewItemsConverter : IValueConverter
    {
        private Type TargetTypSingle = null;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IEnumerable<MenuItem> menuItems1 = null;
            MenuItem menuItem = null;
            //check for the correct menuitem typ
            if (value is IEnumerable<MenuItem> ms)
            {
                menuItems1 = ms;
            }
            else if (value is MenuItem m)
            {
                menuItem = m;
            }
            else
            {
                //check if we can convert the unknown menuitem to our typ
                if (value is System.Collections.IEnumerable enumerable)
                {
                    menuItems1 = TryConvertToMenuItem(enumerable);
                }
                else
                {
                    PropertyInfo namePropertie;
                    PropertyInfo pageTypePropertie;
                    PropertyInfo iconPropertie;
                    CheckMenuItemType(value.GetType(), out namePropertie, out pageTypePropertie, out iconPropertie);
                    CreateMenuItem(namePropertie, pageTypePropertie, iconPropertie, value);
                }

            }


            if (menuItems1 is IEnumerable<MenuItem> menuItems)
            {
                List<NavigationViewItem> navigationViewItems = new List<NavigationViewItem>();
                foreach (MenuItem item in menuItems)
                {
                    navigationViewItems.Add(MenuItemToNavigationViewItem(item));
                }
                return navigationViewItems;
            }
            else
            {
                return MenuItemToNavigationViewItem(menuItem);
            }
        }
        public object NavigationViewItemToMenuItem(NavigationViewItem navigationViewItem)
        {
            if (TargetTypSingle == null)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Name = navigationViewItem.Content?.ToString();
                menuItem.PageType = navigationViewItem.Tag as Type;
                menuItem.Icon = $"{(navigationViewItem.Icon as SymbolIcon).Symbol}";
                return menuItem;
            }
            else
            {
                object createdInstance = Activator.CreateInstance(TargetTypSingle);
                PropertyInfo namePropertie;
                PropertyInfo pageTypePropertie;
                PropertyInfo iconPropertie;
                CheckMenuItemType(createdInstance.GetType(), out namePropertie, out pageTypePropertie, out iconPropertie);
                namePropertie.SetValue(createdInstance, navigationViewItem.Content?.ToString());
                pageTypePropertie.SetValue(createdInstance, navigationViewItem.Tag as Type);
                iconPropertie.SetValue(createdInstance, $"{(navigationViewItem.Icon as SymbolIcon).Symbol}");
                return createdInstance;
            }

        }
        public NavigationViewItem MenuItemToNavigationViewItem(MenuItem menuItem)
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();
            navigationViewItem.Content = menuItem?.Name;
            navigationViewItem.Tag = menuItem?.PageType;
            Symbol symbol = Symbol.Accept;
            if (menuItem != null)
            {
                Enum.TryParse(menuItem.Icon, out symbol);
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

        public IEnumerable<MenuItem> TryConvertToMenuItem(System.Collections.IEnumerable value)
        {
            var enumerator = value.GetEnumerator();
            enumerator.MoveNext();
            //get first menu item
            Type menuitemType = enumerator.Current.GetType();
            //save the type for later (convertback)
            TargetTypSingle = menuitemType;
            //check if all neccessary properties are available
            PropertyInfo namePropertie, pageTypePropertie, iconPropertie;
            CheckMenuItemType(menuitemType, out namePropertie, out pageTypePropertie, out iconPropertie);
            //create the menuitems
            List<MenuItem> menuItems = new List<MenuItem>();
            foreach (var item in value)
            {
                MenuItem menuItem = CreateMenuItem(namePropertie, pageTypePropertie, iconPropertie, item);
                menuItems.Add(menuItem);
            }
            return menuItems;
        }

        private static MenuItem CreateMenuItem(PropertyInfo namePropertie, PropertyInfo pageTypePropertie, PropertyInfo iconPropertie, object item)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Name = (string)namePropertie.GetValue(item);
            menuItem.PageType = (Type)pageTypePropertie.GetValue(item);
            menuItem.Icon = (string)iconPropertie.GetValue(item);
            return menuItem;
        }

        private static void CheckMenuItemType(Type value, out PropertyInfo namePropertie, out PropertyInfo pageTypePropertie, out PropertyInfo iconPropertie)
        {
            var properties = value.GetProperties();
            namePropertie = properties.FirstOrDefault(a => a.Name == nameof(UWP.XAML.MenuItem.Name) && a.PropertyType == typeof(string));
            pageTypePropertie = properties.FirstOrDefault(a => a.Name == nameof(UWP.XAML.MenuItem.PageType) && a.PropertyType == typeof(Type));
            iconPropertie = properties.FirstOrDefault(a => a.Name == nameof(UWP.XAML.MenuItem.Icon) && a.PropertyType == typeof(string));
            if (namePropertie == null)
            {
                throw new ArgumentException(
                    $"The used Menuitem of Type {value} should implement a property named {nameof(UWP.XAML.MenuItem.Name)} of type {typeof(string)}", nameof(value));
            }
            if (pageTypePropertie == null)
            {
                throw new ArgumentException(
                    $"The used Menuitem of Type {value} should implement a property named {nameof(UWP.XAML.MenuItem.PageType)} of type {typeof(Type)}", nameof(value));
            }
            if (iconPropertie == null)
            {
                throw new ArgumentException(
                    $"The used Menuitem of Type {value} should implement a property named {nameof(UWP.XAML.MenuItem.Icon)} of type {typeof(string)}", nameof(value));
            }
        }
    }
}
