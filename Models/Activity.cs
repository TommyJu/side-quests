using System.ComponentModel.DataAnnotations;
using im_bored.Data;

namespace im_bored.Models;

public class Activity
{
    [Key]
    [Required]
    public required int Id { get; set; }
    [Required]
    public required string Title { get; set; }
    [Required]
    public required ActivityType Type { get; set; }
    [Required]
    public required ActivityParticipants Participants { get; set; }
    [Required]
    public required ActivityPrice Price { get; set; }
    [Required]
    public required ActivityDuration ActivityDuration { get; set; }
    [Required]
    public required bool kidFriendly { get; set; }
    
    // Many to Many Relationship: list of users who have saved this activity
    public ICollection<UserSavedActivity> SavedByUsers { get; set; } = new List<UserSavedActivity>();
}

public enum ActivityType
{ 
    Recreational,
    Social,
    Education,
    Charity,
    Cooking,
    Relaxation,
    Music,
    DIY,
    Sports,
    Travel
}

public enum ActivityDuration
{
    Short,
    Medium,
    Long
}

public enum ActivityPrice
{
    Free,
    Low,
    Medium,
    High
}
public enum ActivityParticipants
{
    Solo,
    Group
}
