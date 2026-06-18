namespace BilsoftAnketPlatformu.Models
{
    public class AnketMusteri
    {
        public int ServisID { get; set; }
        public int? PersonelID { get; set; }
        public int AnketID { get; set; }
        public bool AnketDurumu { get; set; }
        public DateTime? AnketTarihKayit { get; set; }
    }
}
