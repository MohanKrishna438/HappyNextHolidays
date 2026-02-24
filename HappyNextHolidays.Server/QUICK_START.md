# Quick Start Guide - Happy Next Holidays MVP

## ?? Getting Started in 5 Minutes

### Prerequisites
- .NET 10 SDK installed
- Visual Studio, VS Code, or any code editor
- PowerShell or Terminal

### Step 1: Build the Project
```bash
cd HappyNextHolidays.Server
dotnet restore
dotnet build
```

### Step 2: Run the Application
```bash
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

### Step 3: Test the API

Open a REST client (Postman, Thunder Client, or VS Code REST Client extension) and try these requests:

#### Create a Family Profile
```http
POST https://localhost:5001/api/familyprofile
Content-Type: application/json

{
  "destination": "Orlando, Florida",
  "travelStartDate": "2024-07-01T00:00:00",
  "travelEndDate": "2024-07-07T00:00:00",
  "numberOfKids": 2,
  "kidsAges": [2, 4],
  "napStartTime": "13:00",
  "napEndTime": "15:00",
  "hasStroller": true,
  "budgetPreference": "Medium"
}
```

**Response**: 201 Created with profile ID (e.g., ID: 1)

#### Generate Smart Itinerary
```http
POST https://localhost:5001/api/itinerary/generate/1
```

**Response**: 7-day itinerary with scheduled activities respecting:
- Age constraints
- Nap times
- Stroller needs
- Walking distances

#### Generate Packing Checklist
```http
POST https://localhost:5001/api/packingchecklist/generate/1
```

**Response**: Categorized packing list with quantities calculated for:
- Each child's age
- Trip duration (7 days)
- All essentials pre-populated

#### Get Safety Information
```http
GET https://localhost:5001/api/safetymap/destination/Orlando%2C%20Florida
```

**Response**: All hospitals, pharmacies, restaurants, and parks with contact info

---

## ?? Project Structure

```
HappyNextHolidays.Server/
??? Models/                      # Data models
?   ??? FamilyProfile.cs        # Family trip details
?   ??? Itinerary.cs            # Activities & schedules
?   ??? PackingChecklist.cs     # Packing items
?   ??? SafetyInfo.cs           # Destination safety data
?
??? Data/
?   ??? AppDbContext.cs         # Entity Framework Core setup
?
??? Services/                    # Business logic
?   ??? ItineraryService.cs     # Smart itinerary generation
?   ??? PackingService.cs       # Smart checklist generation
?   ??? SafetyService.cs        # Safety data management
?
??? Controllers/                 # API endpoints
?   ??? FamilyProfileController.cs
?   ??? ItineraryController.cs
?   ??? PackingChecklistController.cs
?   ??? SafetyMapController.cs
?
??? DTOs/
?   ??? ApiDtos.cs              # Request/response objects
?
??? appsettings.json            # Configuration
??? Program.cs                  # App startup & configuration
??? app.db                       # SQLite database (auto-created)
```

---

## ?? Core Features at a Glance

### 1. Family Profile Setup
**What**: Store family trip details
**API**: `POST /api/familyprofile`
**Input**: Destination, dates, kids' ages, nap schedule, stroller need, budget
**Output**: Saved profile with ID

### 2. Smart Itinerary Generator
**What**: Auto-generate age-appropriate daily activities
**API**: `POST /api/itinerary/generate/{familyProfileId}`
**Smart Logic**:
- Kids < 3: max 2 outdoor activities/day
- Avoid nap times
- Respect stroller needs
- Keep walking < 4km/day
- Sequence activities efficiently
**Output**: 7-day itinerary with times & activities

### 3. Packing Checklist
**What**: Auto-generate age-specific packing list
**API**: `POST /api/packingchecklist/generate/{familyProfileId}`
**Smart Logic**:
- Diapers: 6-8/day × trip duration (age-dependent)
- Clothes: 2-3 sets/day × trip duration (age-dependent)
- All health, feeding, gear items
- Multi-kid consolidation
**Output**: Categorized checklist with quantities

### 4. Safety Map
**What**: Find hospitals, pharmacies, restaurants, parks
**API**: `GET /api/safetymap/destination/{destination}`
**Pre-Seeded Data**:
- Orlando, Florida
- New York, New York
- Los Angeles, California
**Output**: Locations with address, phone, GPS coordinates

---

## ?? Example Usage Scenario

**Family**: Sarah, Mike, and their kids (2 and 4 years old)
**Trip**: Orlando, July 1-7

### Step 1: Create Profile
```json
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

### Step 2: Get Itinerary
System generates:
- Day 1: Park Play (9am-10am) ? Museum (10:30am-12pm) ? REST (1-3pm NAP) ? Soft Play (3:30-4:30pm)
- Day 2: Similar schedule, different activities
- ... (7 days total)

**Smart Decisions Made**:
- ? Only 2 outdoor activities (respects age 2)
- ? 1-3pm reserved for nap
- ? All activities under 4km walking
- ? 15-min buffers between activities

### Step 3: Get Packing List
System generates:
```
HYGIENE (42 items)
  ? Diapers (42) - 6/day for 2-year-old
  ? Baby Wipes (2 packs)
  
CLOTHING (28 items)
  ? Extra Clothes (28 sets) - 2/day for both kids
  ? Socks (12 pairs)
  
HEALTH (multiple items)
  ? Medicine Kit (1)
  ? Sunscreen SPF 30+ (1)
  ? Thermometer (1)
  
... (more categories)
```

**Smart Calculations**:
- ? 42 diapers: 6/day × 7 days for 2-year-old
- ? 28 clothes: 2 per day × 7 days for both kids
- ? Age-appropriate items only

