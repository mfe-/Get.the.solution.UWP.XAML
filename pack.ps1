$root = (split-path -parent $MyInvocation.MyCommand.Definition)
Write-Host  "$root\Core\bin\x64\Release\Get.the.solution.UWP.XAML.dll"
$version = [System.Reflection.Assembly]::LoadFile("$root\Core\bin\x64\Release\Get.the.solution.UWP.XAML.dll").GetName().Version
Write-Host "extracted $version"
$commit = git rev-parse HEAD;
$xml = [xml](Get-Content("Get.the.solution.UWP.XAML.csproj.nuspec"))
$xml.package.metadata.repository.commit="$commit";
$xml.Save("Get.the.solution.UWP.XAML.csproj.nuspec")
& $root\nuget.exe pack Get.the.solution.UWP.XAML.csproj.nuspec -properties version=$version 


# & $root\nuget.exe pack $root\Get.the.solution.UWP.XAML.csproj.nuspec