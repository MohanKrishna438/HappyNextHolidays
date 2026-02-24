using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.DTOs;
using HappyNextHolidays.Server.Services;

namespace HappyNextHolidays.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IItineraryService _itineraryService;

    public ItineraryController(AppDbContext context, IItineraryService itineraryService)
    {
        _context = context;
        _itineraryService = itineraryService;
    }

    [HttpPost("generate/{familyProfileId}")]
    public async Task<ActionResult<GeneratedItineraryDto>> GenerateItinerary(Guid familyProfileId)
    {
        var profile = await _context.FamilyProfiles.FindAsync(familyProfileId);
        if (profile == null)
            return NotFound("Family profile not found");

        try
        {
            var generatedItineraries = await _itineraryService.GenerateItineraryAsync(familyProfileId);

            foreach (var itinerary in generatedItineraries)
            {
                _context.Itineraries.Add(itinerary);
            }
            await _context.SaveChangesAsync();

            var dto = MapToDto(familyProfileId, generatedItineraries);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error generating itinerary: {ex.Message}");
        }
    }

    [HttpGet("family/{familyProfileId}")]
    public async Task<ActionResult<GeneratedItineraryDto>> GetItinerary(Guid familyProfileId)
    {
        var days = await _context.Itineraries
            .Where(d => d.FamilyProfileId == familyProfileId)
            .Include(d => d.Items)
            .ThenInclude(i => i.Place)
            .OrderBy(d => d.Date)
            .ToListAsync();

        if (!days.Any())
            return NotFound("No itinerary found for this family profile");

        var dto = MapToDto(familyProfileId, days);
        return Ok(dto);
    }

    [HttpDelete("family/{familyProfileId}")]
    public async Task<IActionResult> DeleteItinerary(Guid familyProfileId)
    {
        var days = await _context.Itineraries
            .Where(d => d.FamilyProfileId == familyProfileId)
            .ToListAsync();

        if (!days.Any())
            return NotFound();

        _context.Itineraries.RemoveRange(days);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private GeneratedItineraryDto MapToDto(Guid familyProfileId, List<Models.Itinerary> days)
    {
        return new GeneratedItineraryDto
        {
            FamilyProfileId = familyProfileId,
            TotalDays = days.Count,
            Days = days.Select(d => new ItineraryDto
            {
                Id = d.Id,
                DayNumber = d.DayNumber,
                Date = d.Date,
                StressScore = d.StressScore,
                TotalWalkingKm = d.TotalWalkingKm,
                Items = d.Items.Select(i => new ItineraryItemDto
                {
                    Id = i.Id,
                    PlaceId = i.PlaceId,
                    Place = i.Place != null ? new PlaceDto
                    {
                        Id = i.Place.Id,
                        Name = i.Place.Name,
                        Type = i.Place.Type.ToString(),
                        Description = i.Place.Description,
                        AvgDurationMinutes = i.Place.AvgDurationMinutes,
                        KidFriendlyScore = i.Place.KidFriendlyScore,
                        WalkabilityScore = i.Place.WalkabilityScore,
                        IsStrollerFriendly = i.Place.IsStrollerFriendly,
                        HasHighChairs = i.Place.HasHighChairs,
                        HasKidsMenu = i.Place.HasKidsMenu,
                        HasChangingRoom = i.Place.HasChangingRoom,
                        Latitude = i.Place.Latitude,
                        Longitude = i.Place.Longitude
                    } : null,
                    StartTime = i.StartTime,
                    EndTime = i.EndTime,
                    IsRestBlock = i.IsRestBlock,
                    Notes = i.Notes
                }).ToList()
            }).ToList()
        };
    }
}
