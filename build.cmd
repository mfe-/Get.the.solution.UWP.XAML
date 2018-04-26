nuget.exe restore
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=ARM
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=ARM
mkdir Core\bin\Release

copy Core\bin\x86\Release\Get.the.solution.UWP.XAML.dll Core\bin\Release

copy Core\bin\x86\Release\Get.the.solution.UWP.XAML.pri Core\bin\Release

copy Core\bin\x86\Release\Get.the.solution.UWP.XAML.xr.xml Core\bin\Release


rem "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars32.bat"


corflags.exe /32bitreq- Core\bin\Release\Get.the.solution.UWP.XAML.dll