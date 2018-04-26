
# Get.the.solution.UWP.XAML
UWP XAML Helper Stuff

[<img src="https://ci.appveyor.com/api/projects/status/7dx8yid04xyw8jc8?svg=true">](https://ci.appveyor.com/project/mfe-/get-the-solution-uwp-xaml)
[![NuGet](https://img.shields.io/nuget/v/Get.the.solution.UWP.XAML.svg)](https://www.nuget.org/packages/Get.the.solution.UWP.XAML/)

Default XAML Style Dictionary for
http://stackoverflow.com/questions/37336314/uwp-xaml-how-to-inherit-from-the-default-styles-using-basedon
http://stackoverflow.com/questions/38825696/style-inheritance-how-to-refine-a-custom-default-style?noredirect=1&lq=1 


Check for Platform/Device

if (DeviceTypeHelper.GetDeviceFormFactorType() != DeviceFormFactorType.Desktop)
{
	...
}
