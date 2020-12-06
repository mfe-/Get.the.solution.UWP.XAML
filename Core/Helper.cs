using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Provides additional methods like <see cref="Windows.UI.Xaml.Media.VisualTreeHelper"/>
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Register an event dynamically by creating the required add and remove functions for <seealso cref="WindowsRuntimeMarshal.AddEventHandler{T}(Func{T, EventRegistrationToken}, Action{EventRegistrationToken}, T)"/> and <seealso cref="WindowsRuntimeMarshal.RemoveEventHandler{T}(Action{EventRegistrationToken}, T)"/>
        /// </summary>
        /// <param name="sourceInstance">for example textbox</param>
        /// <param name="eventname">for example KeyDown</param>
        /// <param name="methodToFireOnEvent">the function which should be fired when the proper event occours</param>
        /// <returns></returns>
        public static Tuple<Func<object, EventRegistrationToken>, Action<EventRegistrationToken>, Action<object, object>> RegisterEventDynamically(UIElement sourceInstance, string eventname, Action<object, object> methodToFireOnEvent, bool registerEvent = false)
        {
            UIElement AssociatedObject = sourceInstance;
            EventInfo eventInfo = sourceInstance.GetType().GetRuntimeEvent(eventname);

            MethodInfo addMethod = eventInfo.AddMethod;
            MethodInfo removeMethod = eventInfo.RemoveMethod;
            //contains the handler as parameter
            ParameterInfo[] addParameters = addMethod.GetParameters();
            //get the delegatetype of the event
            Type delegateType = addParameters[0].ParameterType;
            Action<object, object> handler = (s, e) => methodToFireOnEvent(s, e);
            MethodInfo handlerInvoke = typeof(Action<object, object>).GetRuntimeMethod("Invoke", new[] { typeof(object), typeof(object) });
            Delegate @delegate = handlerInvoke.CreateDelegate(delegateType, handler);

            EventRegistrationToken add(object a) => (EventRegistrationToken)addMethod.Invoke(AssociatedObject, new object[] { @delegate });
            void remove(EventRegistrationToken t) => removeMethod.Invoke(AssociatedObject, new object[] { t });

            if (registerEvent)
            {
                WindowsRuntimeMarshal.AddEventHandler(add, remove, handler);

            }

            return new Tuple<Func<object, EventRegistrationToken>, Action<EventRegistrationToken>, Action<object, object>>(add, remove, handler);
        }

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
