# Happy Next Holidays MVP - Feature Guide

## Overview

The Happy Next Holidays MVP is a smart trip planner designed specifically for families traveling with toddlers (ages 1-5). The application uses **rule-based logic** to generate intelligent itineraries and checklists without requiring AI, making it cost-effective and reliable.

---

## Feature 1: Family Profile Setup

### Purpose
Capture essential family information to personalize the trip planning experience.

### What It Does
- Stores family travel preferences
- Captures kid-specific requirements (ages, nap schedules)
- Tracks equipment needs (stroller)
- Records budget constraints

### API Endpoints
- `POST /api/familyprofile` - Create new profile
- `GET /api/familyprofile/{id}` - Retrieve profile
- `PUT /api/familyprofile/{id}` - Update profile
- `DELETE /api/familyprofile/{id}` - Delete profile

### Data Stored
```
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

### Benefits
? Personalized trip planning
? Data persistence across sessions
? Easy profile management for multiple trips
? Foundation for all other features

---

## Feature 2: Smart Itinerary Generator

### Purpose
**Your Differentiator** - Automatically create age-appropriate daily itineraries that respect family constraints.

### Smart Logic (Rule-Based)

#### Rule 1: Age-Based Activity Limits
```
If: Child age < 3 years
Then: Maximum 2 outdoor activities per day
Why: Younger toddlers tire quickly and need more rest
```

#### Rule 2: Nap Time Protection
```
If: Family has nap schedule (1pm-3pm)
Then: No activities scheduled during this window
And: Suggest hotel/rest block in itinerary
Why: Consistent naps are critical for toddler behavior & health
```

#### Rule 3: Stroller Compatibility
```
If: hasStroller = false
Then: Filter out non-walker-friendly activities
Why: Some locations are difficult without stroller access
```

#### Rule 4: Daily Walking Distance Limit
```
If: Total daily walking > 4km
Then: Do not schedule that activity
Why: Toddlers are limited in walking capacity
```

#### Rule 5: Activity Sequencing
```
Priority Order:
1. Schedule indoor activities first (less weather dependent)
2. Then schedule outdoor activities
3. Ensure sufficient time between activities (15 min buffer)
4. Never schedule too late (all activities end by 7pm)
Why: Reduces stress, maintains flexibility, respects bedtime
```

### How It Works

1. **User generates itinerary** ? POST to `/api/itinerary/generate/{familyProfileId}`

2. **System analyzes:**
   - Child ages from profile
   - Nap schedule constraints
   - Stroller requirements
   - Available activities database

3. **System generates:** Day-by-day schedule with activities that:
   - Match all child age requirements
   - Fit within nap times
   - Respect stroller constraints
   - Keep walking distance reasonable

4. **System saves:** Each day's schedule with times and notes

### Example Output
```
Day 1 (July 1):
  9:00 AM - 10:00 AM : Park Play (outdoor, 0.5km walk)
  10:30 AM - 12:00 PM: Children's Museum (indoor, 0.2km walk)
  [1:00 PM - 3:00 PM: NAP TIME - Hotel rest]
  3:30 PM - 4:30 PM  : Soft Play Center (indoor, 0.1km walk)
  
Daily Stats:
  - Total Activities: 3
  - Outdoor Activities: 1
  - Total Walking: 0.8km
```

### Pre-Seeded Activities (Sample)
- Park Play (outdoor, toddler-friendly)
- Children's Museum (indoor, educational)
- Splash Pad (water play, summer fun)
- Zoo Visit (interactive, requires walking)
- Soft Play Center (indoor, high energy)
- Botanical Garden (peaceful, stroller-friendly)

### API Response
```json
{
  "familyProfileId": 1,
  "totalDays": 7,
  "days": [
    {
      "date": "2024-07-01",
      "scheduledActivities": [
        {
          "startTime": "09:00",
          "endTime": "10:00",
          "activity": {
            "name": "Park Play",
            "minAgeRequired": 1,
            "maxAgeRecommended": 5,
            "isIndoor": false,
            "walkingDistanceKm": 0.5
          }
        }
      ]
    }
  ]
}
```

### Benefits
? Time-saving (instant itineraries)
? Smart constraints prevent bad experiences
? Respects family's natural rhythms
? Age-appropriate activities only
? Reduces planning stress

---

## Feature 3: Packing Checklist Generator

### Purpose
Generate a complete, age-specific packing list automatically.

### Smart Logic

#### Smart Calculations
```
For each child age:
  - Find all applicable items
  - Calculate quantities based on:
    * Age (younger = more changes)
    * Trip duration (multiply by days)
    * Category (diapers, clothes, gear, etc.)

Combine duplicates:
  - If multiple kids need same item, sum quantities
  - Avoid duplicate entries
```

#### Age-Based Item Rules

**For children < 2 years:**
- Diapers: 8 per day × trip duration
- Extra clothes: 3 per day × trip duration
- Wipes: 1-2 packs
- Formula & bottles: Full supply
- Diaper rash cream
- Portable crib

**For children 2-3 years:**
- Diapers: 6-7 per day × trip duration
- Extra clothes: 2 per day × trip duration
- Sippy cups
- Medicines
- Portable crib (optional)

**For children 3-5 years:**
- Pull-ups: 4-6 per day × trip duration
- Extra clothes: 1-2 per day × trip duration
- More independence items
- Entertainment focus

**All ages (general):**
- Sunscreen (SPF 30+)
- Medicine kit (fever reducer, pain reliever, antihistamine)
- Thermometer
- Comfort items (blanket, lovey)
- Toys & books
- Nighttime clothes

### Categories
1. **Clothing** - Onesies, socks, daily clothes, pajamas
2. **Hygiene** - Diapers, wipes, creams
3. **Health** - Medicine kit, thermometer, sunscreen
4. **Feeding** - Bottles, formula, sippy cups, snacks
5. **Gear** - Stroller, car seat, portable crib
6. **Entertainment** - Toys, books
7. **Comfort** - Blankets, loveys

### Example Calculation
```
Family: 2 kids (age 2 and age 4), 7-day trip

