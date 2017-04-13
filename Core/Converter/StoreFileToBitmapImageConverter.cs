using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class StoreFileToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            StorageFile storeFile = value as StorageFile;
            if (storeFile != null)
            {
                BitmapImage imageSource = StorageFileToBitmapImage(storeFile);
                return imageSource;
            }
            return null;

        }
        public static BitmapImage StorageFileToBitmapImage(StorageFile savedStorageFile)
        {
            const uint size = 200; //Send your required size
            using (StorageItemThumbnail thumbnail = savedStorageFile.GetThumbnailAsync(ThumbnailMode.PicturesView, size, ThumbnailOptions.ResizeThumbnail).AsTask().Result)
            {
                if (thumbnail != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.SetSource(thumbnail);
                    return bitmapImage;
                }
                else
                {
                    return null;
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
