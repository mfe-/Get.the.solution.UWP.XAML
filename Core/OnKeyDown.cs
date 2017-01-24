using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    public class OnKeyDown
    {

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OnKeyDown), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                Window.Current.CoreWindow.KeyDown += (Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args) =>
                {
                    ICommand Command = GetCommand(d);

                    if (Command != null && Command.CanExecute(args.VirtualKey))
                    {
                        Command.Execute(args.VirtualKey);
                    }
                };
            }
        }

    }

}