### Step 4: Get Safety Info
System shows:
```
HOSPITALS
  - Orlando Health Arnold Palmer Hospital
    92 W Miller St | (321) 841-5000
    
PHARMACIES
  - CVS Pharmacy (24-hour)
    123 Main St | (407) 555-0100
    
RESTAURANTS
  - Baby-Friendly Bistro
    456 Park Ave | High chairs, allergy-friendly
    
PARKS
  - Lake Eustis Park
    Playground, picnic areas, toddler-friendly
```

**Ready for Trip!** ?

---

## ?? Configuration

### Database
- **Type**: SQLite
- **Location**: `app.db` (auto-created)
- **Connection**: Defined in `appsettings.json`

### API Base URL
- **Local**: `https://localhost:5001/api`
- **Production**: Configure in `appsettings.Production.json`

### CORS
- **Enabled**: All origins for MVP
- **Production**: Configure specific origins in `Program.cs`

---

## ?? Database Schema

### FamilyProfile Table
- Id (PK)
- Destination
- TravelStartDate
- TravelEndDate
- NumberOfKids
- KidsAges (JSON array)
- NapStartTime
- NapEndTime
- HasStroller
- BudgetPreference

### Activity Table
- Id (PK)
- Name, Description
- MinAgeRequired, MaxAgeRecommended
- DurationMinutes
- WalkingDistanceKm
- IsWalkerFriendly, IsIndoor
- Category

### ItineraryDay Table
- Id (PK)
- FamilyProfileId (FK)
- Date
- Notes

### ScheduledActivity Table
- Id (PK)
- ItineraryDayId (FK)
- ActivityId (FK)
- StartTime, EndTime

### PackingChecklist Table
- Id (PK)
- FamilyProfileId (FK)
- GeneratedAt

### ChecklistEntry Table
- Id (PK)
- PackingChecklistId (FK)
- ItemName
- Category
- Quantity
- IsPacked
- Notes

### SafetyInfo Table
- Id (PK)
- Destination
- InfoType (Hospital, Pharmacy, Restaurant, Park)
- Name, Address, PhoneNumber
- Latitude, Longitude
- Notes
- IsVerified

---

## ?? Testing the API

### Using VS Code REST Client Extension
1. Install "REST Client" extension
2. Open `sample-api-requests.http`
3. Click "Send Request" on any request block
4. View responses in output panel

### Using Postman
1. Import the sample requests
2. Set base URL to `https://localhost:5001/api`
3. Create a new family profile
4. Copy the returned ID
5. Use ID in subsequent requests

### Using curl
```bash
curl -X POST https://localhost:5001/api/familyprofile \
  -H "Content-Type: application/json" \
  -d '{
    "destination": "Orlando, Florida",
    "travelStartDate": "2024-07-01",
    "travelEndDate": "2024-07-07",
    "numberOfKids": 2,
    "kidsAges": [2, 4],
    "napStartTime": "13:00",
    "napEndTime": "15:00",
    "hasStroller": true,
    "budgetPreference": "Medium"
  }'
```

---

## ?? Understanding the Smart Logic

### Itinerary Generator Logic
```
For each day in trip:
  1. Get youngest child age
  2. Calculate max outdoor activities (3 if ?3, 2 if <3)
  3. Filter activities by:
     - Child age compatibility
     - Stroller friendliness (if needed)
  4. Sort by: indoor first, then by walking distance
  5. Schedule activities:
     - Start at 9am
     - Check overlap with nap time
     - Check daily walking limit (4km)
     - Add 15-min buffers
     - Don't schedule after 7pm
  6. Save to database
```

### Packing Checklist Logic
```
For each child in profile:
  1. Calculate trip duration days
  2. For each item in master list:
     - Check if age matches (MinAgeRequired ? childAge ? MaxAgeApplicable)
     - If yes: quantity = quantityPerDay × tripDays
     - If no: skip item
  3. Consolidate duplicates (sum for multiple kids)
  4. Sort by category
  5. Save to database
```

---

## ?? Common Issues & Solutions

### Issue: "Database is locked"
**Solution**: Ensure only one instance is running. Stop the app and restart.

### Issue: Port 5001 already in use
**Solution**: Kill the process or specify a different port:
```bash
dotnet run --launch-profile https
```

### Issue: CORS errors from frontend
**Solution**: CORS is already configured in Program.cs. If issues persist, check cross-origin requests.

### Issue: Activities not showing in itinerary
**Solution**: 
1. Verify child ages match activity requirements
2. Check nap time overlap
3. Ensure stroller requirement is satisfied

---

## ?? Next Steps

1. **Test the 4 main features** with sample data
2. **Review the Feature Guide** (`FEATURE_GUIDE.md`) for detailed logic
3. **Check API Documentation** (`API_DOCUMENTATION.md`) for all endpoints
4. **Build frontend** (React/Vue) to consume these APIs
5. **Add more activities** to the database for specific destinations
6. **Expand safety data** for more destinations
7. **Deploy** to Azure, AWS, or your preferred hosting

---

## ?? Support

For issues or questions:
1. Check `FEATURE_GUIDE.md` for feature details
2. Check `API_DOCUMENTATION.md` for endpoint details
3. Review `sample-api-requests.http` for examples
4. Check build output for error messages

---

## ? Verification Checklist

- [ ] Project builds without errors
- [ ] Database created automatically (check for `app.db`)
- [ ] API responds on `https://localhost:5001`
- [ ] Can create family profile
- [ ] Can generate itinerary
- [ ] Can generate packing checklist
- [ ] Can retrieve safety information
- [ ] All CRUD operations work

**You're all set! Happy trip planning! ??**
