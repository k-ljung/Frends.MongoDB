# Frends.MongoDB.Index
Frends Task for MongoDB index operation.

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/FrendsPlatform/Frends.MongoDB/actions/workflows/Index_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.MongoDB/actions)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.MongoDB.Index)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.MongoDB/Frends.MongoDB.Index|main)

# Installing

You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed https://www.myget.org/F/frends-tasks/api/v2.

## Building


Rebuild the project

`dotnet build`

Run tests
 
Run command `docker-compose up -d` in \Frends.MongoDB.Index.Tests\Files\

`dotnet test`


Create a NuGet package

`dotnet pack --configuration Release`