using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// Shows a <see cref="FlyoutBase"/>
    /// <code>
    /// <Interactivity:Interaction.Behaviors>
    ///     <Core:EventTriggerBehavior EventName="RightTapped">
    ///         <common:OpenMenuFlyoutAction />
    ///     </Core:EventTriggerBehavior>
    ///  </Interactivity:Interaction.Behaviors>
    /// </code>
    /// </summary>
    public class OpenMenuFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            if(flyoutBase!=null)
            {
               flyoutBase.ShowAt(senderElement);
            }

            return null;
        }
    }
}
