namespace BilsoftAnketPlatformu.Models
{
    public class AnketCevap
    {
        public int CevapID { get; set; }
        public int ServisNo { get; set; }
        public int? PersonelID { get; set; }
        public int AnketID { get; set; }
        public int SoruID { get; set; }
        public int SoruTipi { get; set; }
        public string Cevap { get; set; } = null!;
        public DateTime? KayitTarih { get; set; }
    }
}
