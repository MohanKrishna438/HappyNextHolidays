using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Models;

namespace HappyNextHolidays.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FamilyProfile> FamilyProfiles { get; set; }
    public DbSet<Child> Children { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Itinerary> Itineraries { get; set; }
    public DbSet<ItineraryItem> ItineraryItems { get; set; }
    public DbSet<PackingChecklistTemplate> PackingChecklistTemplates { get; set; }
    public DbSet<GeneratedPackingChecklist> GeneratedPackingChecklists { get; set; }
    public DbSet<GeneratedPackingItem> GeneratedPackingItems { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // FamilyProfile relationships
        modelBuilder.Entity<FamilyProfile>()
            .HasMany(fp => fp.Children)
            .WithOne(c => c.FamilyProfile)
            .HasForeignKey(c => c.FamilyProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FamilyProfile>()
            .HasMany(fp => fp.Itineraries)
            .WithOne(i => i.FamilyProfile)
            .HasForeignKey(i => i.FamilyProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FamilyProfile>()
            .HasOne(fp => fp.PackingChecklist)
            .WithOne(pc => pc.FamilyProfile)
            .HasForeignKey<GeneratedPackingChecklist>(pc => pc.FamilyProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // City relationships
        modelBuilder.Entity<City>()
            .HasMany(c => c.Places)
            .WithOne(p => p.City)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<City>()
            .HasMany(c => c.EmergencyContacts)
            .WithOne(ec => ec.City)
            .HasForeignKey(ec => ec.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Itinerary relationships
        modelBuilder.Entity<Itinerary>()
            .HasMany(i => i.Items)
            .WithOne(ii => ii.Itinerary)
            .HasForeignKey(ii => ii.ItineraryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Place relationship
        modelBuilder.Entity<Place>()
            .HasMany(p => p.ItineraryItems)
            .WithOne(ii => ii.Place)
            .HasForeignKey(ii => ii.PlaceId)
            .OnDelete(DeleteBehavior.Restrict);

        // GeneratedPackingChecklist relationships
        modelBuilder.Entity<GeneratedPackingChecklist>()
            .HasMany(gpc => gpc.Items)
            .WithOne(gpi => gpi.GeneratedChecklist)
            .HasForeignKey(gpi => gpi.GeneratedChecklistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        SeedCities(modelBuilder);
        SeedPlaces(modelBuilder);
        SeedPackingTemplates(modelBuilder);
        SeedEmergencyContacts(modelBuilder);
    }

    private void SeedCities(ModelBuilder modelBuilder)
    {
        var cities = new List<City>
        {
            // International Cities
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Orlando",
                Country = "United States",
                Description = "Family-friendly destination known for theme parks and attractions",
                AvgDailyTemp = 28,
                IsInternational = false,
                AirportCode = "MCO",
                EmergencyNumber = "911",
                Currency = "USD"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "New York",
                Country = "United States",
                Description = "Vibrant city with museums, parks, and diverse attractions",
                AvgDailyTemp = 10,
                IsInternational = false,
                AirportCode = "JFK",
                EmergencyNumber = "911",
                Currency = "USD"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "London",
                Country = "United Kingdom",
                Description = "Historic city with iconic landmarks and family attractions",
                AvgDailyTemp = 9,
                IsInternational = true,
                AirportCode = "LHR",
                EmergencyNumber = "999",
                Currency = "GBP"
            },
            
            // Indian Cities
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Delhi",
                Country = "India",
                Description = "Capital city with historical monuments, parks, and diverse attractions",
                AvgDailyTemp = 25,
                IsInternational = true,
                AirportCode = "DEL",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Mumbai",
                Country = "India",
                Description = "Cosmopolitan metropolis with beaches, parks, and family entertainment",
                AvgDailyTemp = 28,
                IsInternational = true,
                AirportCode = "BOM",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Bangalore",
                Country = "India",
                Description = "Modern city with tech parks, gardens, and kid-friendly attractions",
                AvgDailyTemp = 22,
                IsInternational = true,
                AirportCode = "BLR",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Hyderabad",
                Country = "India",
                Description = "Pearl of the south with historic sites, parks, and modern attractions",
                AvgDailyTemp = 27,
                IsInternational = true,
                AirportCode = "HYD",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Goa",
                Country = "India",
                Description = "Beach paradise with golden sand, parks, and family resorts",
                AvgDailyTemp = 30,
                IsInternational = true,
                AirportCode = "GOI",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Jaipur",
                Country = "India",
                Description = "Pink City with royal heritage, gardens, and cultural attractions",
                AvgDailyTemp = 26,
                IsInternational = true,
                AirportCode = "JAI",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Pune",
                Country = "India",
                Description = "Hill station near city with gardens, museums, and adventure activities",
                AvgDailyTemp = 24,
                IsInternational = true,
                AirportCode = "PNQ",
                EmergencyNumber = "100",
                Currency = "INR"
            },
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Kolkata",
                Country = "India",
                Description = "Cultural hub with museums, parks, and traditional cuisine",
                AvgDailyTemp = 26,
                IsInternational = true,
                AirportCode = "CCU",
                EmergencyNumber = "100",
                Currency = "INR"
            }
        };

        modelBuilder.Entity<City>().HasData(cities);
    }

    private void SeedPlaces(ModelBuilder modelBuilder)
    {
        var places = new List<Place>();
        
        // Define city IDs for reference (matching the order in SeedCities)
        var orlandoId = Guid.NewGuid();
        var nyId = Guid.NewGuid();
        var londonId = Guid.NewGuid();
        var delhiId = Guid.NewGuid();
        var mumbaiId = Guid.NewGuid();
        var bangaloreId = Guid.NewGuid();
        var hyderabadId = Guid.NewGuid();
        var goaId = Guid.NewGuid();
        var jaipurId = Guid.NewGuid();
        var puneId = Guid.NewGuid();
        var kolkataId = Guid.NewGuid();

        // Orlando Places
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = orlandoId, Name = "Lake Eustis Park", Type = PlaceType.Park, Description = "Beautiful park with playground suitable for toddlers", AvgDurationMinutes = 60, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 28.5518, Longitude = -81.3667 },
            new Place { Id = Guid.NewGuid(), CityId = orlandoId, Name = "Orlando Science Center", Type = PlaceType.Museum, Description = "Interactive museum with child-friendly exhibits", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 28.5412, Longitude = -81.3816 },
        });

        // New York Places
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = nyId, Name = "Central Park", Type = PlaceType.Park, Description = "Large park with playgrounds and open spaces", AvgDurationMinutes = 90, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, Latitude = 40.7829, Longitude = -73.9654 },
        });

        // London Places
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = londonId, Name = "Regent's Park", Type = PlaceType.Park, Description = "Beautiful park with zoo and playgrounds", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 51.5310, Longitude = -0.1553 },
        });

        // DELHI PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "Lodhi Garden", Type = PlaceType.Park, Description = "Historic garden with open spaces perfect for toddlers", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 28.5921, Longitude = 77.2197 },
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "Delhi Zoo", Type = PlaceType.Zoo, Description = "One of India's best zoos with various animal species", AvgDurationMinutes = 180, KidFriendlyScore = 5, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 28.6116, Longitude = 77.2499 },
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "Children's Park, Talkatora", Type = PlaceType.Park, Description = "Dedicated playground with toddler rides and activities", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 28.5834, Longitude = 77.2301 },
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "The Indian Habitat Centre Cafe", Type = PlaceType.Restaurant, Description = "Family-friendly cafe with healthy menu options", AvgDurationMinutes = 60, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 28.5892, Longitude = 77.2229 },
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "National Museum Delhi", Type = PlaceType.Museum, Description = "Interactive exhibits showcasing Indian heritage", AvgDurationMinutes = 120, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = true, Latitude = 28.6141, Longitude = 77.2194 },
            new Place { Id = Guid.NewGuid(), CityId = delhiId, Name = "Rajpath Walking Trail", Type = PlaceType.Park, Description = "Wide open spaces with monuments, perfect for stroller walks", AvgDurationMinutes = 100, KidFriendlyScore = 4, WalkabilityScore = 5, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 28.6087, Longitude = 77.2292 },
        });

        // MUMBAI PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Marine Drive", Type = PlaceType.Park, Description = "Famous beachside promenade with scenic views and playgrounds", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 5, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 18.9676, Longitude = 72.8194 },
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Mumbai Zoo (Veermata Jijamata Zoo)", Type = PlaceType.Zoo, Description = "Large zoo with diverse animal collection and play areas", AvgDurationMinutes = 150, KidFriendlyScore = 5, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 19.0176, Longitude = 72.8479 },
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Girgaum Beach", Type = PlaceType.Beach, Description = "Urban beach with clean sandy area suitable for kids", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 18.9689, Longitude = 72.8246 },
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Mahim Causeway Cafe", Type = PlaceType.Restaurant, Description = "Beachfront restaurant with kid-friendly options", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 19.0397, Longitude = 72.8240 },
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Science Centre, Mumbai", Type = PlaceType.Museum, Description = "Interactive science exhibits perfect for curious kids", AvgDurationMinutes = 150, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 19.0127, Longitude = 72.8261 },
            new Place { Id = Guid.NewGuid(), CityId = mumbaiId, Name = "Sanjay Gandhi National Park", Type = PlaceType.Park, Description = "Large nature park with picnic areas and walking trails", AvgDurationMinutes = 180, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = false, HasChangingRoom = false, Latitude = 19.2183, Longitude = 72.9781 },
        });

        // BANGALORE PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "Bangalore Cubbon Park", Type = PlaceType.Park, Description = "Large urban park with playground and open spaces", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 12.9716, Longitude = 77.5946 },
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "Bangalore Museum", Type = PlaceType.Museum, Description = "Interactive museum with exhibits for all ages", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = true, Latitude = 12.9707, Longitude = 77.5963 },
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "Innovative Children's Park", Type = PlaceType.IndoorPlay, Description = "Modern play center with safe indoor activities", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 12.9716, Longitude = 77.6412 },
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "MG Road Family Restaurant", Type = PlaceType.Restaurant, Description = "Modern family restaurant with international and Indian cuisine", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 12.9759, Longitude = 77.6087 },
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "Bannerghatta National Park", Type = PlaceType.Park, Description = "Nature park with zoo and safari rides", AvgDurationMinutes = 180, KidFriendlyScore = 4, WalkabilityScore = 2, IsStrollerFriendly = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 12.8340, Longitude = 77.5950 },
            new Place { Id = Guid.NewGuid(), CityId = bangaloreId, Name = "Ulsoor Lake Parks", Type = PlaceType.Park, Description = "Scenic lake park with walking trails and picnic spots", AvgDurationMinutes = 100, KidFriendlyScore = 3, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 12.9726, Longitude = 77.6068 },
        });

        // HYDERABAD PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "Sanjeevaiah Park", Type = PlaceType.Park, Description = "Large park with playground and scenic walking paths", AvgDurationMinutes = 100, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 17.3850, Longitude = 78.4867 },
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "Hyderabad Museum", Type = PlaceType.Museum, Description = "Cultural museum showcasing Hyderabadi heritage", AvgDurationMinutes = 90, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = true, Latitude = 17.3629, Longitude = 78.4751 },
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "Snow Kingdom", Type = PlaceType.IndoorPlay, Description = "Indoor theme park with fun rides for kids", AvgDurationMinutes = 180, KidFriendlyScore = 5, WalkabilityScore = 2, IsStrollerFriendly = false, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 17.4670, Longitude = 78.3561 },
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "Family Kitchen Hyderabad", Type = PlaceType.Restaurant, Description = "Hyderabadi specialties restaurant with kids corner", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 17.3850, Longitude = 78.4867 },
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "Charminar & Heritage Area", Type = PlaceType.Park, Description = "Historic monument with open spaces and street food stalls", AvgDurationMinutes = 90, KidFriendlyScore = 2, WalkabilityScore = 3, IsStrollerFriendly = false, HasChangingRoom = false, Latitude = 17.3608, Longitude = 78.4734 },
            new Place { Id = Guid.NewGuid(), CityId = hyderabadId, Name = "NTR Gardens (Indira Park)", Type = PlaceType.Park, Description = "Large landscaped garden with playgrounds and water features", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 17.3650, Longitude = 78.4767 },
        });

        // GOA PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Baga Beach", Type = PlaceType.Beach, Description = "Golden sandy beach with calm waters perfect for toddlers", AvgDurationMinutes = 120, KidFriendlyScore = 5, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 15.5500, Longitude = 73.7589 },
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Anjuna Beach", Type = PlaceType.Beach, Description = "Beautiful beach with rocky outcrops and safe swimming areas", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 15.5601, Longitude = 73.8102 },
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Dudhsagar Falls", Type = PlaceType.Park, Description = "Majestic waterfall viewpoint (can view from safe distance)", AvgDurationMinutes = 180, KidFriendlyScore = 3, WalkabilityScore = 2, IsStrollerFriendly = false, HasChangingRoom = false, Latitude = 15.2995, Longitude = 74.1184 },
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Goan Spice House", Type = PlaceType.Restaurant, Description = "Traditional Goan cuisine with family-friendly dining", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 15.5500, Longitude = 73.8200 },
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Aguada Fort Park", Type = PlaceType.Park, Description = "Historic fort with gardens overlooking the beach", AvgDurationMinutes = 90, KidFriendlyScore = 3, WalkabilityScore = 2, IsStrollerFriendly = false, HasChangingRoom = false, Latitude = 15.4850, Longitude = 73.7645 },
            new Place { Id = Guid.NewGuid(), CityId = goaId, Name = "Calangute Beach", Type = PlaceType.Beach, Description = "Long sandy beach with restaurants and water sports", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 15.5339, Longitude = 73.7664 },
        });

        // JAIPUR PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "City Palace Park", Type = PlaceType.Park, Description = "Historic palace grounds with open spaces for kids", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 26.9243, Longitude = 75.8229 },
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "Jantar Mantar", Type = PlaceType.Museum, Description = "UNESCO World Heritage astronomical observation site", AvgDurationMinutes = 60, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = false, Latitude = 26.9245, Longitude = 75.8233 },
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "Nahargarh Zoo", Type = PlaceType.Zoo, Description = "Zoo with Indian wildlife and play areas for children", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 2, IsStrollerFriendly = false, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 26.8849, Longitude = 75.8265 },
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "Maharaja Bhog Jaipur", Type = PlaceType.Restaurant, Description = "Royal cuisine restaurant with kids corner", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 26.9243, Longitude = 75.8229 },
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "Hawa Mahal Park", Type = PlaceType.Park, Description = "Open square near the famous pink palace with photo opportunities", AvgDurationMinutes = 60, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 26.9255, Longitude = 75.8243 },
            new Place { Id = Guid.NewGuid(), CityId = jaipurId, Name = "Government Museum", Type = PlaceType.Museum, Description = "Museum showcasing Jaipur's art and culture", AvgDurationMinutes = 90, KidFriendlyScore = 2, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = false, Latitude = 26.9263, Longitude = 75.8271 },
        });

        // PUNE PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Aga Khan Palace Park", Type = PlaceType.Park, Description = "Historic palace with gardens and walking trails", AvgDurationMinutes = 100, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 18.4893, Longitude = 73.8674 },
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Okayama Friendship Garden", Type = PlaceType.Park, Description = "Japanese garden with peaceful ambiance for relaxation", AvgDurationMinutes = 60, KidFriendlyScore = 3, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 18.5204, Longitude = 73.8567 },
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Essel World", Type = PlaceType.IndoorPlay, Description = "Family amusement park with rides suitable for toddlers", AvgDurationMinutes = 240, KidFriendlyScore = 5, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 18.5895, Longitude = 73.9206 },
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Konhapur Family Restaurant", Type = PlaceType.Restaurant, Description = "Family restaurant with Maharashtrian and North Indian cuisine", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 18.5204, Longitude = 73.8567 },
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Saras Baug Lake", Type = PlaceType.Park, Description = "Lake park with temple, open spaces and walking trails", AvgDurationMinutes = 90, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 18.4950, Longitude = 73.8580 },
            new Place { Id = Guid.NewGuid(), CityId = puneId, Name = "Rajendra Park (Katraj)", Type = PlaceType.Park, Description = "Scenic park with zoo, water features and play areas", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = false, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 18.4650, Longitude = 73.8650 },
        });

        // KOLKATA PLACES (6 places)
        places.AddRange(new[]
        {
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Victoria Memorial Park", Type = PlaceType.Park, Description = "Historic monument with beautiful gardens and open spaces", AvgDurationMinutes = 120, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasChangingRoom = true, Latitude = 22.5441, Longitude = 88.3426 },
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Indian Museum", Type = PlaceType.Museum, Description = "India's oldest museum with artifacts and exhibits for all ages", AvgDurationMinutes = 120, KidFriendlyScore = 3, WalkabilityScore = 3, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = true, Latitude = 22.5673, Longitude = 88.3699 },
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Birla Planetarium", Type = PlaceType.Museum, Description = "Planetarium with kid-friendly shows about astronomy", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 2, IsStrollerFriendly = true, HasKidsMenu = false, HasChangingRoom = true, Latitude = 22.5465, Longitude = 88.3577 },
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Oh! Calcutta Family Restaurant", Type = PlaceType.Restaurant, Description = "Bengali cuisine restaurant with family dining area", AvgDurationMinutes = 90, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = true, HasKidsMenu = true, HasChangingRoom = true, Latitude = 22.5673, Longitude = 88.3699 },
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Alipore Zoo", Type = PlaceType.Zoo, Description = "Historic zoo with diverse animal species and play areas", AvgDurationMinutes = 150, KidFriendlyScore = 4, WalkabilityScore = 3, IsStrollerFriendly = true, HasHighChairs = false, HasKidsMenu = true, HasChangingRoom = true, Latitude = 22.5207, Longitude = 88.3598 },
            new Place { Id = Guid.NewGuid(), CityId = kolkataId, Name = "Maidan Open Park", Type = PlaceType.Park, Description = "Large open green space with walking paths and vendors", AvgDurationMinutes = 90, KidFriendlyScore = 3, WalkabilityScore = 4, IsStrollerFriendly = true, HasChangingRoom = false, Latitude = 22.5637, Longitude = 88.3630 },
        });

        modelBuilder.Entity<Place>().HasData(places);
    }

    private void SeedPackingTemplates(ModelBuilder modelBuilder)
    {
        var templates = new List<PackingChecklistTemplate>
        {
            // ESSENTIALS (9 items)
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Diapers Size 4-5", MinAgeMonths = 0, MaxAgeMonths = 36, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 8 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Baby Wipes (Pack)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 2 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Wet Bag (Large)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Portable Changing Mat", MinAgeMonths = 0, MaxAgeMonths = 36, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Diaper Cream", MinAgeMonths = 0, MaxAgeMonths = 36, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Plastic Bags (Waste)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 5 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Hand Sanitizer", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Portable Stain Remover", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Blanket/Swaddle", MinAgeMonths = 0, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Essentials, DefaultQuantityPerDay = 0 },

            // CLOTHING (12 items)
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Short-sleeve T-shirts", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Hot, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 2 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Long-sleeve Shirts", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Cold, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Light Jacket/Sweater", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Cold, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Shorts", MinAgeMonths = 12, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Hot, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 2 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Pants/Trousers", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Socks (pairs)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Underwear/Pull-ups", MinAgeMonths = 12, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 2 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Shoes (pairs)", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Sandals/Slippers", MinAgeMonths = 12, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Hot, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Rain Jacket", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Rainy, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Nightwear/Pajamas", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Hat/Sun Cap", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Hot, Category = PackingCategory.Clothing, DefaultQuantityPerDay = 0 },

            // MEDICAL (11 items)
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Sunscreen SPF 50+", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = true, WeatherType = WeatherType.Hot, Category = PackingCategory.Medical, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Baby Lotion/Moisturizer", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Fever Reducer (Paracetamol)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Cough Syrup (Age-appropriate)", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Antibiotic Ointment", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Antihistamine (Cold medicine)", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "First Aid Kit", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Thermometer (Digital)", MinAgeMonths = 0, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Diarrhea Medicine", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Allergy Medicine (if needed)", MinAgeMonths = 12, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Vitamin D Drops", MinAgeMonths = 0, MaxAgeMonths = 12, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Medical, DefaultQuantityPerDay = 0 },

            // FOOD (8 items)
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Baby Formula", MinAgeMonths = 0, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 5 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Baby Bottles (with nipples)", MinAgeMonths = 0, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Bottle Brush & Cleaning Tablets", MinAgeMonths = 0, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Baby Cereal/Purees", MinAgeMonths = 4, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Snacks (Puffs, Crackers)", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 1 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Portable Spoon & Fork (Soft)", MinAgeMonths = 6, MaxAgeMonths = 36, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Teething Toys", MinAgeMonths = 4, MaxAgeMonths = 24, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 0 },
            new PackingChecklistTemplate { Id = Guid.NewGuid(), ItemName = "Sippy Cups/Water Bottles", MinAgeMonths = 6, MaxAgeMonths = 60, IsWeatherSpecific = false, WeatherType = WeatherType.Any, Category = PackingCategory.Food, DefaultQuantityPerDay = 1 },
        };

        modelBuilder.Entity<PackingChecklistTemplate>().HasData(templates);
    }

    private void SeedEmergencyContacts(ModelBuilder modelBuilder)
    {
        var orlandoId = Guid.NewGuid();
        var nyId = Guid.NewGuid();
        var londonId = Guid.NewGuid();
        var delhiId = Guid.NewGuid();
        var mumbaiId = Guid.NewGuid();
        var bangaloreId = Guid.NewGuid();
        var hyderabadId = Guid.NewGuid();
        var goaId = Guid.NewGuid();
        var jaipurId = Guid.NewGuid();
        var puneId = Guid.NewGuid();
        var kolkataId = Guid.NewGuid();

        var contacts = new List<EmergencyContact>
        {
            // INTERNATIONAL CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = orlandoId, Type = EmergencyContactType.Hospital, Name = "Arnold Palmer Hospital for Children", PhoneNumber = "(321) 841-5000", Address = "92 W Miller St, Orlando, FL 32806" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = orlandoId, Type = EmergencyContactType.PediatricClinic, Name = "Orlando Pediatric Center", PhoneNumber = "(407) 541-0067", Address = "500 S Orange Ave, Orlando, FL 32801" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = orlandoId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "911", Address = "Nationwide Emergency Services" },
            
            new EmergencyContact { Id = Guid.NewGuid(), CityId = nyId, Type = EmergencyContactType.Hospital, Name = "Mount Sinai Hospital", PhoneNumber = "(212) 241-6500", Address = "One Gustave L. Levy Pl, New York, NY 10029" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = nyId, Type = EmergencyContactType.PediatricClinic, Name = "Columbia Presbyterian Medical Center - Pediatrics", PhoneNumber = "(212) 305-5000", Address = "622 W 168th St, New York, NY 10032" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = nyId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "911", Address = "Nationwide Emergency Services" },
            
            new EmergencyContact { Id = Guid.NewGuid(), CityId = londonId, Type = EmergencyContactType.Hospital, Name = "Great Ormond Street Hospital", PhoneNumber = "+44 20 7405 9200", Address = "Great Ormond St, London WC1N 3JH, UK" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = londonId, Type = EmergencyContactType.PediatricClinic, Name = "St Mary's Hospital - Pediatric A&E", PhoneNumber = "+44 20 3312 6666", Address = "Praed Street, London W2 1NY, UK" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = londonId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "999", Address = "Nationwide Emergency Services" },

            // DELHI CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.Hospital, Name = "Apollo Hospital Delhi", PhoneNumber = "+91-11-2961-0101", Address = "Mathura Road, Sarita Vihar, New Delhi 110044" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.Hospital, Name = "Fortis Healthcare Delhi", PhoneNumber = "+91-11-4747-7777", Address = "A-Block, Pocket A1, Sector 8, Rohini, New Delhi 110085" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.PediatricClinic, Name = "Max Super Speciality Hospital - Saket", PhoneNumber = "+91-11-4141-1111", Address = "Press Enclave Marg, Saket, New Delhi 110017" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.PediatricClinic, Name = "Delhi Pediatric Clinic - Lutyens", PhoneNumber = "+91-11-2371-1234", Address = "Lutyens Delhi, New Delhi 110001" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services - Ambulance", PhoneNumber = "102", Address = "Nationwide Emergency Services" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = delhiId, Type = EmergencyContactType.EmergencyNumber, Name = "Police Emergency", PhoneNumber = "100", Address = "Nationwide Police Services" },

            // MUMBAI CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = mumbaiId, Type = EmergencyContactType.Hospital, Name = "Fortis Hospital Mulund", PhoneNumber = "+91-22-6132-8888", Address = "Mulund, Mumbai 400080" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = mumbaiId, Type = EmergencyContactType.Hospital, Name = "Lilavati Hospital & Research Centre", PhoneNumber = "+91-22-6145-1616", Address = "A-791, Bandra Reclamation, Bandra West, Mumbai 400050" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = mumbaiId, Type = EmergencyContactType.PediatricClinic, Name = "Apollo Hospitals - Pediatrics", PhoneNumber = "+91-22-3000-3000", Address = "Navi Mumbai 400614" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = mumbaiId, Type = EmergencyContactType.PediatricClinic, Name = "Breach Candy Hospital - Pediatrics", PhoneNumber = "+91-22-6620-3333", Address = "60 Bhulabhai Desai Road, Mumbai 400026" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = mumbaiId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // BANGALORE CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = bangaloreId, Type = EmergencyContactType.Hospital, Name = "Apollo Hospital Bangalore", PhoneNumber = "+91-80-4073-6000", Address = "Bannerghatta Road, Bangalore 560076" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = bangaloreId, Type = EmergencyContactType.Hospital, Name = "Fortis Hospital Bannerghatta", PhoneNumber = "+91-80-4069-1616", Address = "Bannerghatta Road, Bangalore 560076" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = bangaloreId, Type = EmergencyContactType.PediatricClinic, Name = "St. John's Medical College - Pediatrics", PhoneNumber = "+91-80-4065-2222", Address = "Bangalore 560034" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = bangaloreId, Type = EmergencyContactType.PediatricClinic, Name = "Manipal Hospital - Pediatrics", PhoneNumber = "+91-80-4225-0025", Address = "Bangalore 560017" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = bangaloreId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // HYDERABAD CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = hyderabadId, Type = EmergencyContactType.Hospital, Name = "Apollo Hospital Hyderabad", PhoneNumber = "+91-40-6616-6666", Address = "Jubilee Hills, Hyderabad 500033" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = hyderabadId, Type = EmergencyContactType.Hospital, Name = "Medicana Hospital", PhoneNumber = "+91-40-4475-4475", Address = "Hyderabad 500080" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = hyderabadId, Type = EmergencyContactType.PediatricClinic, Name = "Maxcure Hospital - Pediatrics", PhoneNumber = "+91-40-2333-3333", Address = "Madhapur, Hyderabad 500081" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = hyderabadId, Type = EmergencyContactType.PediatricClinic, Name = "Yashoda Hospital - Pediatrics", PhoneNumber = "+91-40-6703-7777", Address = "Hyderabad 500082" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = hyderabadId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // GOA CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = goaId, Type = EmergencyContactType.Hospital, Name = "Manipal Hospital Goa", PhoneNumber = "+91-832-660-5000", Address = "Alto Porvorim, North Goa 403521" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = goaId, Type = EmergencyContactType.Hospital, Name = "Apollo Victor Hospital", PhoneNumber = "+91-832-392-2233", Address = "Vasco da Gama, Goa 403802" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = goaId, Type = EmergencyContactType.PediatricClinic, Name = "Goa Medical College Hospital", PhoneNumber = "+91-832-245-8777", Address = "Bambolim, Goa 403202" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = goaId, Type = EmergencyContactType.PediatricClinic, Name = "Dr. Ashok's Pediatric Clinic", PhoneNumber = "+91-832-276-6000", Address = "Panjim, Goa 403001" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = goaId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // JAIPUR CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = jaipurId, Type = EmergencyContactType.Hospital, Name = "Fortis Escorts Hospital Jaipur", PhoneNumber = "+91-141-276-6666", Address = "Malviya Nagar, Jaipur 302017" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = jaipurId, Type = EmergencyContactType.Hospital, Name = "Apollo Hospital Jaipur", PhoneNumber = "+91-141-235-0235", Address = "Jaipur 302015" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = jaipurId, Type = EmergencyContactType.PediatricClinic, Name = "Max Hospital Jaipur - Pediatrics", PhoneNumber = "+91-141-305-3000", Address = "Jaipur City, Rajasthan 302001" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = jaipurId, Type = EmergencyContactType.PediatricClinic, Name = "Narayana Hospital - Pediatrics", PhoneNumber = "+91-141-300-6000", Address = "Jaipur 302021" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = jaipurId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // PUNE CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = puneId, Type = EmergencyContactType.Hospital, Name = "Apollo Hospitals Pune", PhoneNumber = "+91-20-6689-2222", Address = "Chandannagar, Pune 411014" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = puneId, Type = EmergencyContactType.Hospital, Name = "Deenanath Mangeshkar Hospital", PhoneNumber = "+91-20-2614-5433", Address = "Pune 411004" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = puneId, Type = EmergencyContactType.PediatricClinic, Name = "Inamdar Multispecialty Hospital - Pediatrics", PhoneNumber = "+91-20-2558-9898", Address = "Pune 411001" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = puneId, Type = EmergencyContactType.PediatricClinic, Name = "Ruby Hall Clinic - Pediatrics", PhoneNumber = "+91-20-2606-4000", Address = "Pune 411001" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = puneId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },

            // KOLKATA CONTACTS
            new EmergencyContact { Id = Guid.NewGuid(), CityId = kolkataId, Type = EmergencyContactType.Hospital, Name = "Apollo Gleneagles Hospital Kolkata", PhoneNumber = "+91-33-2320-6000", Address = "Magarpatta, Kolkata 700054" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = kolkataId, Type = EmergencyContactType.Hospital, Name = "AMRI Hospital Kolkata", PhoneNumber = "+91-33-6644-6644", Address = "Kolkata 700045" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = kolkataId, Type = EmergencyContactType.PediatricClinic, Name = "Belle Vue Clinic - Pediatrics", PhoneNumber = "+91-33-2282-6565", Address = "Kolkata 700006" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = kolkataId, Type = EmergencyContactType.PediatricClinic, Name = "Calcutta Medical Research Institute - Pediatrics", PhoneNumber = "+91-33-2426-2626", Address = "Kolkata 700009" },
            new EmergencyContact { Id = Guid.NewGuid(), CityId = kolkataId, Type = EmergencyContactType.EmergencyNumber, Name = "Emergency Medical Services", PhoneNumber = "102", Address = "Nationwide Emergency Services" },
        };

        modelBuilder.Entity<EmergencyContact>().HasData(contacts);
    }
}
