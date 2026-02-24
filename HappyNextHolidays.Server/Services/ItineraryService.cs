using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.Models;

namespace HappyNextHolidays.Server.Services;

public interface IItineraryService
{
    Task<List<Itinerary>> GenerateItineraryAsync(Guid familyProfileId);
}

public class ItineraryService : IItineraryService
{
    private readonly AppDbContext _context;

    public ItineraryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Itinerary>> GenerateItineraryAsync(Guid familyProfileId)
    {
        var profile = await _context.FamilyProfiles
            .Include(fp => fp.Children)
            .FirstOrDefaultAsync(fp => fp.Id == familyProfileId);

        if (profile == null)
            throw new InvalidOperationException("Family profile not found");

        var itineraries = new List<Itinerary>();
        var currentDate = profile.StartDate;
        var dayNumber = 1;

        // Get available places for the city
        var availablePlaces = await _context.Places
            .Where(p => p.City.Name == profile.DestinationCity)
            .ToListAsync();

        while (currentDate <= profile.EndDate)
        {
            var dayItems = GenerateDayItems(profile, availablePlaces, currentDate);
            var totalWalking = dayItems.Where(i => !i.IsRestBlock)
                .Sum(i => 0.5m); // Simplified: 0.5km per place

            var itinerary = new Itinerary
            {
                FamilyProfileId = familyProfileId,
                DayNumber = dayNumber,
                Date = currentDate,
                StressScore = CalculateStressScore(profile, dayItems),
                TotalWalkingKm = totalWalking,
                Items = dayItems
            };

            itineraries.Add(itinerary);
            currentDate = currentDate.AddDays(1);
            dayNumber++;
        }

        return itineraries;
    }

    private List<ItineraryItem> GenerateDayItems(FamilyProfile profile, List<Place> availablePlaces, DateTime date)
    {
        var items = new List<ItineraryItem>();
        var youngestChildMonths = profile.Children.Any() ? profile.Children.Min(c => c.AgeInMonths) : 24;

        // Sort by kid-friendly score
        var suitablePlaces = availablePlaces
            .Where(p => p.KidFriendlyScore >= 3)
            .OrderByDescending(p => p.KidFriendlyScore)
            .Take(profile.SlowTravelMode ? 2 : 3)
            .ToList();

        var currentTime = new TimeSpan(9, 0, 0);

        foreach (var place in suitablePlaces)
        {
            // Check nap time overlap for children
            var childNapTimes = profile.Children.Select(c => (c.NapStartTime, c.NapEndTime)).ToList();
            var activityEnd = currentTime.Add(TimeSpan.FromMinutes(place.AvgDurationMinutes));

            // Skip if overlaps with any child's nap
            if (childNapTimes.Any(nap => OverlapsWithNapTime(currentTime, activityEnd, nap.NapStartTime, nap.NapEndTime)))
            {
                // Try scheduling after first child's nap
                if (childNapTimes.Any())
                {
                    currentTime = childNapTimes.First().NapEndTime.Add(TimeSpan.FromMinutes(15));
                    activityEnd = currentTime.Add(TimeSpan.FromMinutes(place.AvgDurationMinutes));
                }
            }

            // Don't schedule too late
            if (currentTime >= new TimeSpan(19, 0, 0))
                break;

            items.Add(new ItineraryItem
            {
                PlaceId = place.Id,
                StartTime = currentTime,
                EndTime = activityEnd,
                IsRestBlock = false
            });

            currentTime = activityEnd.Add(TimeSpan.FromMinutes(15));
        }

        // Add rest/nap blocks
        if (profile.Children.Any())
        {
            var napStart = profile.Children.First().NapStartTime;
            var napEnd = profile.Children.First().NapEndTime;
            items.Add(new ItineraryItem
            {
                PlaceId = Guid.Empty, // No specific place for rest block
                StartTime = napStart,
                EndTime = napEnd,
                IsRestBlock = true,
                Notes = "Nap time / Rest block"
            });
        }

        return items.OrderBy(i => i.StartTime).ToList();
    }

    private bool OverlapsWithNapTime(TimeSpan actStart, TimeSpan actEnd, TimeSpan napStart, TimeSpan napEnd)
    {
        return !(actEnd <= napStart || actStart >= napEnd);
    }

    private int CalculateStressScore(FamilyProfile profile, List<ItineraryItem> items)
    {
        var score = 5; // Base score

        // Fewer activities = less stress
        if (items.Count(i => !i.IsRestBlock) <= 2)
            score -= 2;

        // Slow travel mode = less stress
        if (profile.SlowTravelMode)
            score -= 1;

        // More adults = less stress
        if (profile.AdultsCount >= 2)
            score -= 1;

        return Math.Max(1, Math.Min(10, score));
    }
}
