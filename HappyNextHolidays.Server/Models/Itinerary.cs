namespace HappyNextHolidays.Server.Models;

public class Itinerary
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FamilyProfileId { get; set; }
    public int DayNumber { get; set; }
    public DateTime Date { get; set; }
    public int StressScore { get; set; } // 1-10, where 1 is most relaxed
    public decimal TotalWalkingKm { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public FamilyProfile FamilyProfile { get; set; } = null!;
    public List<ItineraryItem> Items { get; set; } = new();
}

public class ItineraryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ItineraryId { get; set; }
    public Guid PlaceId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsRestBlock { get; set; } // For nap time, lunch, rest
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Itinerary Itinerary { get; set; } = null!;
    public Place Place { get; set; } = null!;
}
