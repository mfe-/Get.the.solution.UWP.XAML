
# Get.the.solution.UWP.XAML
UWP XAML Helper Stuff

### Install

The current nuget package target platform is 10.0.16299.0 (fall creators update)

[<img src="https://ci.appveyor.com/api/projects/status/7dx8yid04xyw8jc8?svg=true">](https://ci.appveyor.com/project/mfe-/get-the-solution-uwp-xaml)
[![NuGet](https://img.shields.io/nuget/v/Get.the.solution.UWP.XAML.svg)](https://www.nuget.org/packages/Get.the.solution.UWP.XAML/)

## Purpose of this lib

Collection of common UWP related helpers. If you have any uwp stuff, feel free to contribute!


# Samples
Select text on focus

        <TextBox Header="Sample for select all for textboxes" 
                 Text="sample text"
                 common:OnFocus.SelectAll="True" />
		 
Execute command when user presses CTRL + C 	

        common:OnKeyDown.Command="{Binding Path=CtrlOpenCommand}"
        common:OnKeyDown.Ctrl="True"
        common:OnKeyDown.OnKey="C"
        common:OnKeyDown.CommandParameter="foo"

MVVM Helper for opening context menu

        <Interactivity:Interaction.Behaviors>
                <Core:EventTriggerBehavior EventName="RightTapped">
                        <common:OpenMenuFlyoutAction />
                </Core:EventTriggerBehavior>
        </Interactivity:Interaction.Behaviors>
	
# Old todo links

Default XAML Style Dictionary for
http://stackoverflow.com/questions/37336314/uwp-xaml-how-to-inherit-from-the-default-styles-using-basedon
http://stackoverflow.com/questions/38825696/style-inheritance-how-to-refine-a-custom-default-style?noredirect=1&lq=1 


Check for Platform/Device

if (DeviceTypeHelper.GetDeviceFormFactorType() != DeviceFormFactorType.Desktop)
{
	...
}
