# Changelog

## [2.0.0] - 2025-03-12
### Added
- Adds caching for the MongoClient connection to improve performance.
- Adds support for array filter for an update operation.
- Option to use upsert when updating documents.

### Updated
- Updated MongoDB.Driver to version 3.2.1

### Breaking changes
- The MongoDB driver drops support for MongoDB Server v3.6 and earlier.
- The MongoDB driver drops support for .NET Core 2.x and .NET Framework 4.6.
- Read more about MongoDB driver 3.0 breaking changes [here](https://mongodb.github.io/mongo-csharp-driver/3.0/reference/breaking_changes/)

## [1.0.1] - 2023-11-23
### Fixed
- Fixed dll error when importing the Task to Frends by adding local dll reference to the project file.

## [1.0.0] - 2022-11-01
### Added
- Initial implementation