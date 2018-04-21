nuget.exe restore
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Debug /property:Platform=ARM
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x64
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=x86
msbuild Core\Get.the.solution.UWP.XAML.csproj  /property:Configuration=Release /property:Platform=ARM
