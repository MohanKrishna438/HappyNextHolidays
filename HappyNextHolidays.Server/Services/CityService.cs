using Microsoft.EntityFrameworkCore;
using HappyNextHolidays.Server.Data;
using HappyNextHolidays.Server.Models;

namespace HappyNextHolidays.Server.Services;

public interface ICityService
{
    Task<City?> GetCityByNameAsync(string name);
    Task<List<Place>> GetPlacesByCityAsync(string cityName);
    Task<List<EmergencyContact>> GetEmergencyContactsByCityAsync(string cityName);
}

public class CityService : ICityService
{
    private readonly AppDbContext _context;

    public CityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetCityByNameAsync(string name)
    {
        return await _context.Cities
            .Include(c => c.Places)
            .Include(c => c.EmergencyContacts)
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<List<Place>> GetPlacesByCityAsync(string cityName)
    {
        return await _context.Places
            .Where(p => p.City.Name == cityName)
            .ToListAsync();
    }

    public async Task<List<EmergencyContact>> GetEmergencyContactsByCityAsync(string cityName)
    {
        return await _context.EmergencyContacts
            .Where(ec => ec.City.Name == cityName)
            .ToListAsync();
    }
}
