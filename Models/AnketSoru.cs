namespace BilsoftAnketPlatformu.Models
{
    public class AnketSoru
    {
        public int SoruID { get; set; }
        public int SoruTipi { get; set; }
        public string Soru { get; set; } = null!;
        public int AnketID { get; set; }
        public int? SiraNo { get; set; }
    }
}
