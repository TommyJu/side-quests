using im_bored.Models;
using Microsoft.AspNetCore.Identity;

namespace im_bored.Data;

public class ApplicationUser : IdentityUser
{
    public int Points { get; set; } = 0;
    public string PostalCode { get; set; } = "";

    public ICollection<UserSavedActivity> SavedActivities { get; set; } = new List<UserSavedActivity>();
}

