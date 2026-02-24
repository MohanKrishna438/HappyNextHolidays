namespace HappyNextHolidays.Server.Models;

public enum PlaceType
{
    Park,
    Zoo,
    Restaurant,
    Hospital,
    Pharmacy,
    Museum,
    IndoorPlay,
    Beach
}

public class Place
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public PlaceType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int AvgDurationMinutes { get; set; }
    public int KidFriendlyScore { get; set; } // 1-5
    public int WalkabilityScore { get; set; } // 1-5
    public bool IsStrollerFriendly { get; set; }
    public bool? HasHighChairs { get; set; }
    public bool? HasKidsMenu { get; set; }
    public bool? HasChangingRoom { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public City City { get; set; } = null!;
    public List<ItineraryItem> ItineraryItems { get; set; } = new();
}
