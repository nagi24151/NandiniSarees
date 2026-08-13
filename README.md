# NandiniSarees APIs

Lightweight ASP.NET Core Web API for the NandiniSarees sample e-commerce backend.

Key points
- Target framework: .NET 10
- EF Core provider: Microsoft.EntityFrameworkCore.SqlServer (EF Core 8)
- CQRS pattern: read/write separation via IReadDbContext / IWriteDbContext and feature handlers
- Location of important files:
  - DbContext: NandiniSareesAPIs/Models/NandiniSareesDbContext.cs
  - Entities: NandiniSareesAPIs/Models/Entities/
  - Product CRUD (CQRS): NandiniSareesAPIs/Features/Products/
  - ProductController: NandiniSareesAPIs/Controllers/ProductController.cs
  - Database creation script: Database/Create_NandiniSarees_DB.sql
  - Dummy seed script: Database/Insert_DummyData_NandiniSarees.sql

Getting started (development)
1. Ensure .NET 8+ SDK installed (this repo targets .NET 10).
2. Restore and build the API project:

   dotnet restore NandiniSareesAPIs\NandiniSareesAPIs.csproj
   dotnet build NandiniSareesAPIs\NandiniSareesAPIs.csproj

3. Configure the database connection string
- Edit NandiniSareesAPIs/appsettings.json or set the environment variable `NANDINI_CONNECTION` used by the design-time factory.
- Example (SQL auth, dev only):

  Server=localhost;Database=NandiniSareesDb;User Id=nandini_user;Password=YourP@ss;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true

4. Install dotnet-ef (if you plan to run migrations locally):

   dotnet tool install --global dotnet-ef --version 8.*

5. Create and apply EF Core migrations (from repo root):

   dotnet ef migrations add InitialCreate --project NandiniSareesAPIs --startup-project NandiniSareesAPIs -o Migrations
   dotnet ef database update --project NandiniSareesAPIs --startup-project NandiniSareesAPIs

   Alternative (Visual Studio Package Manager Console): set the Default project to NandiniSareesAPIs and run Add-Migration / Update-Database.

6. (Optional) Run the provided SQL seed script to populate sample data:

   - Run Database/Insert_DummyData_NandiniSarees.sql in SSMS (connected to the same server).

Run the API

   dotnet run --project NandiniSareesAPIs

Open Swagger UI at https://localhost:5001/swagger (or the port printed by the app) while in Development environment.

Security notes
- Do NOT commit production credentials into appsettings.json. Use environment variables, dotnet user-secrets (for local dev), or an external secret store.
- For development with self-signed SQL certificates, add `TrustServerCertificate=True` to the connection string. For production, use a properly trusted certificate chain.

Troubleshooting
- "Login failed for user" — verify connection string credentials and that the SQL login exists and has access to the DB.
- "certificate chain was issued by an authority that is not trusted" — either install/trust the issuing CA on the client machine or add `TrustServerCertificate=True` for dev only.
- If Add-Migration/Update-Database commands are not found in Package Manager Console, install Microsoft.EntityFrameworkCore.Tools in the project or use the dotnet-ef global tool.
- Visual Studio error about duplicate MEF keys (e.g., SelfHostWebServer): try closing VS, removing ComponentModelCache, deleting the .vs folder, and restarting.

Further work
- Add MediatR for command/query dispatching, AutoMapper for DTO mappings, and more granular IEntityTypeConfiguration classes if you prefer.
- Move entities to individual class libraries if you plan to share models between services.

Contact
- Repo: https://github.com/nagi24151/NandiniSarees
