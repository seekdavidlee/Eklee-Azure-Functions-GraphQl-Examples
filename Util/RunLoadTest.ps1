param(
	[Parameter(Mandatory = $True)][string]$Name,
	[Parameter(Mandatory = $True)][string]$ApiName,
	[Parameter(Mandatory = $True)][string]$TestFileName,
	[Parameter(Mandatory = $True)][string]$OutputReportDir
)

$ErrorActionPreference = "Stop"

if (!(Test-Path $OutputReportDir)) {
	New-Item -ItemType Directory -Force -Path $OutputReportDir
}

$currentDir = (Get-Location).Path

$buildConfig = "debug"
$projectName = $Name
Push-Location .\$Name
dotnet build --configuration=$buildConfig
Pop-Location

$workingDir = "$Name\$Name\bin\$buildConfig\netstandard2.1"

Push-Location $workingDir

$localSettings = '{
	"IsEncrypted": false,
	"Host": {
		"LocalHttpPort": 7072,
		"CORS": "*"
	},
	"Values": {
		"AzureWebJobsStorage": "UseDevelopmentStorage=true",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"Db:Name": "sampledb",
		"Db:Url": "https://localhost:8081",
		"Db:Key": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
		"Db:RequestUnits": "10000",
		"Storage:ConnectionString": "UseDevelopmentStorage=true"
	}
}'

Set-Content -Path .\local.settings.json -Value $localSettings
npm init --yes
npm install --save-dev azure-functions-core-tools@3
Start-Process -FilePath node_modules\.bin\func.cmd -ArgumentList "host start -p 7072"

Start-Sleep -s 10

$func = Get-Process -Name func

Pop-Location

Push-Location "Example.LoadTester\Example.LoadTester"

dotnet run -- -r "$currentDir\$projectName\LoadTest\$TestFileName" -g "http://localhost:7072/api/$ApiName" -o $OutputReportDir

Pop-Location

Stop-Process $func