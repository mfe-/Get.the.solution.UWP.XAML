echo publishing
rem %1 script argument param[1]
set dec=%1
echo %dec%
nuget.exe push *.nupkg %dec% -src  https://api.nuget.org/v3/index.json                  