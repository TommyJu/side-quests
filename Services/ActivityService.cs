using im_bored.Data;
using im_bored.Models;
using Microsoft.EntityFrameworkCore;

namespace im_bored.Services;

/// <summary>
/// Represents the business logic and related functionality of activities.
/// </summary>
/// <param name="_context">The database context for database operations.</param>
public class ActivityService(ApplicationDbContext _context)
{

    /// <summary>
    /// Indicates the number of points gained for each completed quest or activity.
    /// </summary>
    public const int POINTS_GAINED_PER_ACTIVITY = 10;


    /// <summary>
    /// Chooses a random activity given a list of activities
    /// </summary>
    /// <param name="activities">Represents a list of activities</param>
    /// <returns>Returns an activity or null if no activites are given.</returns>
    private Activity? ChooseRandomActivity(List<Activity> activities)
    {
        if (activities.Count == 0) return null;

        var random = new Random();
        return activities[random.Next(activities.Count)];
    }


    /// <summary>
    /// Gets a random activity from all activities stored in the database, after applying search filters.
    /// </summary>
    /// <param name="type">Represents the type of activity.</param>
    /// <param name="participants">Represents the number of participants for an activity.</param>
    /// <param name="price">Represents the cost of an activity.</param>
    /// <param name="duration">Represents the duration of an activity.</param>
    /// <param name="kidFriendly">Represents whether the activity is appropriate for kids.</param>
    /// <returns>Returns a random activity that matches all filters.</returns>
    public async Task<Activity?> GetFilteredRandomActivity(
    ApplicationUser currentUser,
    ActivityType? type = null,
    ActivityParticipants? participants = null,
    ActivityPrice? price = null,
    ActivityDuration? duration = null,
    bool? kidFriendly = null)
    {
        // Apply each filter to all activities.
        var query = _context.Activities.AsQueryable();
        if (type.HasValue) query = query.Where(a => a.Type == type.Value);
        if (participants.HasValue) query = query.Where(a => a.Participants == participants.Value);
        if (price.HasValue) query = query.Where(a => a.Price == price.Value);
        if (duration.HasValue) query = query.Where(a => a.ActivityDuration == duration.Value);
        if (kidFriendly.HasValue) query = query.Where(a => a.kidFriendly == kidFriendly.Value);
        
        // Prevent loading an already saved activity
        await LoadUserSavedActivities(currentUser);
        var savedActivityIds = currentUser.SavedActivities.Select(usa => usa.ActivityId).ToList();
        query = query.Where(a => !savedActivityIds.Contains(a.Id));

        var activities = await query.ToListAsync();
        return ChooseRandomActivity(activities);
    }


    /// <summary>
    /// Loads the user's saved activities from the database.
    /// </summary>
    /// <param name="currentUser">The user that is currently authenticated.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if the current user is null.</exception>
    private async Task LoadUserSavedActivities(ApplicationUser currentUser)
    {
        if (currentUser == null) throw new ArgumentException("The current user cannot be null.");

        _context.Attach(currentUser);
        await _context.Entry(currentUser)
            .Collection(u => u.SavedActivities)
            .LoadAsync();
    }


    /// <summary>
    /// Saves a given activity to the user's saved activities.
    /// </summary>
    /// <param name="currentUser">The current user that is saving the activity.</param>
    /// <param name="activity">The activity to be saved.</param>
    /// <param name="description">The curated description for the saved activity.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if the activity is null.</exception>
    public async Task SaveActivityAsync(
        ApplicationUser currentUser,
        Activity activity,
        string description)
    {
        if (activity == null) throw new ArgumentException("The current activity cannot be null.");
        await LoadUserSavedActivities(currentUser);

        UserSavedActivity savedActivity = new UserSavedActivity
        {
            UserId = currentUser.Id,
            User = currentUser,
            ActivityId = activity.Id,
            Activity = activity,
            Description = description
        };
        currentUser.SavedActivities.Add(savedActivity);
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Removes the activity from the user's saved activities.
    /// </summary>
    /// <param name="currentUser">The current user who is removing the activity.</param>
    /// <param name="savedActivity">The saved activity to be removed.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if the activity is null.</exception>
    public async Task RemoveActivityAsync(ApplicationUser currentUser, UserSavedActivity savedActivity)
    {
        if (savedActivity == null) throw new ArgumentException("The saved activity cannot be null.");
        await LoadUserSavedActivities(currentUser);

        currentUser.SavedActivities.Remove(savedActivity);
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Marks the user's saved activity as complete.
    /// </summary>
    /// <param name="currentUser">The current user who is completing the activity.</param>
    /// <param name="activity">The activity to be completed.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if the activity is null.</exception>
    public async Task CompleteActivityAsync(ApplicationUser currentUser, Activity activity)
    {
        if (activity == null) throw new ArgumentException("The current activity cannot be null.");
        await LoadUserSavedActivities(currentUser);

        UserSavedActivity? savedActivity = currentUser.SavedActivities.FirstOrDefault(usa => usa.ActivityId == activity.Id);
        if (savedActivity != null)
        {
            savedActivity.IsComplete = true;
            currentUser.Points += POINTS_GAINED_PER_ACTIVITY;
            await _context.SaveChangesAsync();
        }
    }


    /// <summary>
    /// Gets the user's saved activities
    /// </summary>
    /// <param name="currentUser">The current user to retrieve saved activites from.</param>
    /// <returns></returns>
    public async Task<List<UserSavedActivity>> GetSavedActivitiesAsync(ApplicationUser currentUser)
    {
        await LoadUserSavedActivities(currentUser);

        return await _context.UserSavedActivities
            .Where(usa => usa.UserId == currentUser.Id)
            .Include(usa => usa.Activity)
            .ToListAsync();
    }

} // end of class
