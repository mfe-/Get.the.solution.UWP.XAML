﻿using System;
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
            if (Children.Count > 0)
            {
                var cellWidth = size.Height / Children.Count;
                var childRect = new Rect(0, 0, cellWidth, cellWidth);
                foreach (var child in Children)
                {
                    var omg= VisualTreeHelper.GetChild(child, 0);
                    child.Arrange(childRect);
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
