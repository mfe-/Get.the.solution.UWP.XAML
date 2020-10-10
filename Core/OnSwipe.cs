using System;
using System.Windows.Input;
using Windows.Devices.Input;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Get.the.solution.UWP.XAML
{
    public class OnSwipe
    {
        public static ICommand GetRaiseCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(RaiseCommandProperty);
        }
        public static void SetRaiseCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(RaiseCommandProperty, value);
        }
        // Using a DependencyProperty as the backing store for RaiseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaiseCommandProperty =
            DependencyProperty.RegisterAttached("RaiseCommand", typeof(ICommand), typeof(OnSwipe), new PropertyMetadata(null, OnRaiseCommandPropertyChanged));

        private static void OnRaiseCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement uIElement)
            {
                uIElement.AddHandler(UIElement.PointerEnteredEvent, new PointerEventHandler(Element_PointerEntered), true);
                uIElement.AddHandler(UIElement.PointerExitedEvent, new PointerEventHandler(Element_PointerReleased), true);
            }
        }
        public static bool PointerTypeSwipeAble(PointerDeviceType pointerDeviceType)
        {
            return pointerDeviceType == PointerDeviceType.Touch || pointerDeviceType == PointerDeviceType.Pen;
        }
        private static PointerPoint _StartinPoint;

        public OnSwipe()
        {
        }

        private static void Element_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && PointerTypeSwipeAble(e.Pointer.PointerDeviceType))
            {
                FrameworkElement Element = frameworkElement;
                _StartinPoint = e.GetCurrentPoint(Element);
            }
        }
        private static void Element_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //TODO: werte 100 und 500 in zusätzliche AP auslagern und besser das ganze über winkel abschätzen ob left /right down up usw
            bool Swipe = false;
            if (sender is FrameworkElement frameworkElement && PointerTypeSwipeAble(e.Pointer.PointerDeviceType))
            {
                var currentpoint = e.GetCurrentPoint(frameworkElement);

                VirtualKey Touch = VirtualKey.Left;
                //minimum 70 px difference of swipe required
                if (Math.Abs(currentpoint.Position.X - _StartinPoint.Position.X) >= 100)
                {

                    if (_StartinPoint.Position.X > currentpoint.Position.X)
                    {
                        Touch = VirtualKey.Left;
                    }
                    else
                    {
                        Touch = VirtualKey.Right;
                    }
                    Swipe = true;

                }
                else if (Math.Abs(currentpoint.Position.X - _StartinPoint.Position.X) < 30
                    && Math.Abs(currentpoint.Position.Y - _StartinPoint.Position.Y) <= 500)
                {
                    if (_StartinPoint.Position.Y < currentpoint.Position.Y)
                    {
                        Touch = VirtualKey.Down;
                    }
                    else
                    {
                        Touch = VirtualKey.Up;
                    }
                    Swipe = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("exit" + _StartinPoint.Position + " " + e.GetCurrentPoint((sender as FrameworkElement)).Position);
                }

                if (Swipe)
                {
                    ICommand Command = GetRaiseCommand(frameworkElement);
                    if (Command != null)
                    {
                        if (Command.CanExecute(Touch))
                        {
                            Command.Execute(Touch);
                        }
                    }
                }
            }
        }
    }
}
