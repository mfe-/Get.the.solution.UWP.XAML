using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int result = 0;
            String text = value.ToString();
            if (value == null) return result;

            if(!String.IsNullOrWhiteSpace(text))
            {
                if (Int32.TryParse(text, out result))
                {
                    return result;
                }
                else
                {
                    return -1;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
