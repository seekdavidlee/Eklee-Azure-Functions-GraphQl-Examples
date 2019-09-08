# Introduction

This example demostrates the use of the storage resolver for handling your Models. 

## Setup

Create a local.settings.json file with the following content to start it.

```
{
    "IsEncrypted": false,
	"Values": {
		"AzureWebJobsStorage": "",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"Storage:ConnectionString": "UseDevelopmentStorage=true"
	}
}
```

## Load test

Use the following command to run a load test from the root directory.

```
.\Util\RunLoadTest.ps1 -Name Example.DocumentDb -ApiName testdocumentdb -TestFileName ProductsOnly2.json -OutputReportDir C:\dev\loadtest-reports
```