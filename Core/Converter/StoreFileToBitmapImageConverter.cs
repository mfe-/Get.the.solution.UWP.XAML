using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class StoreFileToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is StorageFile storeFile)
            {
                BitmapImage imageSource = StorageFileToBitmapImage(storeFile);
                return imageSource;
            }
            return null;

        }
        public static BitmapImage StorageFileToBitmapImage(StorageFile savedStorageFile)
        {
            try
            {
                using (IRandomAccessStream fileStream = savedStorageFile.OpenAsync(FileAccessMode.Read).AsTask().Result)
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fileStream);
                    return bitmapImage;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
