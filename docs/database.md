# Database
## Migrations
This guide outlines how to manage database migrations using Entity Framework Core in a .NET environment.

### Pre-requisites
Ensure you have the dotnet-ef tool installed globally:
```bash
dotnet tool install --global dotnet-ef
```
If the tool is already installed, update it to the latest version:
```bash
dotnet tool update --global dotnet-ef
```
Additionally, ensure the following NuGet package is installed in your startup project:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### Migrations
#### Create Migration
If your migrations are stored in a separate project (e.g., MadWorldNL.MadTransfer.Infrastructures.Databases):
```bash
dotnet ef migrations add InitialCreate --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructures.Databases -o ../MadWorldNL.MadTransfer.Infrastructures.Databases/Migrations
```

#### Apply Migration

To apply the created migration to the database:
```bash
dotnet ef database update --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructures.Databases
```

#### Rollback
##### Listing All Migrations
To view all migrations:
```bash
dotnet ef migrations list --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructures.Databases
```

Sample Output:
```bash
20230201120000_InitialCreate
20230202130000_AddNewTable
```

##### Rolling Back to a Specific Migration
To rollback to a specific migration (e.g., InitialCreate):
```bash
dotnet ef database update InitialCreate --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructures.Databases
```

##### Rolling Back All Migrations
To revert the database to its initial state (no migrations applied):
```bash
dotnet ef database update 0 --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructures.Databases
```

#### Removing the Last Migration
If you need to remove the last migration (without applying it to the database):
```bash
dotnet ef migrations remove --context MadTransferContext --project ../MadWorldNL.MadTransfer.Infrastructure.Databases
```