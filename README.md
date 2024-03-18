# OGA.InfraBase
Base Infrastructure Class Library, providing persistence to domain model

## Description
This library provides classes to facilitate EF persistence, pagination support for mapping model to DTO, and dbcontext-backed configuration storage.\
Specifically, this library was put together to be referenced by your app-specific domain service layer, making persistence and DTO mapping easier.

This library includes the following classes and elements that can be consumed:
* Pagination classes for domain queries that require server-side pagination.\
  These classes support pagination needs for Next/Last URLs, and entity mapping to DTO types, using AutoMapper.
* A Uri Service implementation that can compose URL strings for pagination, and any other sort/filtering passed as query-parameter.
* A Config Service for process configuration mapping key-value type access into a backing table.\
  It includes both In-Memory and DbContext support, with a common interface, so you can easily perform unit-testing without a backend.
* A Config Update Service that provides runtime R/W access to application settings and build data stored in a process' appsettings.json file.\
  This can be used by diagnostic API calls to retrieve version, source control info, etc, for the process.\
  This library leverages the functionality of OGA.AppSettings.Writeable, to provide runtime write access to appsettings.json.
* A generic Repository class, providing the majority of access and update methods to entity types that derive from IAggregateRoot<>.\
  This class type can be used by generic API controllers, for domain model access without entity-specific code.
* A data context base that is able to set creation and modified timestamps of saved entities, and correctly retrieve datetimes from a database backend, as UTC (when stored as UCT).
* A data context extension that bolts on key-value storage capability.
* A data context extension that bolts on an ability to determine if all migrations are applied, and to retrieve a list of pending migrations, waiting to update storage.
* A data context attribute that marks storage-specific data contexts with their specific storage type: MSSQL, PostGres, InMemory.\
  This allows us to retrieve derived data contexts from a simple process assembly search, and apply the correct access config (host/user/pw) for the specific storage provider.
* A DataTime UTC Value Converter that sets the UTC Kind flag of a given retrieved DateTime.\
  This value converter can be quickly used in property-tablecolumn mappings of an IEntityTypeConfiguration<> implementation.\
  This is especially used for any datetime stored in MSSQL, because the SQL Server storage provider does not set the UTC flag of retrieved DateTimes, if they were stored in UTC.

## Installation
OGA.InfraBase is available via NuGet:
* NuGet Official Releases: [![NuGet](https://img.shields.io/nuget/vpre/OGA.InfraBase.svg?label=NuGet)](https://www.nuget.org/packages/OGA.InfraBase)

## Dependencies
This library depends on:
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [Microsoft.EntityFrameworkCore](https://github.com/dotnet/efcore)
* [NLog](https://github.com/NLog/NLog/)
* [OGA.AppSettings.Writeable](https://github.com/LeeWhite187/OGA.AppSettings.Writeable)
* [OGA.DomainBase](https://github.com/LeeWhite187/OGA.DomainBase)
* [OGA.SharedKernel](https://github.com/LeeWhite187/OGA.SharedKernel)

## Building OGA.InfraBase
This library is built with the new SDK-style projects.
It contains multiple projects, one for each of the following frameworks:
* NET Framework 4.5.2
* NET Framework 4.7
* NET 5
* NET 6
* NET 7

And, the output nuget package includes runtimes targets for:
* linux-64
* win-x64

## Framework and Runtime Support
Currently, the nuget package of this library supports the framework versions and runtimes of applications that I maintain (see above).
If someone needs others (older or newer), let me know, and I'll add them to the build script.

## Visual Studio
This library is currently built using Visual Studio 2019 17.1.

## License
Please see the [License](LICENSE).

## Opinionation Apology...
This library references NLog, directly, for now.\
I understand this may appear overly opinionated, at the infrastructure layer of a process stack. I agree... though, NLog works very well.\
Once I get a chance to circle back, and work through a more agnostic logging interface, I will update (removing the specific logger tie).

You're welcome to swap out and compile whatever logger you'd like, of course.\
If you have the need or feel inclined, send me feedback or a pull, so I know it helps someone, to make time and generalize the logging layer.

