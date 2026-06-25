using BilsoftAnketPlatformu.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BilsoftAnketPlatformu.ViewModels
{
    public class AnketFilterViewModel
    {
        public DateTime? Baslangic { get; set; }
        public DateTime? Bitis { get; set; }
        public int? PersonelID { get; set; }
        public int? AnketID { get; set; }
        public int? ServisNo { get; set; }
        public bool? Durum { get; set; }
        public string? Cevap { get; set; }
        public int Sayfa { get; set; } = 1;
        public int ToplamKayit { get; set; }
        public int SayfaBoyutu { get; set; } = 20;
        public List<AnketListItemViewModel> Kayitlar { get; set; } = new();
        public List<SelectListItem> Personeller { get; set; } = new();
        public List<SelectListItem> Anketler { get; set; } = new();
    }
}
