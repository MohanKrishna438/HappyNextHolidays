namespace HappyNextHolidays.Server.DTOs;

// Family Profile DTOs
public class CreateFamilyProfileDto
{
    public string DestinationCity { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AdultsCount { get; set; }
    public int KidsCount { get; set; }
    public bool StrollerRequired { get; set; }
    public string BudgetPreference { get; set; } = "Medium"; // Low, Medium, High
    public bool SlowTravelMode { get; set; }
    public List<CreateChildDto> Children { get; set; } = new();
}

public class FamilyProfileDto
{
    public Guid Id { get; set; }
    public string DestinationCity { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AdultsCount { get; set; }
    public int KidsCount { get; set; }
    public bool StrollerRequired { get; set; }
    public string BudgetPreference { get; set; } = string.Empty;
    public bool SlowTravelMode { get; set; }
    public List<ChildDto> Children { get; set; } = new();
}

// Child DTOs
public class CreateChildDto
{
    public string Name { get; set; } = string.Empty;
    public int AgeInMonths { get; set; }
    public bool HasSpecialNeeds { get; set; }
    public string? DietaryRestrictions { get; set; }
    public string NapStartTime { get; set; } = "13:00"; // HH:mm format
    public string NapEndTime { get; set; } = "15:00";
}

public class ChildDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AgeInMonths { get; set; }
    public bool HasSpecialNeeds { get; set; }
    public string? DietaryRestrictions { get; set; }
    public TimeSpan NapStartTime { get; set; }
    public TimeSpan NapEndTime { get; set; }
}

// City DTOs
public class CityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? AvgDailyTemp { get; set; }
    public bool IsInternational { get; set; }
    public string? AirportCode { get; set; }
    public string EmergencyNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
}

// Place DTOs
public class PlaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int AvgDurationMinutes { get; set; }
    public int KidFriendlyScore { get; set; }
    public int WalkabilityScore { get; set; }
    public bool IsStrollerFriendly { get; set; }
    public bool? HasHighChairs { get; set; }
    public bool? HasKidsMenu { get; set; }
    public bool? HasChangingRoom { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

// Itinerary DTOs
public class ItineraryItemDto
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public PlaceDto? Place { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsRestBlock { get; set; }
    public string? Notes { get; set; }
}

public class ItineraryDto
{
    public Guid Id { get; set; }
    public int DayNumber { get; set; }
    public DateTime Date { get; set; }
    public int StressScore { get; set; }
    public decimal TotalWalkingKm { get; set; }
    public List<ItineraryItemDto> Items { get; set; } = new();
}

public class GeneratedItineraryDto
{
    public Guid FamilyProfileId { get; set; }
    public int TotalDays { get; set; }
    public List<ItineraryDto> Days { get; set; } = new();
}

// Packing Checklist DTOs
public class GeneratedPackingItemDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool IsChecked { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class GeneratedPackingChecklistDto
{
    public Guid Id { get; set; }
    public Guid FamilyProfileId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public List<GeneratedPackingItemDto> Items { get; set; } = new();
}

// Emergency Contact DTOs
public class EmergencyContactDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class CityDetailDto
{
    public CityDto City { get; set; } = null!;
    public List<PlaceDto> Places { get; set; } = new();
    public List<EmergencyContactDto> EmergencyContacts { get; set; } = new();
}
