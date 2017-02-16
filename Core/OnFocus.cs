using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Get.the.solution.UWP.XAML
{
    public class OnFocus
    {
        #region Select All
        public static bool GetSelectAll(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllProperty);
        }

        public static void SetSelectAll(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectAll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectAllProperty =
            DependencyProperty.RegisterAttached("SelectAll", typeof(bool), typeof(OnFocus), new PropertyMetadata(false, OnSelectAllChanged));

        public static void OnSelectAllChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox TextBox = DependencyObject as TextBox;
            if (TextBox != null)
            {
                TextBox.GotFocus += (sender, ee) => TextBox.SelectAll();
            }

        }
        #endregion
    }
}
