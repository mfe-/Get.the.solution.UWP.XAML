// Ported from Silverlight Toolkit.
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.
//source: https://raw.githubusercontent.com/xyzzer/WinRTXamlToolkit/54f94a78ef108e7273ee52816cd4f464c293fc9a/WinRTXamlToolkit/Controls/DockPanel/DockPanel.cs
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Arranges child elements around the edges of the panel. Optionally, 
    /// last added child element can occupy the remaining space.
    /// </summary>
    public class DockPanel : Panel
    {
        /// <summary>
        /// Gets or sets a value that indicates the position of a child element within a parent <see cref="DockPanel"/>.
        /// </summary>
        public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached(
            "Dock",
            typeof(Dock),
            typeof(FrameworkElement),
            new PropertyMetadata(Dock.Left, DockChanged));

        /// <summary>
        /// Gets DockProperty attached property
        /// </summary>
        /// <param name="obj">Target FrameworkElement</param>
        /// <returns>Dock value</returns>
        public static Dock GetDock(FrameworkElement obj)
        {
            return (Dock)obj.GetValue(DockProperty);
        }

        /// <summary>
        /// Sets DockProperty attached property
        /// </summary>
        /// <param name="obj">Target FrameworkElement</param>
        /// <param name="value">Dock Value</param>
        public static void SetDock(FrameworkElement obj, Dock value)
        {
            obj.SetValue(DockProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="LastChildFill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastChildFillProperty
            = DependencyProperty.Register(
                nameof(LastChildFill),
                typeof(bool),
                typeof(DockPanel),
                new PropertyMetadata(true, LastChildFillChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the last child element within a DockPanel stretches to fill the remaining available space.
        /// </summary>
        public bool LastChildFill
        {
            get { return (bool)GetValue(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
        }

        /// <summary>
        /// Identifies the Padding dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Padding"/> dependency property.</returns>
        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(
                nameof(Padding),
                typeof(Thickness),
                typeof(DockPanel),
                new PropertyMetadata(default(Thickness), OnPaddingChanged));

        /// <summary>
        /// Gets or sets the distance between the border and its child object.
        /// </summary>
        /// <returns>
        /// The dimensions of the space between the border and its child as a Thickness value.
        /// Thickness is a structure that stores dimension values using pixel measures.
        /// </returns>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        private static void DockChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var dockPanel = senderElement?.FindParent<DockPanel>();

            dockPanel?.InvalidateArrange();
        }

        private static void LastChildFillChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dockPanel = (DockPanel)sender;
            dockPanel.InvalidateArrange();
        }

        private static void OnPaddingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var dockPanel = (DockPanel)sender;
            dockPanel.InvalidateMeasure();
        }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return finalSize;
            }

            var currentBounds = new Rect(Padding.Left, Padding.Top, finalSize.Width - Padding.Right, finalSize.Height - Padding.Bottom);
            var childrenCount = LastChildFill ? Children.Count - 1 : Children.Count;

            for (var index = 0; index < childrenCount; index++)
            {
                var child = Children[index];
                var dock = (Dock)child.GetValue(DockProperty);
                double width, height;
                switch (dock)
                {
                    case Dock.Left:

                        width = Math.Min(child.DesiredSize.Width, GetPositiveOrZero(currentBounds.Width - currentBounds.X));
                        child.Arrange(new Rect(currentBounds.X, currentBounds.Y, width, GetPositiveOrZero(currentBounds.Height - currentBounds.Y)));
                        currentBounds.X += width;

                        break;
                    case Dock.Top:

                        height = Math.Min(child.DesiredSize.Height, GetPositiveOrZero(currentBounds.Height - currentBounds.Y));
                        child.Arrange(new Rect(currentBounds.X, currentBounds.Y, GetPositiveOrZero(currentBounds.Width - currentBounds.X), height));
                        currentBounds.Y += height;

                        break;
                    case Dock.Right:

                        width = Math.Min(child.DesiredSize.Width, GetPositiveOrZero(currentBounds.Width - currentBounds.X));
                        child.Arrange(new Rect(GetPositiveOrZero(currentBounds.Width - width), currentBounds.Y, width, GetPositiveOrZero(currentBounds.Height - currentBounds.Y)));
                        currentBounds.Width -= (currentBounds.Width - width) > 0 ? width : 0;

                        break;
                    case Dock.Bottom:

                        height = Math.Min(child.DesiredSize.Height, GetPositiveOrZero(currentBounds.Height - currentBounds.Y));
                        child.Arrange(new Rect(currentBounds.X, GetPositiveOrZero(currentBounds.Height - height), GetPositiveOrZero(currentBounds.Width - currentBounds.X), height));
                        currentBounds.Height -= (currentBounds.Height - height) > 0 ? height : 0;

                        break;
                }
            }

            if (LastChildFill)
            {
                var width = GetPositiveOrZero(currentBounds.Width - currentBounds.X);
                var height = GetPositiveOrZero(currentBounds.Height - currentBounds.Y);
                var child = Children[Children.Count - 1];
                child.Arrange(
                    new Rect(currentBounds.X, currentBounds.Y, width, height));
            }

            return finalSize;
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size availableSize)
        {
            var parentWidth = 0.0;
            var parentHeight = 0.0;
            var accumulatedWidth = Padding.Left + Padding.Right;
            var accumulatedHeight = Padding.Top + Padding.Bottom;

            foreach (var child in Children)
            {
                var childConstraint = new Size(
                    GetPositiveOrZero(availableSize.Width - accumulatedWidth),
                    GetPositiveOrZero(availableSize.Height - accumulatedHeight));

                child.Measure(childConstraint);
                var childDesiredSize = child.DesiredSize;

                switch ((Dock)child.GetValue(DockProperty))
                {
                    case Dock.Left:
                    case Dock.Right:
                        parentHeight = Math.Max(parentHeight, accumulatedHeight + childDesiredSize.Height);
                        accumulatedWidth += childDesiredSize.Width;
                        break;

                    case Dock.Top:
                    case Dock.Bottom:
                        parentWidth = Math.Max(parentWidth, accumulatedWidth + childDesiredSize.Width);
                        accumulatedHeight += childDesiredSize.Height;
                        break;
                }
            }

            parentWidth = Math.Max(parentWidth, accumulatedWidth);
            parentHeight = Math.Max(parentHeight, accumulatedHeight);
            return new Size(parentWidth, parentHeight);
        }

        private static double GetPositiveOrZero(double value)
        {
            return Math.Max(value, 0);
        }

    }
}