using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Get.the.solution.UWP.XAML
{
    public class OnDel
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

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
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(OnDel), new PropertyMetadata(null));


        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OnDel), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += (CoreDispatcher sender, AcceleratorKeyEventArgs args) =>
                {
                    IEnumerable<Control> Controls = Helper.FindVisualChildren<Control>(Window.Current.Content);
                    DependencyObject FocusedControl = Controls.FirstOrDefault(c => c.FocusState != FocusState.Unfocused);
                    //special behavior for listbox items 
                    if (FocusedControl is SelectorItem)
                    {
                        var childs = Helper.FindVisualChildren<DependencyObject>(FocusedControl);
                        FocusedControl = childs.FirstOrDefault(child => child == d);
                    }

                    if (FocusedControl == d)
                    {
                        if (args.VirtualKey == Windows.System.VirtualKey.Delete || args.VirtualKey == Windows.System.VirtualKey.GamepadY)
                        {
                            ICommand Command = GetCommand(d);

                            object Param = null;//FocusedControl.ReadLocalValue(OnDel.CommandParameterProperty);
                            //if (DependencyProperty.UnsetValue != Param)
                            //{
                                Param = GetCommandParameter(FocusedControl);
                            //}

                            if (Command != null && Command.CanExecute(Param))
                            {
                                Command.Execute(Param);
                            }
                        }
                    }
                };
            }
        }
    }
}
