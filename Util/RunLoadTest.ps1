param(
	[Parameter(Mandatory = $True)][string]$Name,
	[Parameter(Mandatory = $True)][string]$ApiName,
	[Parameter(Mandatory = $True)][string]$TestFileName,
	[Parameter(Mandatory = $True)][string]$OutputReportDir
)

$currentDir = (Get-Location).Path

$buildConfig = "debug"
$projectName = $Name
Push-Location .\$Name
dotnet build --configuration=$buildConfig
Pop-Location

$workingDir = "$Name\$Name\bin\$buildConfig\netstandard2.0"

Push-Location $workingDir
npm install --save-dev azure-functions-core-tools@3

Start-Process -FilePath node_modules\.bin\func -ArgumentList "host start -p 7072"

Start-Sleep -s 10

$func = Get-Process -Name func

Pop-Location

Push-Location "Example.LoadTester\Example.LoadTester"

dotnet run -- -r "$currentDir\$projectName\LoadTest\$TestFileName" -g "http://localhost:7072/api/$ApiName" -o $OutputReportDir

Pop-Location

Stop-Process $func

