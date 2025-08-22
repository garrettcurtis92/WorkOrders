# WorkOrders

Simple ASP.NET Core 8 Razor Pages application to manage Work Orders.

## Features
- Create / Read / Update / Delete work orders
- Filter by Status, Priority, Department
- Full-text search (title & description)
- Column sorting (Title, Dept, Priority, Status, Updated)
- Pagination with query parameters
- Colored status badges (New=Blue, In Progress=Yellow, Completed=Green)
- Basic validation (DataAnnotations)

## Tech Stack
- ASP.NET Core 8 (Razor Pages)
- C#
- Bootstrap 5
- (Planned) EF Core + SQLite

## Running
```bash
dotnet restore
dotnet run
```
Navigate to http://localhost:5000 (or https://localhost:7000).

## Tests
(Planned) Filtering & sorting service tests and PageModel handler tests.

## Roadmap
- Persist data (EF Core SQLite + migrations)
- Authentication/Authorization
- Export (CSV / JSON)
- API endpoints
- Unit & integration tests
- CI with GitHub Actions

## License
MIT
