param(
    [switch]$ExampleProject,
    [Parameter(Mandatory = $True)][string]$Name,
    [Parameter(Mandatory = $False)][string]$ExampleType, 
    [Parameter(Mandatory = $False)][string]$OutputPath,    
    [Parameter(Mandatory = $False)][string]$NugetSource,
    [Switch]$addBulma
)

$ErrorActionPreference = 'Stop'

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
    
    if ($ExampleType) {
        $ExampleSourcePath = "$currentDir\Example.$ExampleType\Example.$ExampleType"
        New-Item -ItemType Directory .\Core
        New-Item -ItemType Directory .\Models
        Copy-Item $ExampleSourcePath\*.cs -Destination .\ -Recurse -Force
        Copy-Item $ExampleSourcePath\Core\*.cs -Destination .\Core -Recurse -Force
        Copy-Item $ExampleSourcePath\Models\*.cs -Destination .\Models -Recurse -Force
    }
    else {
        Copy-Item $currentDir\Templates\Backend\* -Destination .\ -Recurse -Force
    }

    $allFiles = Get-ChildItem -Path .\*.cs -Recurse -Force
    
    $refProjectName = $apiProjectName.Replace("-", ".")

    $allFiles | ForEach-Object {        
        $path = $_.Directory.FullName + "\" + $_.Name

        $content = Get-Content -Path $path

        if ($ExampleType) {
            $content = $content.Replace("namespace Example.$ExampleType", "namespace $refProjectName")
            $content = $content.Replace("using Example.$ExampleType.", "using $refProjectName.")
        }
        else {
            $content = $content.Replace("namespace Example", "namespace $refProjectName")
            $content = $content.Replace("using Example.", "using $refProjectName.")
        }

        $content
        Set-Content $content -Path $path
    }

    $projectFullName = "$apiProjectName.csproj"
    $projContent = Get-Content -Path .\$projectFullName
    $projContent = $projContent.Replace("<TargetFramework>netcoreapp2.1</TargetFramework>", "<TargetFramework>netstandard2.0</TargetFramework>")
    $projContent = $projContent.Replace("<AzureFunctionsVersion>V2</AzureFunctionsVersion>", "<AzureFunctionsVersion>v3</AzureFunctionsVersion>")
    Set-Content $projContent -Path .\$projectFullName
    
    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine('"FUNCTIONS_WORKER_RUNTIME": "dotnet",')
    [void]$sb.AppendLine('"Db:Name": "sampledb",')
    [void]$sb.AppendLine('"Db:Url": "https://localhost:8081",')
    [void]$sb.AppendLine('"Db:Key": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",')
    [void]$sb.AppendLine('"Db:RequestUnits": "10000"')
    
    $localStettingsContent = Get-Content -Path .\local.settings.json
    $localStettingsContent = $localStettingsContent.Replace('"FUNCTIONS_WORKER_RUNTIME": "dotnet"', $sb.ToString())

    $sbHost = [System.Text.StringBuilder]::new()
    [void]$sbHost.AppendLine('IsEncrypted": false,')
    [void]$sbHost.AppendLine('"Host": {')
    [void]$sbHost.AppendLine('"LocalHttpPort": 7071,')
    [void]$sbHost.AppendLine('"CORS": "*" },')

    $localStettingsContent = $localStettingsContent.Replace('IsEncrypted": false,', $sbHost.ToString())

    $localStettingsContent

    Set-Content $localStettingsContent -Path .\local.settings.json

    Pop-Location
    dotnet sln "$apiProjectName.sln" add "$apiProjectName\$projectFullName"

    dotnet build "$apiProjectName.sln"

    if (!$ExampleProject) {
        $appProjectName = $Name + "-App"
        npm init --yes
        npm install @angular/cli
        node_modules\.bin\ng new $appProjectName --defaults=true
        Push-Location $appProjectName
        npm install graphql
        npm install @graphql-codegen/cli
        npm install @graphql-codegen/typescript
        npm install @graphql-codegen/typescript-operations
        npm install microsoft-adal-angular6

        if ($addBulma) {
            npm install bulma
        }
        
        npm install --save-dev azure-functions-core-tools@3
        npm install --save-dev newman

        Copy-Item $currentDir\Templates\Frontend\* -Destination .\ -Recurse -Force

        if ($addBulma) {
            $angularContent = Get-Content -Path .\angular.json

            $sbBulma = [System.Text.StringBuilder]::new()
            [void]$sbBulma.AppendLine('"src/styles.css",')
            [void]$sbBulma.AppendLine('"node_modules/bulma/css/bulma.min.css"')

            $angularContent = $angularContent.Replace('"src/styles.css"', $sbBulma.ToString())

            Set-Content $angularContent -Path .\angular.json
        }
      
        Pop-Location
    }

    Write-Host "Your new project is located: $projectPath"
    Write-Host "Run the following command to generate your typescript files based on GraphQL schema: npx graphql-codegen --config ./codegen.yml"
    Pop-Location
}