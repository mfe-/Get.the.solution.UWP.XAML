using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    public class OnDrag
    {
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(OnDrag), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement dragablecontrol = d as UIElement;

            if (dragablecontrol != null)
            {
                dragablecontrol.AllowDrop = true;
                dragablecontrol.DragEnter += Dragablecontrol_DragEnter;
                dragablecontrol.DragStarting += Dragablecontrol_DragStarting;//does not get fired
                dragablecontrol.DragOver += Dragablecontrol_DragOver; //e.AcceptedOperation got move, link, copy
                dragablecontrol.Drop += Dragablecontrol_Drop;
            }

        }

        private static void Dragablecontrol_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;
            if (e.Data != null)
                e.Data.RequestedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;
           
        }

        private static void Dragablecontrol_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.RequestedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;
        }

        private static async void Dragablecontrol_DragOver(object sender, DragEventArgs e)
        {
            var d = e.GetDeferral();
            try
            {
                var items = await e.DataView.GetStorageItemsAsync();

                if (items != null)
                    e.AcceptedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;

            }
            catch { }
            finally
            {
                d.Complete();
            }
        }

        private static async void Dragablecontrol_Drop(object sender, DragEventArgs e)
        {
            ICommand Command = GetCommand(sender as DependencyObject);

            if (Command != null)
            {
                var deferral = e.GetDeferral();

                var filesAndFolders = await e.DataView.GetStorageItemsAsync();

                //http://stackoverflow.com/questions/31852576/drag-drop-storagefolders-in-uwp-windows-10-apps

                try
                {
                    Command.Execute(filesAndFolders);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    deferral.Complete();
                }



            }
        }
    }
}
