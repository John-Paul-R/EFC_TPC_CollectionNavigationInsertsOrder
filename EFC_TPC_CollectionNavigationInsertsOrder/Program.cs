using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace EFSampleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = new NpgsqlContext())
        {
            // Recreate database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Seed database


            db.SaveChanges();
        }

        using (var db = new NpgsqlContext())
        {
            // Run queries

            db.MagicSkills.Add(new MagicSkill
            {
                Name = "Firebolt",
                RunicName = "ignis",
            });
            db.MagicSkills.Add(new MagicSkill
            {
                Name = "Lightning",
                RunicName = "fulgur",
            });
            db.MartialSkills.Add(new MartialSkill
            {
                Name = "Combo1",
                HasStrike = true,
            });

            db.SaveChanges();

            // var query = db.Blogs.ToList();
            db.Players.Add(new Player
            {
                Skills = new List<PlayerToSkill>
                {
                    new()
                    {
                       SkillId = db.MagicSkills.First(s => s.Name == "Firebolt").Id,
                    }
                },
            });

            db.SaveChanges();
        }
        Console.WriteLine("Program finished.");
    }
}


public class NpgsqlContext : DbContext
{
    private static ILoggerFactory ContextLoggerFactory
        => LoggerFactory.Create(b =>
        {
            b
                .AddConsole()
                .AddFilter("", LogLevel.Debug);
        });

    // Declare DBSets
    public DbSet<MartialSkill> MartialSkills { get; set; }
    public DbSet<MagicSkill> MagicSkills { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "EFC_TPC_CollectionNavigationInsertsOrder",
            Username = "postgres",
        }.ToString();
        // Select 1 provider
        optionsBuilder
            // .UseSqlite(@"Server=(localdb)\mssqllocaldb;Database=_ModelApp;Trusted_Connection=True;Connect Timeout=5;ConnectRetryCount=0")
            // .UseSqlite("filename=_modelApp.db")
            //.UseInMemoryDatabase(databaseName: "_modelApp")
            //.UseCosmos("https://localhost:8081", @"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "_ModelApp")
            .UseNpgsql(connectionString)
            .EnableSensitiveDataLogging()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            .UseLoggerFactory(ContextLoggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractSkill>(e =>
        {
            e.UseTpcMappingStrategy()
                .HasKey(s => s.Id);
            e.Property(s => s.Id);
        });

        modelBuilder.Entity<MartialSkill>();
        modelBuilder.Entity<MagicSkill>();

        modelBuilder.Entity<Player>()
            .HasMany(p => p.Skills)
            .WithOne(pts => pts.Player);

        modelBuilder.Entity<AbstractSkill>()
            .HasMany<PlayerToSkill>()
            .WithOne(pts => pts.Skill);


    }
}
