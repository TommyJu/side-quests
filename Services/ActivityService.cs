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
}
