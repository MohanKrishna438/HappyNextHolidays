namespace HappyNextHolidays.Server.Models;

public enum EmergencyContactType
{
    Hospital,
    PediatricClinic,
    EmergencyNumber,
    Embassy
}

public class EmergencyContact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CityId { get; set; }
    public EmergencyContactType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public City City { get; set; } = null!;
}
