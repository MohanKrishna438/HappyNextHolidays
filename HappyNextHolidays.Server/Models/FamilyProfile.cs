namespace HappyNextHolidays.Server.Models;

public enum BudgetPreference
{
    Low,
    Medium,
    High
}

public class FamilyProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string DestinationCity { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AdultsCount { get; set; }
    public int KidsCount { get; set; }
    public bool StrollerRequired { get; set; }
    public BudgetPreference BudgetPreference { get; set; } = BudgetPreference.Medium;
    public bool SlowTravelMode { get; set; } // If true, fewer activities per day
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Child> Children { get; set; } = new();
    public List<Itinerary> Itineraries { get; set; } = new();
    public GeneratedPackingChecklist? PackingChecklist { get; set; }
}
