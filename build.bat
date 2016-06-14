@echo off

REM IF [%1]==[] GOTO MissingApiKey

reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath > nul 2>&1
if ERRORLEVEL 1 goto MissingMSBuildRegistry

for /f "skip=2 tokens=2,*" %%A in ('reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath') do SET MSBUILDDIR=%%B

IF NOT EXIST %MSBUILDDIR%nul goto MissingMSBuildToolsPath
IF NOT EXIST %MSBUILDDIR%msbuild.exe goto MissingMSBuildExe

::BUILD
"tools\nuget.exe" restore Azure.Insights.WebTests.Sdk.sln
"%MSBUILDDIR%msbuild.exe" Azure.Insights.WebTests.Sdk.sln /t:ReBuild /v:minimal /p:Configuration=Release;TargetFrameworkVersion=v4.5;DefineConstants="TRACE";OutPutPath=bin\Release\net45\

mkdir build
del "build\*.nupkg"

::SET API KEY
REM "tools\nuget.exe" setApiKey %1

::PACK
"tools\nuget.exe" pack "src\Insights.WebTests.Models\Insights.WebTests.Models.nuspec" -OutputDirectory build -Version 1.0.0.0
"tools\nuget.exe" pack "src\Insights.WebTests.Services\Insights.WebTests.Services.nuspec" -OutputDirectory build -Version 1.0.0.0

::DEPLOY
REM "tools\nuget.exe" push "build\*.nupkg"

goto:eof
::ERRORS
::---------------------
:MissingApiKey
echo API Key not found
goto:eof
:MissingMSBuildRegistry
echo Cannot obtain path to MSBuild tools from registry
goto:eof
:MissingMSBuildToolsPath
echo The MSBuild tools path from the registry '%MSBUILDDIR%' does not exist
goto:eof
:MissingMSBuildExe
echo The MSBuild executable could not be found at '%MSBUILDDIR%'
goto:eof