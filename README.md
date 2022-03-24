# Setting up the Database

## Allow using Telegram User Ids as Primary keys in the SQL Server

```SET IDENTITY_INSERT [dbo].[Users] OFF;```


## Applying migrations
```
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```