Item: "Diapers"
- Kid 1 (age 2): 6/day × 7 days = 42 units
- Kid 2 (age 4): Not applicable
- Total: 42 units

Item: "Extra Clothes"
- Kid 1 (age 2): 2/day × 7 days = 14 sets
- Kid 2 (age 4): 2/day × 7 days = 14 sets
- Total: 28 sets

Item: "Medicine Kit"
- Both applicable: Count as 1 shared unit
- Total: 1 kit (but note "one per child preferred")
```

### API Response
```json
{
  "id": 1,
  "familyProfileId": 1,
  "generatedAt": "2024-06-15T10:30:00",
  "items": [
    {
      "itemName": "Diapers",
      "category": "Hygiene",
      "quantity": 42,
      "isPacked": false,
      "notes": "Approximately 6 per day for 2-year-old"
    },
    {
      "itemName": "Extra Clothes",
      "category": "Clothing",
      "quantity": 28,
      "isPacked": false,
      "notes": "2 sets per day for both kids"
    }
  ]
}
```

### Features
- ? Items automatically sorted by category
- ? Checkboxes to mark as packed
- ? Notes section for quantities & tips
- ? Regenerable (create new versions)
- ? Trip duration factored in

### Benefits
? Nothing forgotten
? Correct quantities calculated
? Age-specific recommendations
? Printable checklist format
? Trip duration aware

---

## Feature 4: Toddler Safety Panel

### Purpose
Quick access to critical safety information for the destination.

### Safety Information Types

#### 1. Hospitals & Clinics
- Pediatric emergency care
- After-hours services
- Insurance network info

#### 2. Pharmacies
- Hours of operation
- Children's medication availability
- Languages spoken

#### 3. Kid-Friendly Restaurants
- High chair availability
- Allergy-friendly options
- Menu for young children
- Booster seats

#### 4. Toddler Parks
- Playground equipment sizes
- Shade availability
- Bathroom facilities
- Water fountains

### Data Provided Per Location
```
{
  "name": "Orlando Health Arnold Palmer Hospital",
  "address": "92 W Miller St, Orlando, FL 32806",
  "phoneNumber": "(321) 841-5000",
  "latitude": 28.5412,
  "longitude": -81.3816,
  "notes": "Specialized children's hospital",
  "infoType": "Hospital"
}
```

### Pre-Seeded Destinations
**Orlando, Florida:**
- Arnold Palmer Hospital (pediatric specialty)
- CVS Pharmacy (24-hour)
- Baby-Friendly Bistro (high chairs, allergy options)
- Lake Eustis Park (toddler playground)

**New York, New York:**
- Mount Sinai Hospital (pediatric department)
- Central Park (multiple playgrounds)
- Kids' Corner Cafe (toddler menu)

**Los Angeles, California:**
- Children's Hospital LA (specialist center)
- Griffith Park (trails, playgrounds)

### API Endpoints

1. **Get Complete Safety Map**
   ```
   GET /api/safetymap/destination/{destination}
   
   Returns: All hospitals, restaurants, pharmacies, parks
   ```

2. **Get Specific Type**
   ```
   GET /api/safetymap/{destination}/{infoType}
   
   Types: Hospital, Restaurant, Pharmacy, Park
   ```

3. **Add New Location**
   ```
   POST /api/safetymap
   
   For community contributions or local updates
   ```

### Response Format
```json
{
  "destination": "Orlando, Florida",
  "hospitals": [...],
  "restaurants": [...],
  "pharmacies": [...],
  "parks": [...]
}
```

### Benefits
? Peace of mind
? Emergency preparedness
? Local knowledge without research
? GPS-ready locations
? Contact info at fingertips
? Community expandable

---

## Data Flow Diagram

```
Family Profile Created
         ?
    ??? Itinerary Generator (analyzes constraints, builds schedule)
    ??? Packing Checklist (calculates quantities, organizes items)
    ??? Safety Panel (shows destination resources)
         ?
   Trip Planning Complete
```

---

## MVP Scope

### ? Included in MVP
- Family profile CRUD operations
- Rule-based itinerary generation
- Smart packing checklist
- Safety information lookup
- REST API for all features
- SQLite database
- Pre-seeded activities & safety data

### ?? Future Enhancements (v2+)
- Google Maps integration (directions, ETA)
- Weather API (packing adjustments)
- Flight & hotel booking
- Community reviews & photos
- Expense tracker
- Mobile app
- Multi-language support
- Real-time collaboration

---

## Success Metrics

### User Experience
- ? Create profile: < 2 minutes
- ? Generate itinerary: < 5 seconds
- ? Generate checklist: < 2 seconds
- ? Find safety info: Instant

### Data Quality
- ? All packing items accounted for
- ? No nap time conflicts
- ? Age-appropriate activities only
- ? Walking distance respected

### MVP Validation
- ? Parents save time on planning
- ? Confidence in trip organization
- ? Peace of mind on safety
- ? Nothing forgotten
