using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class HexStringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && (s.Length >= 7 && s.Contains("#")))
            {
                return GetSolidColorBrush(s);
            }
            return new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is SolidColorBrush solidColorBrush)
            {
                string hex = $"#{(int)solidColorBrush.Color.A}{(int)solidColorBrush.Color.R}{(int)solidColorBrush.Color.G}{(int)solidColorBrush.Color.B}";
                return hex;
            }
            return value;
        }
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("\t", "#");
            hex = hex.Replace("#", string.Empty);
            byte a = 255; //alpha channel for opacity
            int position = 0;
            if (hex.Length >= 8)
            {
                a = (byte)(System.Convert.ToUInt32(hex.Substring(position, 2), 16));
                position += 2;
            }
            byte r = (byte)(System.Convert.ToUInt32(hex.Substring(position, 2), 16));
            position += 2;
            byte g = (byte)(System.Convert.ToUInt32(hex.Substring(position, 2), 16));
            position += 2;
            byte b = (byte)(System.Convert.ToUInt32(hex.Substring(position, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;

        }
    }
}
