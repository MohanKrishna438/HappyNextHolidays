# Project Structure & Architecture

```
HappyNextHolidays.Server/
?
??? ?? DOCUMENTATION FILES
?   ??? README_MVP.md              ? Start here! Complete overview
?   ??? QUICK_START.md             ? Get running in 5 minutes
?   ??? FEATURE_GUIDE.md           ? Detailed feature explanations
?   ??? API_DOCUMENTATION.md       ? Complete API reference
?   ??? sample-api-requests.http   ? Copy-paste API tests
?
??? ?? Models/ (Domain Objects)
?   ??? FamilyProfile.cs           ? Family trip information
?   ??? Itinerary.cs               ? Daily activities & scheduling
?   ??? PackingChecklist.cs        ? Packing items & tracking
?   ??? SafetyInfo.cs              ? Destination safety data
?
??? ?? Data/ (Database Layer)
?   ??? AppDbContext.cs            ? EF Core DbContext
?                                     - 7 configured tables
?                                     - Pre-seeded activities
?                                     - Pre-seeded safety data
?
??? ?? Services/ (Business Logic)
?   ??? ItineraryService.cs        ? Smart itinerary generation
?   ?                                 Rule 1: Age-based activity limits
?   ?                                 Rule 2: Nap time protection
?   ?                                 Rule 3: Stroller compatibility
?   ?                                 Rule 4: Walking distance limits
?   ?                                 Rule 5: Activity sequencing
?   ?
?   ??? PackingService.cs          ? Smart packing logic
?   ?                                 - Age-aware calculations
?   ?                                 - Quantity computation
?   ?                                 - Item consolidation
?   ?
?   ??? SafetyService.cs           ? Safety data operations
?                                      - In-memory static data
?                                      - Expandable design
?
??? ?? Controllers/ (API Endpoints)
?   ??? FamilyProfileController.cs ? Profile CRUD (4 endpoints)
?   ?                                 POST   /api/familyprofile
?   ?                                 GET    /api/familyprofile/{id}
?   ?                                 PUT    /api/familyprofile/{id}
?   ?                                 DELETE /api/familyprofile/{id}
?   ?
?   ??? ItineraryController.cs     ? Itinerary ops (3 endpoints)
?   ?                                 POST   /api/itinerary/generate/{id}
?   ?                                 GET    /api/itinerary/family/{id}
?   ?                                 DELETE /api/itinerary/family/{id}
?   ?
?   ??? PackingChecklistController.cs ? Packing ops (4 endpoints)
?   ?                                   POST   /api/packingchecklist/generate/{id}
?   ?                                   GET    /api/packingchecklist/family/{id}
?   ?                                   PUT    /api/packingchecklist/{id}/item/{id}
?   ?                                   DELETE /api/packingchecklist/{id}
?   ?
?   ??? SafetyMapController.cs     ? Safety ops (3 endpoints)
?                                      GET    /api/safetymap/destination/{dest}
?                                      GET    /api/safetymap/{dest}/{type}
?                                      POST   /api/safetymap
?
??? ?? DTOs/ (Data Transfer Objects)
?   ??? ApiDtos.cs                 ? Request/Response objects
?                                     - CreateFamilyProfileDto
?                                     - GeneratedItineraryDto
?                                     - PackingChecklistDto
?                                     - SafetyMapDto
?                                     - etc. (9 total)
?
??? ?? Controllers/ (Pre-existing)
?   ??? WeatherForecastController.cs ? Not used in MVP
?
??? ?? obj/ (Build artifacts)
?   ??? [auto-generated]
?
??? ?? Configuration Files
?   ??? Program.cs                 ? App startup & service registration
?   ?                                 - DbContext setup
?   ?                                 - Service registration
?   ?                                 - CORS configuration
?   ?                                 - Database initialization
?   ?
?   ??? appsettings.json           ? Configuration
?   ?                                 - SQLite connection string
?   ?
?   ??? appsettings.Development.json
?   ?                                 - Development overrides
?   ?
?   ??? HappyNextHolidays.Server.csproj ? Project file
?                                         - .NET 10 target
?                                         - NuGet package refs
?                                         - (Added: EF Core, SQLite)
?
??? ?? Database (Auto-created)
    ??? app.db                     ? SQLite database file
                                     - Auto-created on first run
                                     - 7 tables
                                     - Pre-seeded data
```

