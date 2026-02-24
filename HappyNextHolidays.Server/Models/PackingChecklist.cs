namespace HappyNextHolidays.Server.Models;

public enum PackingCategory
{
    Clothing,
    Medical,
    Food,
    Essentials
}

public enum WeatherType
{
    Hot,
    Cold,
    Rainy,
    Any
}

public class PackingChecklistTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; } = string.Empty;
    public int MinAgeMonths { get; set; }
    public int MaxAgeMonths { get; set; }
    public bool IsWeatherSpecific { get; set; }
    public WeatherType WeatherType { get; set; } = WeatherType.Any;
    public PackingCategory Category { get; set; }
    public int DefaultQuantityPerDay { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class GeneratedPackingChecklist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FamilyProfileId { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; set; }

    // Navigation
    public FamilyProfile FamilyProfile { get; set; } = null!;
    public List<GeneratedPackingItem> Items { get; set; } = new();
}

public class GeneratedPackingItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GeneratedChecklistId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }
    public PackingCategory Category { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public GeneratedPackingChecklist GeneratedChecklist { get; set; } = null!;
}
