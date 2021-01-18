param(
	[Parameter(Mandatory = $True)][string]$OutputReportDir
)

Get-ChildItem -Path $OutputReportDir -Name | ForEach-Object {
	$name = $_
	$file = "$OutputReportDir\$name"

	$results = Get-Content $file | ConvertFrom-Json

	$TotalPayloadSizeInMb = $results.Summary.TotalPayloadSizeInMb
	$AverageTimeInSeconds = $results.Summary.AverageTimeInSeconds
	$TotalTimeMinutes = $results.Summary.TotalTimeMinutes

	Write-Host "TotalPayloadSizeInMb: $TotalPayloadSizeInMb"
	Write-Host "AverageTimeInSeconds: $AverageTimeInSeconds"
	Write-Host "TotalTimeMinutes: $TotalTimeMinutes"
}