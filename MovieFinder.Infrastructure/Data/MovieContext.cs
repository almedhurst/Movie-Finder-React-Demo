using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieFinder.Core.Entities;

namespace MovieFinder.Infrastructure.Data;

public class MovieContext : IdentityDbContext<User>
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }
    
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Director> Directors { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<TitleActor> TitleActors { get; set; }
    public DbSet<TitleCategory> TitleCategories { get; set; }
    public DbSet<TitleDirector> TitleDirectors { get; set; }
    public DbSet<TitleWriter> TitleWriters { get; set; }
    public DbSet<Writer> Writers { get; set; }
    public DbSet<UserFavouriteTitle> UserFavouriteTitles { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));
                var dateTimeProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset));
                foreach (var property in properties)
                    modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                foreach (var property in dateTimeProperties)
                    modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
            }
    }
}