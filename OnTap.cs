using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    public class OnTap
    {

        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(OnTap), new PropertyMetadata(null));


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

                VirtualKey Touch;
                if (position.X<(Element.ActualWidth/2))
                {
                    Touch = VirtualKey.Left;
                }
                else
                {
                    Touch = VirtualKey.Right;
                }
                ICommand Command = GetRaiseCommand(Element);


                object Param = Element.ReadLocalValue(OnTap.CommandParameterProperty);
                if (DependencyProperty.UnsetValue != Param)
                {
                    Param = GetCommandParameter(Element);
                }

                if (Command != null)
                {
                    if(Command.CanExecute(DependencyProperty.UnsetValue == Param ? Touch : Param))
                    {
                        Command.Execute(DependencyProperty.UnsetValue == Param ? Touch : Param);
                    }
                }
            }
        }

    }

}
