# Introduction

Here, you will find some simple examples related to [Eklee.Azure.Functions.GraphQl](https://www.nuget.org/packages/Eklee.Azure.Functions.GraphQl). Please review the code comments for additional details in each of the Example.

## Structure of the Example

Each Example consists of a Solution which we can find a Project as well as a [Postman](https://www.getpostman.com/) test. There could be configuration requirements. Please note that in each Project. After setting up the Project, we should be able to run it.

# Creating a starter solution + project

You can also created a starter solution + project by running the following Powershell command:

```
.\GenSolution.ps1 -Name $nameOfProject
```

Remember to provide a name for your starter solution + project. There are two other parameters -OutputPath and -NugetSource which will allow you to specify where you want the solution and project to be and what nuget sources to use. This Powershell script is used internally to create the Examples, so please note for any new changes.