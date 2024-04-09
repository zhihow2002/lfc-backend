4. Infrastructure (ClaimsPlugin.Infrastructure)
Purpose: Provides implementations for the interfaces defined in the Domain and Application layers, particularly those related to data access and persistence. It acts as a bridge between the application's core logic and the database or other external systems (like file systems, web services, etc.).
Contents: Database Contexts (like EF Core DbContext), Repository Implementations, Data Access Logic, Infrastructure-specific Services (like EmailSender, FileStorage), and External APIs Integration.
Focus: Interacting with external concerns like databases, file systems, external services, and third-party APIs.


Add Migration
dotnet ef migrations add InitialCreate --startup-project ../ClaimsPlugin.Api/ClaimsPlugin.Api.csproj

Add Seed Data
dotnet ef migrations add AddSeedData --startup-project ../ClaimsPlugin.Api/ClaimsPlugin.Api.csproj

Update Database
dotnet ef database update --startup-project ../ClaimsPlugin.Api/ClaimsPlugin.Api.csproj