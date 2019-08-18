param(
    [Parameter(Mandatory = $True)][string]$Name
)

$currentDir = (Get-Location).Path

$buildConfig = "debug"
$projectName = $Name
Push-Location .\$Name
dotnet build --configuration=$buildConfig
Pop-Location

$workingDir = "$Name\$Name\bin\$buildConfig\netstandard2.0"

Push-Location $workingDir
npm install --save-dev azure-functions-core-tools
npm install --save-dev newman

Start-Process -FilePath node_modules\.bin\func -ArgumentList "host start"

Start-Sleep -s 10

$func = Get-Process -Name func

$reportFileName = (Get-Date).ToString("yyyyMMddHHmmss") + ".json"
$reportFilePath = "$currentDir\.reports\$reportFileName"
New-Item -ItemType Directory -Force -Path "$currentDir\.reports"

node_modules\.bin\newman run ..\..\..\..\PostmanTests\$projectName.postman_collection.json --reporters cli,json --reporter-json-export $reportFilePath

$report = (Get-Content $reportFilePath | Out-String | ConvertFrom-Json)	

$failures = $report.run.failures.length
Write-Host "Failures: $failures"
Write-Host "Killing job"
Stop-Process $func

if ($failures -gt 0) {
	Write-Host "Failed!" -ForegroundColor red
}else {
	Write-Host "Success!" -ForegroundColor green
}

Pop-Location