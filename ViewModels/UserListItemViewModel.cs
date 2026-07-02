namespace BilsoftAnketPlatformu.ViewModels
{
    public class UserListItemViewModel
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AdSoyad { get; set; }
        public bool AktifMi { get; set; }
        public string Roller { get; set; } = "-";
        public string Personeller { get; set; } = "-";
    }
}
