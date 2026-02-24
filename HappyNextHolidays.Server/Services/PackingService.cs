using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.Models;

namespace HappyNextHolidays.Server.Services;

public interface IPackingService
{
    Task<GeneratedPackingChecklist> GeneratePackingChecklistAsync(Guid familyProfileId);
}

public class PackingService : IPackingService
{
    private readonly AppDbContext _context;

    public PackingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneratedPackingChecklist> GeneratePackingChecklistAsync(Guid familyProfileId)
    {
        var profile = await _context.FamilyProfiles
            .Include(fp => fp.Children)
            .FirstOrDefaultAsync(fp => fp.Id == familyProfileId);

        if (profile == null)
            throw new InvalidOperationException("Family profile not found");

        var templates = await _context.PackingChecklistTemplates.ToListAsync();
        var checklist = new GeneratedPackingChecklist
        {
            FamilyProfileId = familyProfileId
        };

        var tripDays = (int)(profile.EndDate - profile.StartDate).TotalDays + 1;

        // Generate items based on children's ages
        foreach (var child in profile.Children)
        {
            var relevantTemplates = templates
                .Where(t => child.AgeInMonths >= t.MinAgeMonths && child.AgeInMonths <= t.MaxAgeMonths)
                .ToList();

            foreach (var template in relevantTemplates)
            {
                var quantity = template.DefaultQuantityPerDay > 0
                    ? template.DefaultQuantityPerDay * tripDays
                    : 1;

                // Check if item already exists (for multi-child families)
                var existingItem = checklist.Items
                    .FirstOrDefault(i => i.ItemName == template.ItemName && i.Category == template.Category);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    checklist.Items.Add(new GeneratedPackingItem
                    {
                        ItemName = template.ItemName,
                        Quantity = quantity,
                        Category = template.Category,
                        Notes = $"For {child.Name} (age {child.AgeInMonths} months)"
                    });
                }
            }
        }

        // Sort by category
        checklist.Items = checklist.Items
            .OrderBy(i => i.Category)
            .ThenBy(i => i.ItemName)
            .ToList();

        return checklist;
    }
}
