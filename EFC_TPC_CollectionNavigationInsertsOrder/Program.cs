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
            db.Database.Migrate();

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

            // -- This is the repro --

            db.MagicSkills.Add(new MagicSkill
            {
                Name = "Fly",
                RunicName = "asjdhkas",
                PlayersWithSkill = new List<PlayerToSkill>
                {
                    new PlayerToSkill()
                    {
                        PlayerId = db.Players.First().Id,
                    }
                },
            });

            db.SaveChanges();
            // Atm, this SaveChanges is yielding correct insert order:
            // info: Microsoft.EntityFrameworkCore.Database.Command[20101]
            //   Executed DbCommand (1ms) [Parameters=[@p0='14f59065-2c8a-44c7-bfa4-0422c786e10f', @p1='Fly' (Nullable = false), @p2='asjdhkas' (Nullable = false), @p3='1185f433-5c07-4b4f-aa53-dfcfd356a27a', @p4='cb799828-e49d-4de6-9bd2-1463d20d1025', @p5='14f59065-2c8a-44c7-bfa4-0422c786e10f'], CommandType='Text', CommandTimeout='30']
            //   INSERT INTO "MagicSkills" ("Id", "Name", "RunicName")
            //   VALUES (@p0, @p1, @p2);
            //   INSERT INTO "PlayerToSkill" ("Id", "PlayerId", "SkillId")
            //   VALUES (@p3, @p4, @p5);
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
            .HasMany<PlayerToSkill>(a => a.PlayersWithSkill)
            .WithOne(pts => pts.Skill);

        modelBuilder.Entity<SkillRoView>()
            .HasMany<PlayerToSkill>(a => a.Skills)
            .WithOne(pts => pts.SkillRoView);

    }
}
