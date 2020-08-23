using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class IsObjectNullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Checks whether the overgiven <paramref name="value"/> is null and returns <seealso cref="Visibility.Collapsed"/>. 
        /// Otherwise <seealso cref="Visibility.Visible"/>. To Invert this behaviour set any value on <paramref name="parameter"/>
        /// </summary>
        /// <param name="value">If binded object is null it returns as visibility collapsed.</param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Set a value to invert the behaviour of the converter</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visible = false;
            if (value != null)
            {
                visible = true;
            }
            else
            {
                visible = false;
            }
            if (parameter != null)
            {
                visible = !visible;
            }
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
