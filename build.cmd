@echo Off
set config=%1
if "%config%" == "" (
	set config=Release
)

set version=1.0.0
if not "%PackageVersion%" == "" (
	set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=".nuget\nuget.exe"
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild proactima.jsonobject.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false /p:VisualStudioVersion=14.0

mkdir Build
mkdir Build\lib
mkdir Build\lib\net451
mkdir Build\lib\Portable-Win81+Wpa81

%nuget% pack ".\proactima.jsonobject.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
