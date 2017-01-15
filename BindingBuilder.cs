using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// http://stackoverflow.com/questions/11857505/how-do-i-do-bindings-in-itemcontainerstyle-in-winrt
    /// </summary>
    public class BindingBuilder
    {

        //TODO
        public static String GetBinding(DependencyObject obj)
        {
            return (String)obj.GetValue(BindingProperty);
        }

        public static void SetBinding(DependencyObject obj, String value)
        {
            obj.SetValue(BindingProperty, value);
        }

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.RegisterAttached("Binding", typeof(String), typeof(BindingBuilder), new PropertyMetadata("", OnRelativeSoureBindingChanged));



        public static void OnRelativeSoureBindingChanged(DependencyObject dp,DependencyPropertyChangedEventArgs e)
        {
            //todo Can be used to set binding in styles
        }
    }
}
