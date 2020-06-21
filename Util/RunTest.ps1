param(
    [Parameter(Mandatory = $True)][string]$Name
)

$ErrorActionPreference = "Stop"

$currentDir = (Get-Location).Path

$buildConfig = "debug"
$projectName = $Name
Push-Location .\$Name
dotnet build --configuration=$buildConfig
Pop-Location

$workingDir = "$Name\$Name\bin\$buildConfig\netstandard2.0"

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
npm install --save-dev newman

Start-Process -FilePath node_modules\.bin\func.cmd -ArgumentList "host start -p 7071"

Start-Sleep -s 10

$func = Get-Process -Name func

$reportFileName = (Get-Date).ToString("yyyyMMddHHmmss") + ".json"
$reportFilePath = "$currentDir\.reports\$reportFileName"
New-Item -ItemType Directory -Force -Path "$currentDir\.reports"

node_modules\.bin\newman.cmd run ..\..\..\..\PostmanTests\$projectName.postman_collection.json --reporters cli,json --reporter-json-export $reportFilePath

$report = (Get-Content $reportFilePath | Out-String | ConvertFrom-Json)	

$failures = $report.run.failures.length
Write-Host "Failures: $failures"
Write-Host "Killing job"
Stop-Process $func

if ($failures -gt 0) {
	Write-Host "Failed!" -ForegroundColor red
} else {
	Write-Host "Success!" -ForegroundColor green
}

Pop-Location