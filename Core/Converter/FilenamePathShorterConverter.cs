using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class FilenamePathShorterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is String)
            {
                String path = value.ToString();

                var result = path.Split(new char[] { '\\' });

                if (result.Count() >= 3)
                {
                    StringBuilder stringbuilder = new StringBuilder();
                    stringbuilder.Append(result[0]);
                    stringbuilder.Append(Path.AltDirectorySeparatorChar);
                    stringbuilder.Append(result[1]);
                    stringbuilder.Append(Path.AltDirectorySeparatorChar);
                    stringbuilder.Append("...");
                    stringbuilder.Append(Path.AltDirectorySeparatorChar);
                    stringbuilder.Append(result.Last());

                    return stringbuilder.ToString();
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}