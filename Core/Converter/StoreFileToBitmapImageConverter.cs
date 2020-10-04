using System;
using System.IO;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Get.the.solution.UWP.XAML.Converter
{
    public class StoreFileToBitmapImageConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool disposeFileStreamAfterBitmap = true;
            if (parameter is bool b)
            {
                disposeFileStreamAfterBitmap = b;
            }
            if (value is StorageFile storeFile)
            {
                BitmapImage = StorageFileToBitmapImage(storeFile, disposeFileStreamAfterBitmap);
                return BitmapImage;
            }
            if (value is Stream stream)
            {
                BitmapImage = StreamToBitmapImage(stream, disposeFileStreamAfterBitmap);
                return BitmapImage;
            }
            return null;
        }

        public ICommand DownloadProgressCommand
        {
            get { return (ICommand)GetValue(DownloadProgressCommandProperty); }
            set { SetValue(DownloadProgressCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DownloadProgressCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadProgressCommandProperty =
            DependencyProperty.Register("DownloadProgressCommand", typeof(ICommand), typeof(StoreFileToBitmapImageConverter), new PropertyMetadata(0));


        public IRandomAccessStream RandomAccessStream
        {
            get { return (IRandomAccessStream)GetValue(RandomAccessStreamProperty); }
            set { SetValue(RandomAccessStreamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RandomAccessStream.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RandomAccessStreamProperty =
            DependencyProperty.Register("RandomAccessStream", typeof(IRandomAccessStream), typeof(StoreFileToBitmapImageConverter), new PropertyMetadata(null));

        public BitmapImage BitmapImage
        {
            get { return (BitmapImage)GetValue(BitmapImageProperty); }
            set { SetValue(BitmapImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BitmapImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BitmapImageProperty =
            DependencyProperty.Register("BitmapImage", typeof(BitmapImage), typeof(StoreFileToBitmapImageConverter), new PropertyMetadata(null));



        public BitmapImage StreamToBitmapImage(Stream stream, bool dispose = true)
        {
            try
            {
                if (stream.Position != 0)
                {
                    stream.Position = 0;
                }
                RandomAccessStream = stream.AsRandomAccessStream();
                // Set the image source to the selected bitmap 
                BitmapImage bitmapImage = ToBitmapImage(RandomAccessStream);
                if (dispose) RandomAccessStream.Dispose();
                return bitmapImage;

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        public BitmapImage StorageFileToBitmapImage(StorageFile savedStorageFile, bool dispose = true)
        {
            try
            {
                RandomAccessStream = savedStorageFile.OpenAsync(FileAccessMode.Read).AsTask().Result;
                // Set the image source to the selected bitmap 
                BitmapImage bitmapImage = ToBitmapImage(RandomAccessStream);
                if (dispose) RandomAccessStream.Dispose();
                return bitmapImage;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        private BitmapImage ToBitmapImage(IRandomAccessStream fileStream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.DownloadProgress += BitmapImage_DownloadProgress;
            bitmapImage.ImageOpened += BitmapImage_ImageOpened;
            bitmapImage.SetSource(fileStream);
            return bitmapImage;
        }

        private static void BitmapImage_ImageOpened(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is BitmapImage bitmap)
            {
                bitmap.ImageOpened -= BitmapImage_ImageOpened;
            }
        }

        private void BitmapImage_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if (sender is BitmapImage bitmap && e.Progress == 100)
            {
                bitmap.DownloadProgress -= BitmapImage_DownloadProgress;
            }
            DownloadProgressCommand?.Execute(e.Progress);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
