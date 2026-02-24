using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.DTOs;
using HappyNextHolidays.Server.Services;

namespace HappyNextHolidays.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackingChecklistController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPackingService _packingService;

    public PackingChecklistController(AppDbContext context, IPackingService packingService)
    {
        _context = context;
        _packingService = packingService;
    }

    [HttpPost("generate/{familyProfileId}")]
    public async Task<ActionResult<GeneratedPackingChecklistDto>> GenerateChecklist(Guid familyProfileId)
    {
        var profile = await _context.FamilyProfiles.FindAsync(familyProfileId);
        if (profile == null)
            return NotFound("Family profile not found");

        try
        {
            var checklist = await _packingService.GeneratePackingChecklistAsync(familyProfileId);
            _context.GeneratedPackingChecklists.Add(checklist);
            await _context.SaveChangesAsync();

            return Ok(MapToDto(checklist));
        }
        catch (Exception ex)
        {
            return BadRequest($"Error generating checklist: {ex.Message}");
        }
    }

    [HttpGet("family/{familyProfileId}")]
    public async Task<ActionResult<GeneratedPackingChecklistDto>> GetChecklist(Guid familyProfileId)
    {
        var checklist = await _context.GeneratedPackingChecklists
            .Where(pc => pc.FamilyProfileId == familyProfileId)
            .Include(pc => pc.Items)
            .OrderByDescending(pc => pc.GeneratedAt)
            .FirstOrDefaultAsync();

        if (checklist == null)
            return NotFound("No packing checklist found for this family profile");

        return Ok(MapToDto(checklist));
    }

    [HttpPut("{checklistId}/item/{itemId}")]
    public async Task<IActionResult> UpdateChecklistItem(Guid checklistId, Guid itemId, GeneratedPackingItemDto dto)
    {
        var checklist = await _context.GeneratedPackingChecklists
            .Include(pc => pc.Items)
            .FirstOrDefaultAsync(pc => pc.Id == checklistId);

        if (checklist == null)
            return NotFound("Checklist not found");

        var item = checklist.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            return NotFound("Item not found");

        item.IsChecked = dto.IsChecked;
        item.Notes = dto.Notes;
        checklist.LastModifiedAt = DateTime.UtcNow;

        _context.GeneratedPackingChecklists.Update(checklist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{checklistId}")]
    public async Task<IActionResult> DeleteChecklist(Guid checklistId)
    {
        var checklist = await _context.GeneratedPackingChecklists
            .Include(pc => pc.Items)
            .FirstOrDefaultAsync(pc => pc.Id == checklistId);

        if (checklist == null)
            return NotFound();

        _context.GeneratedPackingChecklists.Remove(checklist);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private GeneratedPackingChecklistDto MapToDto(Models.GeneratedPackingChecklist checklist)
    {
        return new GeneratedPackingChecklistDto
        {
            Id = checklist.Id,
            FamilyProfileId = checklist.FamilyProfileId,
            GeneratedAt = checklist.GeneratedAt,
            Items = checklist.Items.Select(item => new GeneratedPackingItemDto
            {
                Id = item.Id,
                ItemName = item.ItemName,
                Category = item.Category.ToString(),
                Quantity = item.Quantity,
                IsChecked = item.IsChecked,
                Notes = item.Notes
            }).ToList()
        };
    }
}
