Param(
	[string] [Parameter(Mandatory=$true)] $Version,
	[string] [Parameter(Mandatory=$false)] $BuildPath
)

$nuget = ".\tools\nuget.exe"
$projects = Get-ChildItem .\src | ?{ $_.PSIsContainer }
foreach($project in $projects)
{
	$projectPath = $project.FullName;
	$projectName = $project.Name;
	
	$nuspec = $projectName.nuspec
	$isPackage = Test-Path -Path Join-Path($projectPath, $nuspec)
	if ($isPackage -ne $true)
	{
		continue
	}

	if ([string]::IsNullOrWhiteSpace($BuildPath))
	{
		& $nuget pack $projectPath\$nuspec -Version $Version
	}
	else
	{
		& $nuget pack $projectPath\$nuspec -OutputDirectory $buildPath -Version $Version
	}
}
