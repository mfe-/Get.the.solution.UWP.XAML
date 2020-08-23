using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML.Converter
{
    /// <summary>
    /// Converts hex color code to <see cref="SolidColorBrush"/> 
    /// or <see cref="AcrylicBrush"/> if the neccessary api is avaliable
    /// </summary>
    /// <remarks>
    /// Use the DP <seealso cref="HexStringToSolidColorBrushConverter.Opacity"/> and <seealso cref="HexStringToSolidColorBrushConverter.TintOpacity"/> for AcrylicBrush
    /// </remarks>
    /// <example>
    /// <code>
    ///  <local:HexStringToSolidColorBrushConverter x:Key="HexStringToSolidColorBrushConverter" 
    ///                                            Opacity="{Binding Path=Opacity,UpdateSourceTrigger=PropertyChanged}" 
    ///                                            TintOpacity="{Binding Path=TintOpacity,UpdateSourceTrigger=PropertyChanged}" />
    /// </code>
    /// </example>
    public class HexStringToSolidColorBrushConverter : DependencyObject, IValueConverter
    {
        public double TintOpacity
        {
            get { return (double)GetValue(TintOpacityProperty); }
            set { SetValue(TintOpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TintOpacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TintOpacityProperty =
            DependencyProperty.Register("TintOpacity", typeof(double), typeof(HexStringToSolidColorBrushConverter), new PropertyMetadata(0));

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Opacity.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(HexStringToSolidColorBrushConverter), new PropertyMetadata(3));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && (s.Length >= 7 && s.Contains("#")))
            {
                SolidColorBrush solidColorBrush = GetSolidColorBrush(s);
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"))
                {
                    AcrylicBrush myBrush = new AcrylicBrush();
                    myBrush.BackgroundSource = AcrylicBackgroundSource.HostBackdrop;
                    myBrush.TintColor = solidColorBrush.Color;
                    myBrush.FallbackColor = solidColorBrush.Color;
                    myBrush.TintOpacity = TintOpacity;
                    myBrush.Opacity = Opacity;
                    return myBrush;
                }
                return solidColorBrush;
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