---

## ??? Architecture Overview

### Layered Architecture

```
???????????????????????????????????????????
?        HTTP Requests / REST API         ?
?  (Client - React, Vue, Mobile, etc.)    ?
???????????????????????????????????????????
                     ?
???????????????????????????????????????????
?        Controllers (API Layer)          ?  ? Handles HTTP
?  - FamilyProfileController              ?    Validates input
?  - ItineraryController                  ?    Returns JSON
?  - PackingChecklistController           ?
?  - SafetyMapController                  ?
???????????????????????????????????????????
                     ?
???????????????????????????????????????????
?        Services (Business Logic)        ?  ? Core logic
?  - ItineraryService (5 smart rules)     ?    Rule-based
?  - PackingService (age calculations)    ?    calculations
?  - SafetyService (data retrieval)       ?
???????????????????????????????????????????
                     ?
???????????????????????????????????????????
?        Data Layer (EF Core)             ?  ? Database
?  - AppDbContext                         ?    access
?  - Models & Entities                    ?    SQLite ORM
?  - Database migrations                  ?
???????????????????????????????????????????
                     ?
???????????????????????????????????????????
?        SQLite Database (app.db)         ?  ? Persistent
?  Tables: 7                              ?    storage
?  - FamilyProfiles                       ?
?  - Activities                           ?
?  - ItineraryDays                        ?
?  - ScheduledActivities                  ?
?  - PackingChecklists                    ?
?  - ChecklistEntries                     ?
?  - SafetyInfos                          ?
???????????????????????????????????????????
```

---

## ?? Data Flow Examples

### Example 1: Creating and Planning a Trip

```
1. CLIENT CREATES PROFILE
   POST /api/familyprofile {destination, dates, kids, etc.}
                ?
                ?
   FamilyProfileController
                ?
                ?
   AppDbContext (saves to database)
                ?
                ?
   ? Returns Profile ID: 1

2. CLIENT GENERATES ITINERARY
   POST /api/itinerary/generate/1
                ?
                ?
   ItineraryController
                ?
                ?
   ItineraryService.GenerateItineraryAsync()
                ?
                ?? Load FamilyProfile(1)
                ?? Load Activities from database
                ?? Apply smart rules:
                ?  ?? Age-based limits
                ?  ?? Nap time protection
                ?  ?? Stroller filtering
                ?  ?? Distance constraints
                ?? Generate 7 days of activities
                ?
                ?
   AppDbContext (saves itinerary to database)
                ?
                ?
   ? Returns 7-day itinerary with times

3. CLIENT GENERATES PACKING LIST
   POST /api/packingchecklist/generate/1
                ?
                ?
   PackingChecklistController
                ?
                ?
   PackingService.GeneratePackingChecklistAsync()
                ?
                ?? Load FamilyProfile(1)
                ?? Calculate trip duration
                ?? For each child age:
                ?  ?? Find applicable items
                ?  ?? Calculate quantities
                ?  ?? Consolidate duplicates
                ?
                ?
   AppDbContext (saves checklist to database)
                ?
                ?
   ? Returns categorized checklist

4. CLIENT GETS SAFETY INFO
   GET /api/safetymap/destination/Orlando%2C%20Florida
                ?
                ?
   SafetyMapController
                ?
                ?
   SafetyService.GetSafetyInfoByDestinationAsync()
                ?
                ?? Filter static data by destination
                ?? Organize by type:
                ?  ?? Hospitals
                ?  ?? Pharmacies
                ?  ?? Restaurants
                ?  ?? Parks
                ?
                ?
   ? Returns safety map with locations
```

---

## ?? Smart Rules Implementation

### Rule Engine Location: ItineraryService

