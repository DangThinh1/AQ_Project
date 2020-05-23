###########################################################
#
# Script to upgrade all NuGet packages in solution to last version
#
# USAGE
# Place this file (AutoUpdateNugetPackages.ps1) to your solution folder. 
# From Package Manager Console execute
#
# .\CMD\AutoUpdateNugetPackages.ps1
#
# Do not hestitate to contact me at any time
# toanbk.dev@gmail.com
#
#
##########################################################



$regex = 'PackageReference Include="([^"]*)" Version="([^"]*)"'

ForEach ($file in get-childitem . -recurse | where {$_.extension -like "*csproj"})
{
    $packages = Get-Content $file.FullName |
        select-string -pattern $regex -AllMatches | 
        ForEach-Object {$_.Matches} | 
        ForEach-Object {$_.Groups[1].Value.ToString()}| 
        sort -Unique

    ForEach ($package in $packages)
    {
        $fullName = $file.name
		#List Exclude package: "Microsoft.Extensions.DependencyInjection","packageid2","packageid3",...
		$excludePackage ="Microsoft.Extensions.DependencyInjection","Microsoft.AspNetCore.StaticFiles","Microsoft.AspNetCore.Session","Microsoft.Extensions.Configuration.Abstraction","Microsoft.AspNetCore.Authentication.Cookies","Microsoft.Extensions.Configuration","Microsoft.AspNetCore.DataProtection.EntityFrameworkCore","Microsoft.Extensions.Options","Microsoft.Extensions.Options.ConfigurationExtensions","Microsoft.EntityFrameworkCore.SqlServer","Microsoft.AspNetCore.Razor.Design","Microsoft.VisualStudio.Web.CodeGeneration.Design","Microsoft.VisualStudio.Web.BrowserLink","Microsoft.Extensions.Logging","Microsoft.AspNetCore.Authentication","System.IdentityModel.Tokens.Jwt","Microsoft.Extensions.Configuration","Microsoft.AspNetCore.Authentication.JwtBearer","Microsoft.AspNetCore.Identity","Microsoft.EntityFrameworkCore","Microsoft.AspNetCore.Hosting","Microsoft.AspNetCore.Builder","Microsoft.AspNetCore.Mvc","Microsoft.AspNetCore.Http", "SixLabors.ImageSharp","Microsoft.Extensions.Primitives","ExtendedUtility","Microsoft.AspNetCore.App"
		if($package -notcontains $excludePackage  )
		{
			if($package -ne "Microsoft.Extensions.Options" -And  $package -ne "Microsoft.Extensions.Options.ConfigurationExtensions" -And $package -ne "Microsoft.Extensions.Primitives"  -And $package -ne "Microsoft.Extensions.Configuration.Abstraction" -And $package -ne "Microsoft.Extensions.DependencyInjection" -And $package -ne "Microsoft.AspNetCore.Session" -And $package -ne "Microsoft.AspNetCore.StaticFiles" -And $package -ne "Microsoft.AspNetCore.Authentication" -And $package -ne "Microsoft.AspNetCore.Authentication.Cookies" -And $package -ne "Microsoft.Extensions.Configuration" -And $package -ne "Microsoft.AspNetCore.DataProtection.EntityFrameworkCore"  -And $package -ne "Microsoft.AspNetCore.Authentication.JwtBearer" -And $package -ne "Microsoft.AspNetCore.Razor.Design" -And $package -ne "Microsoft.VisualStudio.Web.BrowserLink" -And $package -ne "Microsoft.VisualStudio.Web.CodeGeneration.Design" -And $package -ne "Microsoft.AspNetCore.Razor.Design" -And $package -ne "Microsoft.EntityFrameworkCore.SqlServer" -And $package -ne "Microsoft.EntityFrameworkCore" -And $package -ne "Microsoft.Extensions.Logging" -And $package -ne "Microsoft.AspNetCore.Identity" -And $package -ne "SixLabors.ImageSharp" -And $package -ne "ExtendedUtility" -And $package -ne "Microsoft.AspNetCore.App")
			{
				write-host "Check update in project:$fullName Package: $package"  -foreground 'magenta'
				iex "dotnet add $fullName package $package"
			}
		}
    }
}



