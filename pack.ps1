$root = (split-path -parent $MyInvocation.MyCommand.Definition)
Write-Host  "$root\Core\bin\x64\Release\Get.the.solution.UWP.XAML.dll"
$version = [System.Reflection.Assembly]::LoadFile("$root\Core\bin\x64\Release\Get.the.solution.UWP.XAML.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

& $root\nuget.exe pack Get.the.solution.UWP.XAML.csproj.nuspec -properties version=$versionStr 


# & $root\nuget.exe pack $root\Get.the.solution.UWP.XAML.csproj.nuspec