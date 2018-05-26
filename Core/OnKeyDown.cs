using System;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Provides attached dependency properties to execute a command on the keydown event.
    /// </summary>
    /// <example>
    /// common:OnKeyDown.RequireCtrl="True"
    /// common:OnKeyDown.Command="{Binding Path=CtrlOpenCommand}"
    /// </example>
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

        /// <summary>
        /// Registers the key down event and executes the command depending on the other attaced properties values
        /// </summary>
        /// <param name="d">The dependency object</param>
        /// <param name="e">Provides event related informations</param>
        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d as UIElement != null)
            {
                Window.Current.CoreWindow.KeyDown += (Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args) =>
                {
                    if ((bool)GetCtrl(d) == true)
                    {
                        //exit current operation if the user didn't pressed Ctrl
                        if (CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control) == CoreVirtualKeyStates.None)
                        {
                            return;
                        }
                        //if only ctrl is pressed exit current operation
                        if (args.VirtualKey == VirtualKey.Control)
                        {
                            return;
                        }
                    }
                    object commandParameter = GetCommandParameter(d);
                    //set default command parameter 
                    if (commandParameter == null)
                    {
                        commandParameter = args.VirtualKey;
                    }
                    string key = $"{GetOnKey(d)}".ToUpper();
                    if (!string.IsNullOrEmpty(key))
                    {
                        VirtualKey virtualKey;
                        virtualKey = (VirtualKey)Enum.Parse(typeof(VirtualKey), key);
                        if (virtualKey != args.VirtualKey) return;
                    }
                    ICommand Command = GetCommand(d);
                    if (Command != null && Command.CanExecute(commandParameter))
                    {
                        Command.Execute(commandParameter);
                    }
                };
            }
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
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(OnKeyDown), new PropertyMetadata(null));



        public static string GetOnKey(DependencyObject obj)
        {
            return (string)obj.GetValue(OnKeyProperty);
        }

        public static void SetOnKey(DependencyObject obj, string value)
        {
            obj.SetValue(OnKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnKeyProperty =
            DependencyProperty.RegisterAttached("OnKey", typeof(string), typeof(OnKeyDown), new PropertyMetadata(string.Empty));



        public static bool GetCtrl(DependencyObject obj)
        {
            return (bool)obj.GetValue(CtrlProperty);
        }

        public static void SetCtrl(DependencyObject obj, bool value)
        {
            obj.SetValue(CtrlProperty, value);
        }

        // Using a DependencyProperty as the backing store for Ctrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CtrlProperty =
            DependencyProperty.RegisterAttached("Ctrl", typeof(bool), typeof(OnKeyDown), new PropertyMetadata(false));



    }

}
