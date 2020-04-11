param(
    [switch]$ExampleProject,
    [Parameter(Mandatory = $True)][string]$Name,
    [Parameter(Mandatory = $False)][string]$OutputPath,    
    [Parameter(Mandatory = $False)][string]$NugetSource
)

if (!$ExampleProject -and $Name.Contains(".")) {
    Write-Warning "Project name cannot contain period. Please replace with dash."
    return
}

$projectName = $Name

$response = Invoke-RestMethod -Uri "https://functionscdn.azureedge.net/public/cli-feed-v3.json"
$release = $response.tags.v2.release
$result = $response.releases.$release

$wd = ".\.wd"
if ((Test-Path -Path $wd) -eq $false) {
    New-Item -ItemType Directory -Force -Path $wd
}

$projectNupkgPath = "$wd\$release.nupkg"

if ((Test-Path -Path $projectNupkgPath) -eq $false) {
    Write-Host "Downloading file $($result.projectTemplates)"
    Invoke-WebRequest -Uri $result.projectTemplates -OutFile $projectNupkgPath
}
else {
    Write-Host "Found $projectNupkgPath"
}

dotnet new -i $projectNupkgPath

$templateNupkgPath = "$wd\$release-template.nupkg"

if ((Test-Path -Path $templateNupkgPath) -eq $false) {
    Write-Host "Downloading file $($result.itemTemplates)"
    Invoke-WebRequest -Uri $result.itemTemplates -OutFile $templateNupkgPath
}
else {
    Write-Host "Found $templateNupkgPath"
}
Write-Host "Installing $templateNupkgPath"
dotnet new -i $templateNupkgPath

if (!$OutputPath -Or $ExampleProject) {
    $projectPath = ".\$projectName"
}
else {
    $OutputPath = $OutputPath.TrimEnd("\")
    $projectPath = "$OutputPath\$projectName"
}

if ((Test-Path -Path $projectPath) -eq $false) {

    Write-Host "ProjectPath: $projectPath"

    New-Item -ItemType Directory $projectPath

    # Store current path.
    $currentDir = (Get-Location).Path

    Push-Location $projectPath
    if (!$ExampleProject) {
        $apiProjectName = $Name + "-Api"
    }
    else {
        $apiProjectName = $Name 
    }
    dotnet new sln --name $apiProjectName
    New-Item -ItemType Directory -Force $apiProjectName
    Push-Location $apiProjectName
    Write-Host "Creating a new Azure Function: $apiProjectName"
    dotnet new "Azure Functions" -n $apiProjectName -lang "C#"

    Write-Host "Creating a new Azure Function Trigger."
    dotnet new "http" -n GraphQLFunction -lang "C#"

    if (!$NugetSource) {
        $NugetSource = "https://api.nuget.org/v3/index.json"
    }
    
    dotnet add package Eklee.Azure.Functions.GraphQl -s $NugetSource
    dotnet add package Microsoft.NET.Sdk.Functions
    dotnet add package Microsoft.Azure.WebJobs.Extensions.Http
    dotnet add package Microsoft.Extensions.Caching.Memory
    
    Copy-Item $currentDir\Templates\* -Destination .\ -Recurse -Force

    $allFiles = Get-ChildItem -Path .\*.cs -Recurse -Force

    $allFiles | ForEach-Object {        
        $path = $_.Directory.FullName + "\" + $_.Name
        $refProjectName = $apiProjectName.Replace("-", ".")
        $content = Get-Content -Path $path
        $content = $content.Replace("namespace Example", "namespace $refProjectName")
        $content = $content.Replace("using Example.", "using $refProjectName.")
        $content
        Set-Content $content -Path $path
    }

    $projectFullName = "$apiProjectName.csproj"
    $projContent = Get-Content -Path .\$projectFullName
    $projContent = $projContent.Replace("<TargetFramework>netcoreapp2.1</TargetFramework>", "<TargetFramework>netstandard2.0</TargetFramework>")
    $projContent = $projContent.Replace("<AzureFunctionsVersion>V2</AzureFunctionsVersion>", "<AzureFunctionsVersion>v3</AzureFunctionsVersion>")
    Set-Content $projContent -Path .\$projectFullName

    Pop-Location
    dotnet sln "$apiProjectName.sln" add "$apiProjectName\$projectFullName"

    if (!$ExampleProject) {
        $appProjectName = $Name + "-App"
        npm install @angular/cli
        node_modules\.bin\ng new $appProjectName --defaults=true    
    }

    Pop-Location
}