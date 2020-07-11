using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Get.the.solution.UWP.XAML
{
    public static class OnTextChanged
    {
        public static int GetDefaultNumber(DependencyObject obj)
        {
            return (int)obj.GetValue(DefaultNumberProperty);
        }

        public static void SetDefaultNumber(DependencyObject obj, int value)
        {
            obj.SetValue(DefaultNumberProperty, value);
        }

        // Using a DependencyProperty as the backing store for DefaultNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultNumberProperty =
            DependencyProperty.RegisterAttached("DefaultNumber", typeof(int), typeof(OnTextChanged), new PropertyMetadata(0));

        public static bool GetAllowOnlyDigit(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowOnlyDigitProperty);
        }

        public static void SetAllowOnlyDigit(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowOnlyDigitProperty, value);
        }

        // Using a DependencyProperty as the backing store for AllowOnlyDigit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowOnlyDigitProperty =
            DependencyProperty.RegisterAttached("AllowOnlyDigit", typeof(bool), typeof(OnTextChanged), new PropertyMetadata(false, OnAllowOnlyDigitChanged));

        public static void OnAllowOnlyDigitChanged(DependencyObject DependencyObject, DependencyPropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (DependencyObject is TextBox textBox)
            {
                void TextBox_LostFocus(object sender, RoutedEventArgs routedEventArgs)
                {
                    if (sender is TextBox t)
                    {
                        t.Text = new String(t.Text.Where(char.IsDigit).ToArray());
                        if (String.IsNullOrEmpty(t.Text))
                        {
                            int defaultvalue = GetDefaultNumber(t);
                            t.Text = defaultvalue.ToString();
                        }
                    }
                }
#pragma warning disable S1854 // Unused assignments are not recognized by local functions
                VirtualKey PreviewKey = VirtualKey.Accept;
#pragma warning restore S1854 // Unused assignments should be removed
                void TextBox_PreviewKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
                {
                    if (sender is TextBox t)
                    {
                        if (
                            ((int)e.Key <= 58 && (int)e.Key >= 48) ||
                            ((int)e.Key <= 105 && (int)e.Key >= 96) ||
                            e.Key == VirtualKey.Tab ||
                            e.Key == VirtualKey.Back ||
                            e.Key == VirtualKey.Delete ||
                            e.Key == VirtualKey.Control
                          )
                        {
                            e.Handled = false;
                            PreviewKey = e.Key;
                        }
                        else if (VirtualKey.Control.Equals(PreviewKey) &&
                            (e.Key == VirtualKey.A || e.Key == VirtualKey.C || e.Key == VirtualKey.V))
                        {
                            e.Handled = false;
                            PreviewKey = VirtualKey.Accept;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                }

                if (propertyChangedEventArgs.NewValue is bool b)
                {
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                    textBox.LostFocus += TextBox_LostFocus;
                }
                else
                {
                    textBox.PreviewKeyDown -= TextBox_PreviewKeyDown;
                    textBox.LostFocus -= TextBox_LostFocus;
                }
            }
        }
    }
}
