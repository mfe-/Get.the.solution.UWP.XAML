using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Binding to the Window Title in UWP
    /// http://www.spikie.be/post/2017/07/28/SettingWindowTitleInBinding.html
    /// </summary>
    public static class PageExtensions
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached("Title", typeof(string),
                typeof(PageExtensions),
                new PropertyMetadata(string.Empty, OnTitlePropertyChanged));

        public static string GetTitle(DependencyObject d)
        {
            return d.GetValue(TitleProperty).ToString();
        }

        public static void SetTitle(DependencyObject d, string value)
        {
            d.SetValue(TitleProperty, value);
        }

        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string title = e.NewValue.ToString();
            var window = ApplicationView.GetForCurrentView();
            window.Title = title;
        }
    }
}
