namespace HappyNextHolidays.Server.Models;

public class Child
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FamilyProfileId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AgeInMonths { get; set; }
    public bool HasSpecialNeeds { get; set; }
    public string? DietaryRestrictions { get; set; }
    public TimeSpan NapStartTime { get; set; }
    public TimeSpan NapEndTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public FamilyProfile FamilyProfile { get; set; } = null!;
}
