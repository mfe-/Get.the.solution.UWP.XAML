using Windows.UI.Xaml;

namespace Get.the.solution.UWP.XAML
{
    /// <summary>
    /// StateTrigger for <seealso cref="DeviceFormFactorType"/>
    /// </summary>
    public class DeviceTypeStateTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty TargetDeviceFamilyProperty = DependencyProperty.Register(
            "TargetDeviceFamily", typeof(DeviceFormFactorType), typeof(DeviceTypeStateTrigger), new PropertyMetadata(default(DeviceFormFactorType), OnDeviceTypePropertyChanged));

        public DeviceFormFactorType TargetDeviceFamily
        {
            get { return (DeviceFormFactorType)GetValue(TargetDeviceFamilyProperty); }
            set { SetValue(TargetDeviceFamilyProperty, value); }
        }

        private static void OnDeviceTypePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            var trigger = (DeviceTypeStateTrigger)dependencyObject;
            var newTargetDeviceFamily = (DeviceFormFactorType)eventArgs.NewValue;
            trigger.SetActive(newTargetDeviceFamily == DeviceTypeHelper.GetDeviceFormFactorType());
        }
    }
}
