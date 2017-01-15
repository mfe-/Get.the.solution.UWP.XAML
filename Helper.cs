using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML
{
    public static class Helper
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
        public static IList<TControl> FindParents<TControl>(this DependencyObject child) where TControl : DependencyObject
        {
            return FindParents<TControl>(child, new List<TControl>());
        }
        private static IList<TControl> FindParents<TControl>(DependencyObject child, IList<TControl> parentList) where TControl : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return parentList;

            //check if the parent matches the type we're looking for
            TControl parent = parentObject as TControl;
            if (parent != null)
            {
                parentList.Add(parent);
                parentList = FindParents<TControl>(parent, parentList);
            }

            return parentList;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        static IEnumerable<PropertyInfo> GetDependencyProperties(this Type type)
        {
            List<PropertyInfo> PropertyInfoList = new List<PropertyInfo>();
            while (type != typeof(DependencyObject))
            {
                List<PropertyInfo> Result = type.GetProperties(BindingFlags.Public | BindingFlags.Static).
                    Where(p => typeof(DependencyProperty).IsAssignableFrom(p.PropertyType)).ToList();

                PropertyInfoList.AddRange(Result);

                type = type.GetTypeInfo().BaseType;
            }
            return PropertyInfoList;
        }
        public static IEnumerable<BindingExpression> GetBindingExpressions(this FrameworkElement element)
        {
            IEnumerable<PropertyInfo> infos = element.GetType().GetDependencyProperties();
            foreach (PropertyInfo field in infos)
            {
                DependencyProperty dp = (DependencyProperty)field.GetValue(element);
                BindingExpression ex = element.GetBindingExpression(dp);
                if (ex != null)
                {
                    yield return ex;
                    System.Diagnostics.Debug.WriteLine("Binding found with path: “ +ex.ParentBinding.Path.Path");
                }
            }
        }

    }
}
