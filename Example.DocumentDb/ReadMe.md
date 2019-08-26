# Introduction

This example demostrates the use of the DocumentDb resolver for handling your Models. 

## Setup

Create a local.settings.json file with the following content to start it. Be sure to start the Azure CosmosDB Emulator first.

```
{
    "IsEncrypted": false,
	"Values": {
		"AzureWebJobsStorage": "",
		"FUNCTIONS_WORKER_RUNTIME": "dotnet",
		"Db:Name": "sampledb",
		"Db:Url": "https://localhost:8081",
		"Db:Key": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
		"Db:RequestUnits": "400"
	}
}
```