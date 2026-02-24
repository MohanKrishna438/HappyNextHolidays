# Happy Next Holidays - Trip Planner for Families with Toddlers

An MVP trip planner designed specifically for families traveling with toddlers (1-5 years old).

## Features

### 1. Family Profile Setup
Create and manage family profiles with:
- Destination
- Travel dates (start & end)
- Number of kids and their ages
- Nap schedule (start & end times)
- Stroller requirement
- Budget preference (Low, Medium, High)

### 2. Smart Itinerary Generator (Rule-Based)
Automatically generates daily itineraries based on:
- **Age restrictions**: Kids under 3 get max 2 outdoor activities per day
- **Nap time handling**: No activities scheduled during nap times
- **Stroller compatibility**: Filters non-walkable areas if needed
- **Daily walking limit**: Recommends max 4km walking per day
- **Activity sequencing**: Indoor activities prioritized to avoid excessive walking

### 3. Packing Checklist Generator
Auto-generates comprehensive packing lists based on:
- Child age
- Travel duration
- Automatically includes:
  - Diapers & wipes (for <3 years)
  - Extra clothes (quantity adjusted by age)
  - Medical supplies & thermometer
  - Feeding items (bottles, formula, sippy cups)
  - Gear (stroller, car seat, portable crib)
  - Entertainment & comfort items

### 4. Toddler Safety Panel
Destination-based safety information including:
- Nearby hospitals & pediatric clinics
- Kid-friendly restaurants
- Pharmacies
- Toddler-friendly parks
- Contact information & locations (lat/long for mapping)

## API Endpoints

### Family Profile Management
```
POST   /api/familyprofile          - Create family profile
GET    /api/familyprofile/{id}     - Get profile details
PUT    /api/familyprofile/{id}     - Update profile
DELETE /api/familyprofile/{id}     - Delete profile
```

### Itinerary Management
```
POST   /api/itinerary/generate/{familyProfileId}    - Generate smart itinerary
GET    /api/itinerary/family/{familyProfileId}      - Get generated itinerary
DELETE /api/itinerary/family/{familyProfileId}      - Delete itinerary
```

### Packing Checklist
```
POST   /api/packingchecklist/generate/{familyProfileId}                    - Generate checklist
GET    /api/packingchecklist/family/{familyProfileId}                      - Get checklist
PUT    /api/packingchecklist/{checklistId}/item/{itemId}                  - Mark item as packed
DELETE /api/packingchecklist/{checklistId}                                 - Delete checklist
```

### Safety Map
```
GET    /api/safetymap/destination/{destination}           - Get full safety map for destination
GET    /api/safetymap/{destination}/{infoType}            - Get specific type (Hospital, Restaurant, Pharmacy, Park)
POST   /api/safetymap                                      - Add new safety info
```

## Data Models

### FamilyProfile
- Id (int)
- Destination (string)
- TravelStartDate (datetime)
- TravelEndDate (datetime)
- NumberOfKids (int)
- KidsAges (List<int>)
- NapStartTime (TimeOnly)
- NapEndTime (TimeOnly)
- HasStroller (bool)
- BudgetPreference (string)

### Activity
- Id (int)
- Name (string)
- Description (string)
- MinAgeRequired (int)
- MaxAgeRecommended (int)
- DurationMinutes (int)
- WalkingDistanceKm (double)
- IsWalkerFriendly (bool)
- IsIndoor (bool)
- Category (string) - Park, Museum, Restaurant, Zoo, Play Center, Garden

### ItineraryDay
- Id (int)
- FamilyProfileId (int)
- Date (datetime)
- ScheduledActivities (List<ScheduledActivity>)
- Notes (string)

### ScheduledActivity
- Id (int)
- ItineraryDayId (int)
- ActivityId (int)
- Activity (Activity)
- StartTime (TimeOnly)
- EndTime (TimeOnly)
- Notes (string)

### PackingChecklist
- Id (int)
- FamilyProfileId (int)
- Items (List<ChecklistEntry>)
- GeneratedAt (datetime)

### ChecklistEntry
- Id (int)
- PackingChecklistId (int)
- ItemName (string)
- Category (string)
- Quantity (int)
- IsPacked (bool)
- Notes (string)

### SafetyInfo
- Id (int)
- Destination (string)
- InfoType (string) - Hospital, Restaurant, Pharmacy, Park
- Name (string)
- Address (string)
- PhoneNumber (string)
- Latitude (double)
- Longitude (double)
- Notes (string)
- IsVerified (bool)

## Example Usage

### 1. Create a Family Profile
```bash
POST /api/familyprofile
{
  "destination": "Orlando, Florida",
  "travelStartDate": "2024-07-01",
  "travelEndDate": "2024-07-07",
  "numberOfKids": 2,
  "kidsAges": [2, 4],
  "napStartTime": "13:00",
  "napEndTime": "15:00",
  "hasStroller": true,
  "budgetPreference": "Medium"
}
```

### 2. Generate Smart Itinerary
```bash
POST /api/itinerary/generate/1
```

Response includes daily activities scheduled around nap times and considering age constraints.

### 3. Generate Packing Checklist
```bash
POST /api/packingchecklist/generate/1
```

Returns categorized checklist with quantities calculated for trip duration and child ages.

### 4. Get Safety Map
```bash
GET /api/safetymap/destination/Orlando%2C%20Florida
```

Returns hospitals, restaurants, pharmacies, and parks with contact info.

## Technology Stack

- **.NET 10** - Backend framework
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **SQLite** - Database
- **CORS** - Cross-origin requests enabled

## Setup & Running

1. Install dependencies:
```bash
dotnet restore
```

2. Create database:
```bash
dotnet ef database update
```

3. Run the application:
```bash
dotnet run
```

API will be available at `https://localhost:5001/api`

## Future Enhancements

- [ ] Integration with Google Maps API for directions
- [ ] Weather API integration for packing recommendations
- [ ] Real-time flight & accommodation booking
- [ ] Community recommendations & reviews
- [ ] Photo gallery from past trips
- [ ] Expense tracking
- [ ] Multi-language support
- [ ] Mobile app (React Native)
- [ ] Real-time collaboration for multiple family members

## Database

Uses SQLite with automatic database creation. Database file: `app.db`

Pre-seeded with sample activities including Parks, Museums, Water Parks, Zoos, Play Centers, and Botanical Gardens.

Pre-seeded with sample safety info for Orlando, New York, and Los Angeles.
