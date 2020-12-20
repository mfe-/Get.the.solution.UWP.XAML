using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Get.the.solution.UWP.XAML.Converter
{
    /// <summary>
    /// Converts <seealso cref="Stream"/>, <seealso cref="StorageFile"/> to an ImageSource for the <seealso cref="Windows.UI.Xaml.Controls.Image"/>
    /// </summary>
    public class ImageSourceConverter : DependencyObject, IValueConverter
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
                ImageSource = StorageFileToImageSource(storeFile, disposeFileStreamAfterBitmap);
                return ImageSource;
            }
            if (value is Stream stream)
            {
                ImageSource = StreamToImageSource(stream, disposeFileStreamAfterBitmap);
                return ImageSource;
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
            DependencyProperty.Register(nameof(DownloadProgressCommand), typeof(ICommand), typeof(ImageSourceConverter), new PropertyMetadata(null));

        public ICommand ImageOpenedCommand
        {
            get { return (ICommand)GetValue(ImageOpenedCommandProperty); }
            set { SetValue(ImageOpenedCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageOpenedCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageOpenedCommandProperty =
            DependencyProperty.Register(nameof(ImageOpenedCommand), typeof(ICommand), typeof(ImageSourceConverter), new PropertyMetadata(null));

        public IRandomAccessStream RandomAccessStream
        {
            get { return (IRandomAccessStream)GetValue(RandomAccessStreamProperty); }
            set { SetValue(RandomAccessStreamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RandomAccessStream.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RandomAccessStreamProperty =
            DependencyProperty.Register(nameof(RandomAccessStream), typeof(IRandomAccessStream), typeof(ImageSourceConverter), new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BitmapImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(ImageSourceConverter), new PropertyMetadata(null));

        public async Task<ImageSource> SoftwareBitmapToBitmapImageAsync(SoftwareBitmap softwareBitmap, bool dispose = true)
        {
            var softwareBitmapSourcesource = new SoftwareBitmapSource();
            await softwareBitmapSourcesource.SetBitmapAsync(softwareBitmap);
            return softwareBitmapSourcesource;
        }

        public ImageSource StreamToImageSource(Stream stream, bool dispose = true)
        {
            try
            {
                if (stream.Position != 0)
                {
                    stream.Position = 0;
                }
                RandomAccessStream = stream.AsRandomAccessStream();
                return RandomAccessStreamToBitmapImage(RandomAccessStream, dispose);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
        public ImageSource RandomAccessStreamToBitmapImage(IRandomAccessStream randomAccessStream, bool dispose = true)
        {
            try
            {
                // Set the image source to the selected bitmap 
                ImageSource bitmapImage = ToBitmapImage(randomAccessStream);
                if (dispose) RandomAccessStream.Dispose();
                return bitmapImage;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }
        public ImageSource StorageFileToImageSource(StorageFile savedStorageFile, bool dispose = true)
        {
            try
            {
                RandomAccessStream = savedStorageFile.OpenAsync(FileAccessMode.Read).AsTask().Result;
                return RandomAccessStreamToBitmapImage(RandomAccessStream, dispose);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return null;
        }

        private ImageSource ToBitmapImage(IRandomAccessStream fileStream)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.CreateOptions = BitmapCreateOptions.None;
            bitmapImage.DownloadProgress += BitmapImage_DownloadProgress;
            bitmapImage.ImageOpened += BitmapImage_ImageOpened;
            bitmapImage.SetSource(fileStream);
            return bitmapImage;
        }

        private void BitmapImage_ImageOpened(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is BitmapImage bitmap)
            {
                bitmap.ImageOpened -= BitmapImage_ImageOpened;
            }


            ImageOpenedCommand?.Execute(sender);
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
