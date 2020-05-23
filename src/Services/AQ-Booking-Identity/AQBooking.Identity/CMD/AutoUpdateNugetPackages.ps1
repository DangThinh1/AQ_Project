###########################################################
#
# Script to upgrade all NuGet packages in solution to last version
#
# USAGE
# Place this file (Upgrade-Package.ps1) to your solution folder. 
# From Package Manager Console execute
#
# .\CMD\AutoUpdateNugetPackages.ps1
#
# Do not hestitate to contact me at any time
# toanbk.dev@gmail.com
#
#
##########################################################



$regex = [regex] 'PackageReference Include="([^"]*)" Version="([^"]*)"'
ForEach ($file in get-childitem . -recurse | where {$_.extension -like "*csproj"})
{
  $proj = $file.name
  $content = Get-Content $proj
  $match = $regex.Match($content)
  if ($match.Success) {
	$name = $match.Groups[1].Value
    $version = $match.Groups[2].Value
    if ($version -notin "-") {
		write-host "Check update in project:[$proj] [Package]: $name"  -foreground 'magenta'
		iex "dotnet add $proj package $name"
    }
  }
}



