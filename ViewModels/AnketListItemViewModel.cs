namespace BilsoftAnketPlatformu.ViewModels
{
    public class AnketListItemViewModel
    {
        public int ServisNo { get; set; }
        public int AnketID { get; set; }
        public string AnketAdi { get; set; } = "-";
        public string Personel { get; set; } = "-";
        public bool? Durum { get; set; }
        public DateTime? Tarih { get; set; }
        public int CevapSayisi { get; set; }
        public string? SonCevap { get; set; }
    }
}
