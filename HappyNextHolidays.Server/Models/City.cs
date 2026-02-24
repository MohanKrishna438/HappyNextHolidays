namespace HappyNextHolidays.Server.Models;

public class City
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? AvgDailyTemp { get; set; }
    public bool IsInternational { get; set; }
    public string? AirportCode { get; set; }
    public string EmergencyNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Place> Places { get; set; } = new();
    public List<EmergencyContact> EmergencyContacts { get; set; } = new();
}
