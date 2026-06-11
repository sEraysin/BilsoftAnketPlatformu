using Microsoft.AspNetCore.Identity;

namespace BilsoftAnketPlatformu.Models
{
    public class AppUser:IdentityUser
    {
        public string? AdSoyad { get; set; }
        public bool AktifMi { get; set; } = true;
    }
}
