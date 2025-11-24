using System.ComponentModel.DataAnnotations;
using im_bored.Data;

namespace im_bored.Models;

public class UserSavedActivity
{
    [Required]
    public required string UserId { get; set; }
    [Required]
    public required ApplicationUser User { get; set; } = null!;
    [Required]
    public required int ActivityId { get; set; }
    [Required]
    public required Activity Activity { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; } = false;
}
