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