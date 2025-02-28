# New Table Instruction

Follow these instructions to create a new table in the database.

## General Requirements
- Use Entity Framework Core: Replace raw SQL queries with Entity Framework Core for database operations.
- Implement the Repository Pattern: Create repositories for data access.
- Important: Use a reference between entities instead of just using the ID. This is done by defining navigation properties in your entity classes.
- Use Fluent Validation: Implement validation logic using Fluent Validation.
- Logging: Use Serilog for structured logging.
- Error Handling: Implement global exception handling middleware.
- Asynchronous Programming: Use asynchronous programming for database operations.
- Use the SQLite database provider: Use SQLite for the database provider.
- Implement the Migration Strategy: Ensure proper migrations are created and applied for the database schema changes.

## 1. Create Table
This is an example of how to create a table in the database.

## Creating Database Tables with Entity Framework Core

This guide demonstrates how to set up your new tables using C# with Entity Framework Core, along with FluentValidation and logging support.

### 1. Defining the Database Context and Entities

Use the following C# example as a guide to define your `DatabaseContext`, entity models:

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Serilog;

public class DatabaseContext : DbContext
{
  public DbSet<Example> Examples { get; set; }

  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Example>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name).IsRequired();
      entity.Property(e => e.CreatedAt).HasDefaultValueSql("datetime('now', 'utc')");
    });

  }
}

public class Example
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public DateTime CreatedAt { get; set; }
  public ICollection<AnotherEntity> AnotherEntities { get; set; }
}

### 2. Adding Model Validation

Integrate FluentValidation to ensure your entities meet business rules. This example validates key properties for the `Example` entity:

```csharp
public class ExampleValidator : AbstractValidator<Example>
{
  public ExampleValidator()
  {
    RuleFor(e => e.Name).NotEmpty();
    RuleFor(e => e.CreatedAt).NotNull();
  }
}
```

### 4. Setting Up Database Migrations

Ensure your database is correctly initialized via migration by using the following asynchronous setup:

```csharp
public async Task SetupDatabaseAsync()
{
  try
  {
    await _context.Database.MigrateAsync();
  }
  catch (Exception ex)
  {
    _logger.LogError(ex, "An error occurred while setting up the database.");
    throw;
  }
}
```