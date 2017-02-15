using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class ImagePropertiesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ImageProperties ImageProperties = null;
            if (value == null) return String.Empty;
            if (value.GetType().Equals(typeof(StorageFile)))
            {
                StorageFile StorageFile = value as StorageFile;
                ImageProperties = StorageFile.Properties.GetImagePropertiesAsync().GetAwaiter().GetResult();
            }
            else if (value.GetType().Equals(typeof(StorageFile)))
            {
                ImageProperties = value as ImageProperties;
            }

            if (ImageProperties != null)
            {
                if (ImageProperties == null) return String.Empty;

                return $"{ImageProperties.Width}x{ImageProperties.Height}";
            }
            else
            {
                throw new ArgumentException($"Image StorageFile expected. Got {value.GetType()}");
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
