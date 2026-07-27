<p align="center">
    <img src="https://github.com/user-attachments/assets/7d62b044-b71d-4434-9721-3e5bd919301f" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/CargoWiseReplicationAPIInterface/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kris701/CargoWiseReplicationAPIInterface/actions/workflows/dotnet-desktop.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/CargoWiseReplicationAPIInterface/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/CargoWiseReplicationAPIInterface)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--10.0-green)

![Static Badge](https://img.shields.io/badge/CargoWiseReplicationAPIInterface-grey)
![Nuget](https://img.shields.io/nuget/v/CargoWiseReplicationAPIInterface)
![Nuget](https://img.shields.io/nuget/dt/CargoWiseReplicationAPIInterface)

![Static Badge](https://img.shields.io/badge/CargoWiseReplicationAPIInterface.Database-grey)
![Nuget](https://img.shields.io/nuget/v/CargoWiseReplicationAPIInterface.Database)
![Nuget](https://img.shields.io/nuget/dt/CargoWiseReplicationAPIInterface.Database)

![Static Badge](https://img.shields.io/badge/CargoWiseReplicationAPIInterface.Serialization-grey)
![Nuget](https://img.shields.io/nuget/v/CargoWiseReplicationAPIInterface.Serialization)
![Nuget](https://img.shields.io/nuget/dt/CargoWiseReplicationAPIInterface.Serialization)

# CargoWise Replication API Interface

*You can find the package on the [NuGet Package Manager](https://www.nuget.org/packages/CargoWiseReplicationAPIInterface).*

This is a small project to make a nice interface to work with CargoWise's Replication API.
Instead of you having to manually do checks and calls to the API for changes, this project makes it a lot easier.
As an example, say you wanted all changes that have occured since the LSN marker `0x00027FF400006C70014C` on the database table `dbo.GlbStaff`:

```csharp
class GlbStaff : BaseReturnData {
	public Guid? GS_PK { get; set; }
	public string? GS_LoginName { get; set; }
	public string? GS_EmploymentBasis { get; set; }
	...
}

var api = new ReplicationAPI("url", "username", "password");
var changes = await api.GetDetails<GlbStaff>("0x00027FF400006C70014C", "0x0003245300048BB70003", "dbo", "GlbStaff");

// OpCodes define what operation it is.
//   1       = delete
//   2       = insert
//   3 and 4 = update
var added = changes.Where(x => x.OpCode == 2);
```

The version number of this package correspond to the replication API version the replication API returns!

## CargoWiseReplicationAPIInterface.Database

*You can find the package on the [NuGet Package Manager](https://www.nuget.org/packages/CargoWiseReplicationAPIInterface.Database).*

This is a extension of the base API project, that makes it easier to make a MSSQL database to replicate data directly into.
You can use it to automatically build a replication database, all the STPs needed for insert/update/delete and SLN saving.
Its primary use is to give a simple class (`DatabaseReplicator`) where you give some setup parameters and from there on you can simply just use it to replicate into a database:

```csharp
var dbReplicator = new DatabaseReplicator(
	new DBClient("connection-string"),
	"CWR", // Or some other schema
	1000, // how many items to send to the database at a time
	new List<Type>(){ typeof(GlbStaff) },
	"replication-url",
	"replication-user",
	"replication-password"
);

await dbReplicator.Replicate();
```
This will automatically get all changes since last time you called the method,
insert them into your replication database and finally update the current
LSN value the system is at.

You can also use this project to build the replication database dynamically,
instead of making the tables and STPs yourself:

```csharp
var builder = new DatabaseQueryBuilder("CWR");
var query = builder.Build("YourNamespace.Tables");
query += builder.BuildLSNSystem();
```
This will create a SQL query that will build the entire database for you automatically.
If you make any changes to the table models, it will also update them (except if there are data in tables, you will have to update them manually)

## CargoWiseReplicationAPIInterface.Serialization

*You can find the package on the [NuGet Package Manager](https://www.nuget.org/packages/CargoWiseReplicationAPIInterface.Serialization).*

This is a helper project that can be used for some manual inserting and serialization.

The biggest thing in this is the `CWExcelImporter`, which is a class that
can be used to import direct reports (in excel format) from CW into a replication
database. (useful when you need to insert backlog data)

An example of how to use it can be seen here:

```csharp
var merger = new DatabaseMergerService(
	new DBClient("connection-string"),
	100000,
	"CWR"
);
var importer = new CWExcelImporter(merger, null, "YourNamespace.Tables");

// assuming you have read a Excel file in as a stream 'str':
await importer.ImportExcel(str, false);
```

This will automatically parse the format of CW's reports, match them
with the correct table, and insert the data.
By default, it will only do INSERT statements, not UPDATE.
If you set the `forceupdate` to true, then all the data will be treated
as UPDATE statements.

You can use [another tool of mine](https://github.com/kris701/CargoWiseReportTemplateCreator) to make the reports you need for exporting
CargoWise data in the correct format.
