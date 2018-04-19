using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.Core;

namespace Get.the.solution.UWP.XAML
{
    public class OnBackRequested
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
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OnBackRequested), new PropertyMetadata(null, OnCommandPropertyChanged));


        /// <summary>
        /// <see cref="http://stackoverflow.com/questions/30597585/windows-10-uap-back-button"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                //TODO
                if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
                {
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed += (s, args) =>
                    {
                        args.Handled = true;
                        ExecuteCommand(d, args);
                    };
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested += (s, args) =>
                    {
                        IEnumerable<Control> Controls = Helper.FindVisualChildren<Control>(Window.Current.Content);
                        Control FocusedControl = Controls.FirstOrDefault(c => c.FocusState != FocusState.Unfocused);
                        if (FocusedControl == d)
                        {
                            ExecuteCommand(d, args);
                            args.Handled = true;
                        }
                    };
                }
               
            }
        }
        private static void ExecuteCommand(DependencyObject d,object defaultCommandParameter)
        {
            ICommand Command = GetCommand(d);

            if (Command != null && Command.CanExecute(defaultCommandParameter))
            {
                Command.Execute(defaultCommandParameter);
            }
        }

    }
}
