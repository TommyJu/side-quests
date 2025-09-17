using CsvHelper;
using CsvHelper.Configuration;
using im_bored.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace im_bored.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<UserSavedActivity> UserSavedActivities { get; set; }
    
    /*
        * This method is used to seed the database with initial data from a CSV file.
        * The CSV file is expected to be located in the wwwroot directory of the project.
        * The data is read using CsvHelper and added to the Activities DbSet.
    */
    private static IEnumerable<Activity> GetActivities()
    {
        string[] p = { Directory.GetCurrentDirectory(), "wwwroot", "activities.csv" };
        var csvFilePath = Path.Combine(p);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            PrepareHeaderForMatch = args => args.Header.ToLower(),

        };

        var data = new List<Activity>().AsEnumerable();
        using (var reader = new StreamReader(csvFilePath))
        {
            using (var csvReader = new CsvReader(reader, config))
            {
                data = csvReader.GetRecords<Activity>().ToList();
            }
        }

        return data;
    }

    /*
        * This method is used to configure the model for the database context.
        * In this case, it is used to seed the database with initial data from a CSV file.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Activity Entity + seeding
        modelBuilder.Entity<Activity>().HasData(GetActivities());

        // ApplicationUser entity
        modelBuilder.Entity<ApplicationUser>();

        // UserSavedActivity join entity (M:N relationship)

        // Configure composite key
        modelBuilder.Entity<UserSavedActivity>()
            .HasKey(usa => new { usa.UserId, usa.ActivityId });

        // Configure relationships
        modelBuilder.Entity<UserSavedActivity>()
            .HasOne(usa => usa.User)
            .WithMany(u => u.SavedActivities)
            .HasForeignKey(usa => usa.UserId);

        modelBuilder.Entity<UserSavedActivity>()
            .HasOne(usa => usa.Activity)
            .WithMany(a => a.SavedByUsers)
            .HasForeignKey(usa => usa.ActivityId);
    }
} // end of class
