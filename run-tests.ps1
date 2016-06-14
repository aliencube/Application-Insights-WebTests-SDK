Param(
    [string] [Parameter(Mandatory=$true)] $Configuration
)

$xunit = ".\packages\xunit.runner.console.2.1.0\tools\xunit.console.exe"
$projects = Get-ChildItem .\test | ?{ $_.PSIsContainer }
foreach($project in $projects)
{
    $projectPath = $project.FullName;
    $projectName = $project.Name;

    & $xunit $projectPath\bin\$Configuration\net45\Aliencube.Azure.$projectName.dll -appveyor
}
