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
    public required ActivityPrice Price { get; set; }
    [Required]
    public required ActivityDuration ActivityDuration { get; set; }
    public required bool kidFriendly { get; set; }
    
    // Relationship: list of users who have saved this activity
    // This is a many-to-many relationship, so we need to create a join table
    public ICollection<ApplicationUser> SavedByUsers { get; set; } = new List<ApplicationUser>();
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
