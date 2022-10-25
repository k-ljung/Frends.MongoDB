# Frends.MongoDB.Insert
Frends task for MongoDB insert operation.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/FrendsPlatform/Frends.MongoDB/actions/workflows/Insert_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.MongoDB/actions)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.MongoDB.Insert)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.MongoDB/Frends.MongoDB.Insert|main)

# Installing

You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed https://www.myget.org/F/frends-tasks/api/v2.

## Building


Rebuild the project

`dotnet build`

Run tests
 
Run commands `docker compose pull mongo` in ..\..\..\Files

`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`