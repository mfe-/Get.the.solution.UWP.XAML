rmdir /S /Q "Core/bin"
rmdir /S /Q "Core/obj"
nuget.exe restore
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=ARM
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=ARM
mkdir Core\bin\Release
rem mkdir Core\bin\Debug


copy /Y Core\bin\x86\Release\Get.the.solution.UWP.XAML.dll Core\bin\Release
rem copy /Y Core\bin\x86\Debug\Get.the.solution.UWP.XAML.pdb Core\bin\Debug

copy /Y Core\bin\x86\Release\Get.the.solution.UWP.XAML.pri Core\bin\Release

copy /Y Core\bin\x86\Release\Get.the.solution.UWP.XAML\Get.the.solution.UWP.XAML.xr.xml Core\bin\Release


rem "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars32.bat"

rem https://docs.microsoft.com/en-us/nuget/guides/create-uwp-packages


corflags.exe /32bitreq- Core\bin\Release\Get.the.solution.UWP.XAML.dll

Powershell -File pack.ps1