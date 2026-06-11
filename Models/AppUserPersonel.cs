namespace BilsoftAnketPlatformu.Models
{
    public class AppUserPersonel
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public AppUser User { get; set; } = null!;

        public int PersonelID { get; set; }

        public Personel Personel { get; set; } = null!;
    }
}