param(
	[Parameter(Mandatory = $True)][string]$OutputReportDir
)

Get-ChildItem -Path "$OutputReportDir\loadtest-reports" -File -Name | ForEach-Object {
	$name = $_

    Write-Host "Processing $name"

	$results = Get-Content "$OutputReportDir\loadtest-reports\$name" | ConvertFrom-Json

	$TotalPayloadSizeInMb = $results.Summary.TotalPayloadSizeInMb
	$AverageTimeInSeconds = $results.Summary.AverageTimeInSeconds
	$TotalTimeMinutes = $results.Summary.TotalTimeMinutes

	Write-Host "TotalPayloadSizeInMb: $TotalPayloadSizeInMb"
	Write-Host "AverageTimeInSeconds: $AverageTimeInSeconds"
	Write-Host "TotalTimeMinutes: $TotalTimeMinutes"
}