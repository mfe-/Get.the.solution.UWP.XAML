using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Get.the.solution.ui
{
    public class OnTap
    {
        public static ICommand GetRaiseCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(RaiseCommandProperty);
        }

        public static void SetRaiseCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(RaiseCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for RaiseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaiseCommandProperty =
            DependencyProperty.RegisterAttached("RaiseCommand", typeof(ICommand), typeof(OnTap), new PropertyMetadata(null, OnRaiseCommandPropertyChanged));

        private static void OnRaiseCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                UIElement Element = (d as UIElement);
                Element.Tapped += Element_Tapped;
            }
        }

        /// <summary>
        /// Determine on a tap event, whether the tap was left or right relative from the current user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Element_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender as UIElement != null)
            {
                FrameworkElement Element = sender as FrameworkElement;
                var type = e.PointerDeviceType.ToString();
                var position = e.GetPosition(Element);

                TouchSide Touch;
                if (position.X<(Element.ActualWidth/2))
                {
                    Touch = TouchSide.Left;
                }
                else
                {
                    Touch = TouchSide.Right;
                }
                ICommand Command = GetRaiseCommand(Element);
                if (Command != null)
                {
                    if(Command.CanExecute(Touch))
                    {
                        Command.Execute(Touch);
                    }
                }
            }
        }

    }

}
