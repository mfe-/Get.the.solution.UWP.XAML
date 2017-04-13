using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Calculates the size of the childs automatically
    /// </summary>
    public class ItemsPanel : Panel
    {
        protected override Size ArrangeOverride(Size availableSize)
        {
            Size size = base.ArrangeOverride(availableSize);
            Rect childRect = Rect.Empty;
            if (Children.Count > 0)
            {
                double cellWidth = 0;
                if (Orientation.Equals(Orientation.Horizontal))
                {
                    cellWidth = size.Width / Children.Count;
                    childRect = new Rect(0, 0, cellWidth, size.Height);
                }
                else
                {
                    cellWidth = size.Height / Children.Count;
                    childRect = new Rect(0, 0, size.Width, cellWidth);
                }
                foreach (UIElement Child in Children)
                {
                    Child.Arrange(childRect);
                    if (Orientation.Equals(Orientation.Horizontal))
                    {
                        childRect.X += cellWidth;
                    }
                    else
                    {
                        childRect.Y += cellWidth;
                    }
                }

            }
            return size;
        }



        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ItemsPanel), new PropertyMetadata(Orientation.Vertical));


    }
}
