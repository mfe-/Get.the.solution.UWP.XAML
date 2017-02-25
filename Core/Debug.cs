using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Display additional Information or provides methods for debugging
    /// </summary>
    public class Debug
    {
        public static bool GetDisplayWindowSize(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisplayWindowSizeProperty);
        }

        public static void SetDisplayWindowSize(DependencyObject obj, bool value)
        {
            obj.SetValue(DisplayWindowSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for DisplayWindowSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayWindowSizeProperty =
            DependencyProperty.RegisterAttached("DisplayWindowSize", typeof(bool), typeof(Debug), new PropertyMetadata(false, OnDisplayWindowSizeChanged));

        public static void OnDisplayWindowSizeChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            Page page = dp as Page;
            page.Loaded += (s, o) =>
            {
                if (page != null)
                {
                    Windows.UI.Xaml.Controls.Panel panel = page.Content as Windows.UI.Xaml.Controls.Panel;
                    if (panel != null)
                    {
                        TextBlock textblock = new TextBlock();
                        ApplicationView.GetForCurrentView().VisibleBoundsChanged += (sender, ev) =>
                          {
                              var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
                              var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                              var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);
                              textblock.Text = size.ToString();
                          };
                        panel.Children.Add(textblock);
                    }
                }
            };


        }

    }
}