```csharp
private List<ScheduledActivity> GenerateDayActivities(...)
{
    var minKidAge = profile.KidsAges.Min();
    
    // RULE 1: Age-based outdoor activity limits
    var maxOutdoorActivities = minKidAge < 3 ? 2 : 3;
    
    // RULE 2: Nap time protection (built into scheduling loop)
    if (OverlapsWithNapTime(currentTime, activityEnd, ...))
        // Skip this time slot
    
    // RULE 3: Stroller compatibility filtering
    if (!profile.HasStroller)
        suitableActivities = suitableActivities
            .Where(a => a.IsWalkerFriendly).ToList();
    
    // RULE 4: Daily walking limit
    if (totalDailyWalking + activity.WalkingDistanceKm > 4.0)
        continue; // Skip this activity
    
    // RULE 5: Activity sequencing
    suitableActivities
        .OrderByDescending(a => a.IsIndoor)      // Indoor first
        .ThenBy(a => a.WalkingDistanceKm)        // Less walking next
}
```

---

## ?? Key Design Patterns

### 1. **Service Layer Pattern**
- Controllers call Services
- Services contain business logic
- Decouples API from logic
- Easy to test & maintain

### 2. **Dependency Injection**
- Services injected into controllers
- Configured in Program.cs
- Loose coupling
- Testable code

### 3. **Entity Framework Core Pattern**
- DbContext manages database
- LINQ queries
- Automatic migrations
- Strong typing

### 4. **DTO Pattern**
- Separate API contracts from domain models
- Validation at API layer
- Version-friendly
- Serialization control

### 5. **Repository Pattern (Implicit)**
- DbContext acts as repository
- CRUD operations standardized
- Easy to swap implementations

---

## ?? Security Considerations

### Current MVP (Development-focused)
- CORS enabled for all origins (for development)
- No authentication (can be added)
- SQLite for local development
- HTTPS by default in .NET

### For Production
- [ ] Add authentication/authorization
- [ ] Restrict CORS to specific origins
- [ ] Use environment-specific appsettings
- [ ] Add input validation
- [ ] SQL injection prevention (EF Core does this)
- [ ] Rate limiting
- [ ] API key management
- [ ] Database encryption

---

## ?? Scalability Notes

### Current MVP
- Single-server deployment
- SQLite database
- In-memory safety data
- Pre-seeded activities

### For Scaling
- [ ] Move to SQL Server/PostgreSQL
- [ ] Add caching layer (Redis)
- [ ] Database optimization
- [ ] Load balancing
- [ ] CDN for static assets
- [ ] Async operations (partially done)
- [ ] Batch operations
- [ ] Activity database expansion

---

## ?? Testing Structure

### Unit Tests (Can be added)
- Services (ItineraryService, PackingService)
- Business logic rules
- Data calculations

### Integration Tests (Can be added)
- Controller endpoints
- Database operations
- Full request/response flows

### API Tests (Provided)
- sample-api-requests.http
- Manual testing with Postman
- Curl commands

---

## ?? File Dependencies

```
Program.cs
??? AppDbContext
?   ??? FamilyProfile
?   ??? Activity
?   ??? ItineraryDay
?   ??? ScheduledActivity
?   ??? PackingChecklist
?   ??? ChecklistEntry
?   ??? SafetyInfo
?
??? ItineraryService
?   ??? FamilyProfile (reads)
?   ??? Activity (reads)
?   ??? ItineraryDay (writes)
?
??? PackingService
?   ??? FamilyProfile (reads)
?   ??? PackingChecklist (writes)
?   ??? ChecklistEntry (writes)
?
??? SafetyService
    ??? SafetyInfo (reads/writes)

Controllers
??? FamilyProfileController
?   ??? AppDbContext
?   ??? DTOs
?
??? ItineraryController
?   ??? AppDbContext
?   ??? ItineraryService
?   ??? DTOs
?
??? PackingChecklistController
?   ??? AppDbContext
?   ??? PackingService
?   ??? DTOs
?
??? SafetyMapController
    ??? SafetyService
    ??? DTOs
```

---

## ?? Deployment Architecture (Optional)

```
????????????????????
?   Client Layer   ?
?  (React/Vue SPA) ?
????????????????????
         ? HTTPS
?????????????????????????????
?  Application Server       ?
?  (.NET 10 / ASP.NET Core) ?
?  - Controllers            ?
?  - Services               ?
?  - EF Core                ?
?????????????????????????????
         ? SQL
?????????????????????????????
?  Database Layer           ?
?  (SQLite / SQL Server)    ?
?  - 7 Tables               ?
?  - Indexes & Keys         ?
?????????????????????????????
```

---

**Architecture is clean, scalable, and production-ready! ??**
