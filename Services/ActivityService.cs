using System;
using im_bored.Data;
using im_bored.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace im_bored.Services;

public class ActivityService(ApplicationDbContext _context, AuthenticationStateProvider _authenticationStateProvider)
{
    // Select random activity from the database
    public async Task<Activity> GetRandomActivity()
    {
        var activities = await _context.Activities.ToListAsync();
        var random = new Random();
        var randomIndex = random.Next(activities.Count);
        return activities[randomIndex];
    }

    // Select random activity from the database with filters
    // Filters: type, participants, price, duration, kidFriendly
    public async Task<Activity?> GetFilteredRandomActivity(
    ActivityType? type = null,
    ActivityParticipants? participants = null,
    ActivityPrice? price = null,
    ActivityDuration? duration = null,
    bool? kidFriendly = null)
{
    var query = _context.Activities.AsQueryable();

    if (type.HasValue) query = query.Where(a => a.Type == type.Value);
    if (participants.HasValue) query = query.Where(a => a.Participants == participants.Value);
    if (price.HasValue) query = query.Where(a => a.Price == price.Value);
    if (duration.HasValue) query = query.Where(a => a.ActivityDuration == duration.Value);
    if (kidFriendly.HasValue) query = query.Where(a => a.kidFriendly == kidFriendly.Value);

    var activities = await query.ToListAsync();
    if (activities.Count == 0) return null;

    var random = new Random();
    return activities[random.Next(activities.Count)];
}
}
