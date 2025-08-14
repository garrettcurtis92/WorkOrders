# WorkOrders

A simple ASP.NET Core (net8.0) Razor Pages application for managing work orders.

## Features
- EF Core with SQLite (`app.db` local dev database)
- Basic Razor Pages scaffold (Index, About, etc.)
- WorkOrders model & migrations

## Local Development
1. Ensure .NET 8 SDK is installed.
2. (Optional) Update connection string in `appsettings.Development.json`.
3. Run database migrations (if not already created):
   ```bash
   dotnet ef database update
   ```
4. Run the app:
   ```bash
   dotnet run
   ```

The site will launch on the ports defined in `Properties/launchSettings.json`.

## Entity Framework Core
Add a migration:
```bash
dotnet ef migrations add MeaningfulName
```
Update database:
```bash
dotnet ef database update
```

## Git Hygiene
- `app.db` and `appsettings.Development.json` are excluded from version control.
- Migrations are committed.

## License
MIT (add a LICENSE file if you intend to open source).
