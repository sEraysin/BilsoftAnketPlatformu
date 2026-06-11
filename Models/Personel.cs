namespace BilsoftAnketPlatformu.Models
{
    public class Personel
    {
        public int PersonelID { get; set; }

        public string AdSoyad { get; set; } = null!;

        public bool AktifMi { get; set; } = true;

        public ICollection<AppUserPersonel> AppUserPersoneller { get; set; } = new List<AppUserPersonel>();
    }
}