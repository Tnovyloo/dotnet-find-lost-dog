using CsvHelper;
using System.Globalization;
using System.Text;
using LostDogApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using LostDogApp.Data;
using CsvHelper.Configuration;


namespace LostDogApp.Seeders {

    public static class CitySeeder {

        public static async Task SeedCitiesAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Skip seeding if cities are already in the database
            if (context.Cities.Any()) return;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", // Make sure the delimiter matches the CSV format
                HeaderValidated = null, // Disable header validation
                MissingFieldFound = null // Ignore missing fields
            };

            var filePath = Path.Combine(AppContext.BaseDirectory, "Dataset", "cities.csv");
            System.Console.WriteLine($"File path: {filePath}");

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            var cities = csv.GetRecords<City>().ToList();

            // Check for null or empty city names and set them to "XXX"
            foreach (var city in cities)
            {
                if (string.IsNullOrEmpty(city.Name))
                {
                    city.Name = "XXX";
                }
            }

            // Add cities to the database and save changes
            context.Cities.AddRange(cities);
            await context.SaveChangesAsync();

            System.Console.WriteLine("Cities seeded successfully.");
        }
    }
}

