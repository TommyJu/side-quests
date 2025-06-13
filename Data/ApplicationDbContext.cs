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
    
    /*
        * This method is used to seed the database with initial data from a CSV file.
        * The CSV file is expected to be located in the wwwroot directory of the project.
        * The data is read using CsvHelper and added to the Activities DbSet.
    */
    private static IEnumerable<Activity> GetActivities() {
        string[] p = { Directory.GetCurrentDirectory(), "wwwroot", "activities.csv" };
        var csvFilePath = Path.Combine(p);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                Encoding = Encoding.UTF8,
                PrepareHeaderForMatch = args => args.Header.ToLower(),

        };

        var data = new List<Activity>().AsEnumerable();
        using (var reader = new StreamReader(csvFilePath)) {
            using (var csvReader = new CsvReader(reader, config)) {
                data = csvReader.GetRecords<Activity>().ToList();
            }
        }

        return data;
    }

    /*
        * This method is used to configure the model for the database context.
        * In this case, it is used to seed the database with initial data from a CSV file.
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Activity>().HasData(GetActivities());
        modelBuilder.Entity<ApplicationUser>()
        .HasMany(a => a.Activities)
        .WithMany(a => a.SavedByUsers)
        .UsingEntity(j => j.ToTable("UserSavedActivities"));
    }
} // end of class
