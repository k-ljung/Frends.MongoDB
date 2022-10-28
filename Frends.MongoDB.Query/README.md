# Frends.MongoDB.Query
Frends Task for MongoDB query operation.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/FrendsPlatform/Frends.MongoDB/actions/workflows/Query_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.MongoDB/actions)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.MongoDB.Query)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.MongoDB/Frends.MongoDB.Query|main)

# Installing

You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed https://www.myget.org/F/frends-tasks/api/v2.

## Building


Rebuild the project

`dotnet build`

Run tests
 
Run commands `docker-compose up -d` in \Frends.MongoDB.Query.Tests\Files\

`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`