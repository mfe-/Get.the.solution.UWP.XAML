using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    /// <summary>
    /// Converts a bool value to a <see cref="Visibility"/> value
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> value to a <see cref="Visibility"/> value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Set any value to invers the input boolean</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                if (parameter != null)
                {
                    b = !b;
                }
                return b ? Visibility.Visible : Visibility.Collapsed;
            }
            return value;
        }
        /// <summary>
        /// Converts a <see cref="Visibility"/> value to a <see cref="bool"/> value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">Set any value to invers the output boolean</param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                bool b = visibility == Visibility.Visible;
                if (parameter != null)
                {
                    b = !b;
                }
                return b;
            }
            return value;
        }
    }
}
