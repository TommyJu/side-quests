using System.Security.Claims;
using im_bored.Data;
using im_bored.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace im_bored.Services;

public class ActivityService(
    ApplicationDbContext _context,
    UserManager<ApplicationUser> _userManager)
{
    public const int POINTS_GAINED_PER_ACTIVITY = 5;
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

    private async Task<ApplicationUser?> GetUserWithActivitiesAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return null;

        _context.Attach(user);
        await _context.Entry(user)
            .Collection(u => u.Activities)
            .LoadAsync();

        return user;
    }

    /*
            * Save activity to the user's saved activities list
            * @param principal The ClaimsPrincipal of the user
            * @param activity The activity to save
            * @return Task representing the asynchronous operation

    */
    public async Task SaveActivityAsync(ClaimsPrincipal principal, Activity activity)
    {
        if (activity == null) return;

        var user = await GetUserWithActivitiesAsync(principal);
        if (user == null) return;

        if (!user.Activities.Any(a => a.Id == activity.Id))
        {
            user.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }
    }

    /*
                * Remove activity from the user's saved activities list
                * @param principal The ClaimsPrincipal of the user
                * @param activity The activity to remove
                * @return Task representing the asynchronous operation
    */
    public async Task RemoveActivityAsync(ClaimsPrincipal principal, Activity activity)
    {
        if (activity == null) return;
        var user = await GetUserWithActivitiesAsync(principal);
        if (user == null) return;

        var dbActivity = await _context.Activities.FindAsync(activity.Id);
        if (dbActivity == null) return;

        if (user.Activities.Any(a => a.Id == dbActivity.Id))
        {
            user.Activities.Remove(dbActivity);
            await _context.SaveChangesAsync();
        }
    }

    /*
            * Complete activity and gain points
            * @param principal The ClaimsPrincipal of the user
            * @param activity The activity to complete
            * @return Task representing the asynchronous operation
    */

    public async Task CompleteActivityAsync(ClaimsPrincipal principal, Activity activity)
    {
        if (activity == null) return;
        var user = await GetUserWithActivitiesAsync(principal);
        if (user == null) return;

        var dbActivity = await _context.Activities.FindAsync(activity.Id);
        if (dbActivity == null) return;

        if (user.Activities.Any(a => a.Id == dbActivity.Id))
        {
            user.Activities.Remove(dbActivity);
            user.Points += POINTS_GAINED_PER_ACTIVITY;
            await _context.SaveChangesAsync();
        }
    }

    /*
            * Get the list of saved activities for the user
            * @param principal The ClaimsPrincipal of the user
            * @return List of saved activities
    */
    public async Task<List<Activity>> GetSavedActivitiesAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return [];

        return await _context.Users
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Activities)
            .ToListAsync();
    }

} // end of class
