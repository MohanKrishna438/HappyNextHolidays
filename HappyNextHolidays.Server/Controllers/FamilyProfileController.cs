using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.DTOs;
using HappyNextHolidays.Server.Models;

namespace HappyNextHolidays.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FamilyProfileController : ControllerBase
{
    private readonly AppDbContext _context;

    public FamilyProfileController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FamilyProfileDto>> GetFamilyProfile(Guid id)
    {
        var profile = await _context.FamilyProfiles
            .Include(fp => fp.Children)
            .FirstOrDefaultAsync(fp => fp.Id == id);

        if (profile == null)
            return NotFound();

        return Ok(MapToDto(profile));
    }

    [HttpPost]
    public async Task<ActionResult<FamilyProfileDto>> CreateFamilyProfile(CreateFamilyProfileDto dto)
    {
        if (!Enum.TryParse<BudgetPreference>(dto.BudgetPreference, out var budgetPref))
            budgetPref = BudgetPreference.Medium;

        var profile = new FamilyProfile
        {
            DestinationCity = dto.DestinationCity,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            AdultsCount = dto.AdultsCount,
            KidsCount = dto.KidsCount,
            StrollerRequired = dto.StrollerRequired,
            BudgetPreference = budgetPref,
            SlowTravelMode = dto.SlowTravelMode
        };

        // Add children
        foreach (var childDto in dto.Children)
        {
            var child = new Child
            {
                Name = childDto.Name,
                AgeInMonths = childDto.AgeInMonths,
                HasSpecialNeeds = childDto.HasSpecialNeeds,
                DietaryRestrictions = childDto.DietaryRestrictions,
                NapStartTime = TimeSpan.Parse(childDto.NapStartTime),
                NapEndTime = TimeSpan.Parse(childDto.NapEndTime)
            };
            profile.Children.Add(child);
        }

        _context.FamilyProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFamilyProfile), new { id = profile.Id }, MapToDto(profile));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFamilyProfile(Guid id, CreateFamilyProfileDto dto)
    {
        var profile = await _context.FamilyProfiles
            .Include(fp => fp.Children)
            .FirstOrDefaultAsync(fp => fp.Id == id);

        if (profile == null)
            return NotFound();

        profile.DestinationCity = dto.DestinationCity;
        profile.StartDate = dto.StartDate;
        profile.EndDate = dto.EndDate;
        profile.AdultsCount = dto.AdultsCount;
        profile.KidsCount = dto.KidsCount;
        profile.StrollerRequired = dto.StrollerRequired;
        profile.SlowTravelMode = dto.SlowTravelMode;

        if (Enum.TryParse<BudgetPreference>(dto.BudgetPreference, out var budgetPref))
            profile.BudgetPreference = budgetPref;

        // Update children
        _context.Children.RemoveRange(profile.Children);
        foreach (var childDto in dto.Children)
        {
            var child = new Child
            {
                Name = childDto.Name,
                AgeInMonths = childDto.AgeInMonths,
                HasSpecialNeeds = childDto.HasSpecialNeeds,
                DietaryRestrictions = childDto.DietaryRestrictions,
                NapStartTime = TimeSpan.Parse(childDto.NapStartTime),
                NapEndTime = TimeSpan.Parse(childDto.NapEndTime),
                FamilyProfileId = id
            };
            profile.Children.Add(child);
        }

        _context.FamilyProfiles.Update(profile);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFamilyProfile(Guid id)
    {
        var profile = await _context.FamilyProfiles.FindAsync(id);
        if (profile == null)
            return NotFound();

        _context.FamilyProfiles.Remove(profile);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private FamilyProfileDto MapToDto(FamilyProfile profile)
    {
        return new FamilyProfileDto
        {
            Id = profile.Id,
            DestinationCity = profile.DestinationCity,
            StartDate = profile.StartDate,
            EndDate = profile.EndDate,
            AdultsCount = profile.AdultsCount,
            KidsCount = profile.KidsCount,
            StrollerRequired = profile.StrollerRequired,
            BudgetPreference = profile.BudgetPreference.ToString(),
            SlowTravelMode = profile.SlowTravelMode,
            Children = profile.Children.Select(c => new ChildDto
            {
                Id = c.Id,
                Name = c.Name,
                AgeInMonths = c.AgeInMonths,
                HasSpecialNeeds = c.HasSpecialNeeds,
                DietaryRestrictions = c.DietaryRestrictions,
                NapStartTime = c.NapStartTime,
                NapEndTime = c.NapEndTime
            }).ToList()
        };
    }
}
