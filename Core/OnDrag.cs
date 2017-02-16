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
        #region DropCommand
        public static ICommand GetDropCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DropCommandProperty);
        }

        public static void SetDropCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DropCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for DropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.RegisterAttached("DropCommand", typeof(ICommand), typeof(OnDrag), new PropertyMetadata(null, OnCommandPropertyChanged));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement dragablecontrol = d as UIElement;
            if (dragablecontrol != null)
            {
                dragablecontrol.AllowDrop = true;
                if(GetOverCommand(dragablecontrol)==null)
                {
                    dragablecontrol.DragOver -= Dragablecontrol_DragOver;
                    dragablecontrol.DragOver += Dragablecontrol_DragOver; //e.AcceptedOperation got move, link, copy
                }

                dragablecontrol.DragStarting += Dragablecontrol_DragStarting;//does not get fired

                if (GetEnterCommand(dragablecontrol)==null)
                {
                    dragablecontrol.DragEnter -= Dragablecontrol_DragEnter;
                    dragablecontrol.DragEnter += Dragablecontrol_DragEnter;
                }

                dragablecontrol.Drop += Dragablecontrol_Drop; //e.DataView.RequestedOperation is set none
            }
        }
        #endregion

        #region DragEnter

        public static ICommand GetEnterCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(EnterCommandProperty);
        }

        public static void SetEnterCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(EnterCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnterCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.RegisterAttached("EnterCommand", typeof(ICommand), typeof(OnDrag), new PropertyMetadata(null, OnCommandDragEnterPropertyChanged));


        private static void OnCommandDragEnterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement dragablecontrol = d as UIElement;
            if (dragablecontrol != null)
            {
                dragablecontrol.AllowDrop = true;
                dragablecontrol.DragEnter -= Dragablecontrol_DragEnter;
                dragablecontrol.DragEnter += Dragablecontrol_DragEnter;
            }
        }
        private static void Dragablecontrol_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;
            if (e.Data != null)
                e.Data.RequestedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;

            ICommand Command = GetEnterCommand(sender as DependencyObject);
            if (Command != null)
            {
                Command.Execute(e);
            }
        }
        #endregion

        #region DragOverCommand

        public static ICommand GetOverCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(OverCommandProperty);
        }

        public static void SetOverCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(OverCommandProperty, value);
        }

        // Using a DependencyProperty as the backing store for OverCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverCommandProperty =
            DependencyProperty.RegisterAttached("OverCommand", typeof(ICommand), typeof(OnDrag), new PropertyMetadata(null, OnOverCommandPropertyChanged));

        private static void OnOverCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement dragablecontrol = d as UIElement;
            if (dragablecontrol != null)
            {
                dragablecontrol.AllowDrop = true;
                dragablecontrol.DragOver -= Dragablecontrol_DragOver;
                dragablecontrol.DragOver += Dragablecontrol_DragOver;
            }
        }
        private static void Dragablecontrol_DragOver(object sender, DragEventArgs e)
        {
            var d = e.GetDeferral();
            try
            {
                //var items = await e.DataView.GetStorageItemsAsync();

                //if (items != null)
                //    e.AcceptedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;

                ICommand Command = GetOverCommand(sender as DependencyObject);
                if (Command != null)
                {
                    //ensure that command will be executed synchronus
                     Command.Execute(e);
                }

            }
            catch { }
            finally
            {
                d.Complete();
            }
        }

        #endregion

        private static void Dragablecontrol_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.RequestedOperation = DataPackageOperation.Move | DataPackageOperation.Link | DataPackageOperation.Copy;
        }

        private static async void Dragablecontrol_Drop(object sender, DragEventArgs e)
        {
            ICommand Command = GetDropCommand(sender as DependencyObject);

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
