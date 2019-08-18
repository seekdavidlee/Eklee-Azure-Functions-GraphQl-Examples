param(
    [Parameter(Mandatory = $True)][string]$Name,
    [Parameter(Mandatory = $False)][string]$OutputPath,    
    [Parameter(Mandatory = $False)][string]$NugetSource
)

$projectName = $Name
$response = Invoke-RestMethod -Uri "https://functionscdn.azureedge.net/public/cli-feed-v3.json"
$release = $response.tags.v2.release
$result = $response.releases.$release

$wd = ".\.wd"
if ((Test-Path -Path $wd) -eq $false) {
    New-Item -ItemType Directory -Force -Path $wd
}

$nupkgPath = "$wd\$release.nupkg"

if ((Test-Path -Path $nupkgPath) -eq $false) {
    Write-Host "Downloading file $($result.projectTemplates)"
    Invoke-WebRequest -Uri $result.projectTemplates -OutFile $nupkgPath
}

if (!$OutputPath) {
    $projectPath = ".\$projectName"
}
else {
    $projectPath = $OutputPath
}

if ((Test-Path -Path $projectPath ) -eq $false) {

    Write-Host "ProjectPath: $projectPath"
    New-Item -ItemType Directory -Force -Path $projectPath

    Push-Location $projectPath
    dotnet new sln --name $projectName
    New-Item -ItemType Directory -Force $projectName
    Push-Location $projectName
    dotnet new "Azure Functions" -n $projectName -lang "C#"
    dotnet new "http" -n GraphQLFunction -lang "C#"

    if (!$NugetSource) {
        $NugetSource = "https://api.nuget.org/v3/index.json"
    }
    
    dotnet add package Eklee.Azure.Functions.GraphQl -s $NugetSource
	dotnet add package Microsoft.Extensions.Caching.Memory
    
    Copy-Item ..\..\Templates\* -Destination .\ -Recurse -Force

    $allFiles = Get-ChildItem -Path .\*.cs -Recurse -Force

    $allFiles | ForEach-Object {        
        $path = $_.Directory.FullName + "\" + $_.Name
        $content = Get-Content -Path $path
        $content = $content.Replace("namespace Example", "namespace $projectName")
		$content = $content.Replace("using Example.", "using $projectName.")
        $content
		Set-Content $content -Path $path
    }

    $projectFullName = "$projectName.csproj"
    $projContent = Get-Content -Path .\$projectFullName
	$projContent = $projContent.Replace("<TargetFramework>netcoreapp2.1</TargetFramework>","<TargetFramework>netstandard2.0</TargetFramework>")
    $projContent = $projContent.Replace("<AzureFunctionsVersion></AzureFunctionsVersion>","<AzureFunctionsVersion>v2</AzureFunctionsVersion>")
    Set-Content $projContent -Path .\$projectFullName

    Pop-Location
    dotnet sln "$projectName.sln" add "$projectName\$projectFullName"
    Pop-Location
}