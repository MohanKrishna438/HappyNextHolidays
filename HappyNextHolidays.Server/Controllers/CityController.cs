using Microsoft.AspNetCore.Mvc;
using HappyNextHolidays.Server.DTOs;
using HappyNextHolidays.Server.Services;

namespace HappyNextHolidays.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet("{cityName}")]
    public async Task<ActionResult<CityDetailDto>> GetCityDetails(string cityName)
    {
        var city = await _cityService.GetCityByNameAsync(cityName);
        if (city == null)
            return NotFound($"City '{cityName}' not found");

        var places = await _cityService.GetPlacesByCityAsync(cityName);
        var emergencyContacts = await _cityService.GetEmergencyContactsByCityAsync(cityName);

        var cityDto = new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Country = city.Country,
            Description = city.Description,
            AvgDailyTemp = city.AvgDailyTemp,
            IsInternational = city.IsInternational,
            AirportCode = city.AirportCode,
            EmergencyNumber = city.EmergencyNumber,
            Currency = city.Currency
        };

        var dto = new CityDetailDto
        {
            City = cityDto,
            Places = places.Select(p => new PlaceDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type.ToString(),
                Description = p.Description,
                AvgDurationMinutes = p.AvgDurationMinutes,
                KidFriendlyScore = p.KidFriendlyScore,
                WalkabilityScore = p.WalkabilityScore,
                IsStrollerFriendly = p.IsStrollerFriendly,
                HasHighChairs = p.HasHighChairs,
                HasKidsMenu = p.HasKidsMenu,
                HasChangingRoom = p.HasChangingRoom,
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }).ToList(),
            EmergencyContacts = emergencyContacts.Select(ec => new EmergencyContactDto
            {
                Id = ec.Id,
                Type = ec.Type.ToString(),
                Name = ec.Name,
                PhoneNumber = ec.PhoneNumber,
                Address = ec.Address,
                Notes = ec.Notes
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet("{cityName}/places")]
    public async Task<ActionResult<List<PlaceDto>>> GetPlaces(string cityName)
    {
        var places = await _cityService.GetPlacesByCityAsync(cityName);
        if (!places.Any())
            return NotFound($"No places found for city '{cityName}'");

        var dtos = places.Select(p => new PlaceDto
        {
            Id = p.Id,
            Name = p.Name,
            Type = p.Type.ToString(),
            Description = p.Description,
            AvgDurationMinutes = p.AvgDurationMinutes,
            KidFriendlyScore = p.KidFriendlyScore,
            WalkabilityScore = p.WalkabilityScore,
            IsStrollerFriendly = p.IsStrollerFriendly,
            HasHighChairs = p.HasHighChairs,
            HasKidsMenu = p.HasKidsMenu,
            HasChangingRoom = p.HasChangingRoom,
            Latitude = p.Latitude,
            Longitude = p.Longitude
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{cityName}/emergency")]
    public async Task<ActionResult<List<EmergencyContactDto>>> GetEmergencyContacts(string cityName)
    {
        var contacts = await _cityService.GetEmergencyContactsByCityAsync(cityName);
        if (!contacts.Any())
            return NotFound($"No emergency contacts found for city '{cityName}'");

        var dtos = contacts.Select(ec => new EmergencyContactDto
        {
            Id = ec.Id,
            Type = ec.Type.ToString(),
            Name = ec.Name,
            PhoneNumber = ec.PhoneNumber,
            Address = ec.Address,
            Notes = ec.Notes
        }).ToList();

        return Ok(dtos);
    }
}
