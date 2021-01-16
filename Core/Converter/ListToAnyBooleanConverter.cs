using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    /// <summary>
    /// Converts a list to a boolean value which indicates whether the list contains any element
    /// </summary>
    public class ListToAnyBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts the overgiven value to <see cref="IEnumerable{T}"/> and returns the result of <seealso cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<object> ts)
            {
                return ts.Any();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
