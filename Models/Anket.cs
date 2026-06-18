namespace BilsoftAnketPlatformu.Models
{
    public class Anket
    {
        public int AnketID { get; set; }
        public string AnketAdi { get; set; } = null!;
        public string? Aciklama { get; set; }
    }
}
