using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Get.the.solution.UWP.XAML
{
    public class OnEscape
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OnEscape), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += (CoreDispatcher sender, AcceleratorKeyEventArgs args) =>
                {
                    //IEnumerable<Control> Controls = Helper.FindVisualChildren<Control>(Window.Current.Content);
                    //Control FocusedControl = Controls.FirstOrDefault(c => c.FocusState != FocusState.Unfocused);
                    //if (FocusedControl == d)
                    {
                        if (args.VirtualKey == Windows.System.VirtualKey.Escape)
                        {
                            ICommand Command = GetCommand(d);

                            if (Command != null && Command.CanExecute(args.VirtualKey))
                            {
                                Command.Execute(args.VirtualKey);
                            }
                        }
                    }
                };
            }
        }

    }
}
