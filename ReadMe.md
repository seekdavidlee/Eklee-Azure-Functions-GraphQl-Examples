# Introduction

Here, you will find some examples related to the usage of [Eklee.Azure.Functions.GraphQl](https://www.nuget.org/packages/Eklee.Azure.Functions.GraphQl).

## Structure of the Example

Each Example consists of:

* Solution for launching with Visual Studio.
* Project containing the code for your GraphQL API. 
* [Postman](https://www.getpostman.com/) test.
* ReadMe.md

Please review the ReadMe for additional details such as configuration setup requirements. After setting up the Project, we will be able to run it.


## Testing the Example

We can use the built-in testing framework which uses newman to test the Example against the Postman test collection. Use the following Powershell command:

```
.\Util\RunTest.ps1 -Name $nameOfProject
```

# Creating a starter solution + project

We can also create a starter solution + project by running the following Powershell command. Note that this Powershell script is also used internally to create a new example, so please note for any new changes. We will also create a starter Angular App.

```
.\GenSolution.ps1 -Name $nameOfProject
```

Remember to provide a name for the starter solution + project. There are two other optional parameters -OutputPath and -NugetSource.

* The OutputPath parameter will allow us to specify where we want the solution and project to be created.
* The NugetSource specify which nuget source to use for adding the Eklee.Azure.Functions.GraphQl nuget package.

## To create a example project for use in this repo, add the ExampleProject switch.

```
.\GenSolution.ps1 -Name $nameOfProject -ExampleProject
```

# Load Testing

We have an internal testing tool that will perform load testing on the examples provided. It is in the following syntax. Be sure the load testng manifest file is present. Refer to each example for load test script.

```
.\Util\RunLoadTest.ps1 -Name <Folder of the example> -ApiName <API endpoint> -TestFileName <Test file> -OutputReportDir <Output directory>
```