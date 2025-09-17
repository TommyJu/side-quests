using System;
using System.ComponentModel.DataAnnotations;
using im_bored.Data;

namespace im_bored.Models;

// Represents a join table for the (M:N) relationship between Activity and ApplicationUser entities.
public class UserSavedActivity
{
    // Composite key (User + Activity)
    [Required]
    public required string UserId { get; set; }
    [Required]
    public required ApplicationUser User { get; set; } = null!;
    [Required]
    public required int ActivityId { get; set; }
    [Required]
    public required Activity Activity { get; set; } = null!;

    // Additional column
    public string Description { get; set; } = string.Empty;
}
