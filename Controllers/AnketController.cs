using BilsoftAnketPlatformu.Data;
using BilsoftAnketPlatformu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BilsoftAnketPlatformu.Controllers
{
    [Authorize]
    public class AnketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(AnketFilterViewModel filtre)
        {
            filtre.Sayfa = filtre.Sayfa < 1 ? 1 : filtre.Sayfa;
            var yetkiliPersoneller = await YetkiliPersonelIdleri();
            var kayitlar = await TemelListeSorgusu(filtre, yetkiliPersoneller);

            filtre.ToplamKayit = kayitlar.Count;
            filtre.Kayitlar = kayitlar
                .OrderByDescending(x => x.Tarih)
                .Skip((filtre.Sayfa - 1) * filtre.SayfaBoyutu)
                .Take(filtre.SayfaBoyutu)
                .ToList();

            await FiltreListeleriniDoldur(filtre, yetkiliPersoneller);
            return View(filtre);
        }

        public async Task<IActionResult> Detay(int servisNo, int anketID)
        {
            var yetkiliPersoneller = await YetkiliPersonelIdleri();

            var baslik = await (
                from c in _context.AnketCevaplar
                join a in _context.Anketler on c.AnketID equals a.AnketID into anketJoin
                from a in anketJoin.DefaultIfEmpty()
                join p in _context.Personeller on c.PersonelID equals p.PersonelID into personelJoin
                from p in personelJoin.DefaultIfEmpty()
                join m in _context.AnketMusterileri on new { ServisID = c.ServisNo, c.AnketID } equals new { m.ServisID, m.AnketID } into musteriJoin
                from m in musteriJoin.DefaultIfEmpty()
                where c.ServisNo == servisNo && c.AnketID == anketID
                select new
                {
                    c.ServisNo,
                    c.AnketID,
                    AnketAdi = a != null ? a.AnketAdi : "-",
                    Personel = p != null ? p.AdSoyad : "-",
                    c.PersonelID,
                    Durum = m != null ? (bool?)m.AnketDurumu : null,
                    Tarih = c.KayitTarih
                }).FirstOrDefaultAsync();

            if (baslik == null)
            {
                return NotFound();
            }

            if (yetkiliPersoneller != null && (!baslik.PersonelID.HasValue || !yetkiliPersoneller.Contains(baslik.PersonelID.Value)))
            {
                return Forbid();
            }

            var cevaplar = await (
                from c in _context.AnketCevaplar
                join s in _context.AnketSorular on c.SoruID equals s.SoruID into soruJoin
                from s in soruJoin.DefaultIfEmpty()
                where c.ServisNo == servisNo && c.AnketID == anketID
                orderby s != null ? s.SiraNo : 999, c.CevapID
                select new AnketDetayCevapViewModel
                {
                    SiraNo = s != null && s.SiraNo.HasValue ? s.SiraNo.Value : 0,
                    Soru = s != null ? s.Soru : "-",
                    Cevap = c.Cevap
                }).ToListAsync();

            var model = new AnketDetayViewModel
            {
                ServisNo = baslik.ServisNo,
                AnketID = baslik.AnketID,
                AnketAdi = baslik.AnketAdi,
                Personel = baslik.Personel,
                Durum = baslik.Durum,
                Tarih = baslik.Tarih,
                Cevaplar = cevaplar
            };

            return View(model);
        }

        private async Task<List<AnketListItemViewModel>> TemelListeSorgusu(AnketFilterViewModel filtre, List<int>? yetkiliPersoneller)
        {
            var cevaplar = _context.AnketCevaplar.AsQueryable();

            if (yetkiliPersoneller != null)
            {
                cevaplar = cevaplar.Where(x => x.PersonelID.HasValue && yetkiliPersoneller.Contains(x.PersonelID.Value));
            }

            if (filtre.Baslangic.HasValue)
            {
                cevaplar = cevaplar.Where(x => x.KayitTarih >= filtre.Baslangic.Value.Date);
            }

            if (filtre.Bitis.HasValue)
            {
                cevaplar = cevaplar.Where(x => x.KayitTarih < filtre.Bitis.Value.Date.AddDays(1));
            }

            if (filtre.PersonelID.HasValue)
            {
                cevaplar = cevaplar.Where(x => x.PersonelID == filtre.PersonelID.Value);
            }

            if (filtre.AnketID.HasValue)
            {
                cevaplar = cevaplar.Where(x => x.AnketID == filtre.AnketID.Value);
            }

            if (filtre.ServisNo.HasValue)
            {
                cevaplar = cevaplar.Where(x => x.ServisNo == filtre.ServisNo.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtre.Cevap))
            {
                cevaplar = cevaplar.Where(x => x.Cevap.Contains(filtre.Cevap));
            }

            var cevapListesi = await cevaplar.AsNoTracking().ToListAsync();
            var anketler = await _context.Anketler.AsNoTracking().ToDictionaryAsync(x => x.AnketID, x => x.AnketAdi);
            var personeller = await _context.Personeller.AsNoTracking().ToDictionaryAsync(x => x.PersonelID, x => x.AdSoyad);
            var musteriListesi = await _context.AnketMusterileri.AsNoTracking().ToListAsync();
            var musteriDurumlari = musteriListesi
                .GroupBy(x => new { x.ServisID, x.AnketID })
                .ToDictionary(x => (x.Key.ServisID, x.Key.AnketID), x => (bool?)x.Any(y => y.AnketDurumu));

            var liste = cevapListesi
                .GroupBy(x => new { x.ServisNo, x.AnketID })
                .Select(g =>
                {
                    var ilk = g.OrderByDescending(x => x.KayitTarih).First();
                    musteriDurumlari.TryGetValue((g.Key.ServisNo, g.Key.AnketID), out var durum);

                    return new AnketListItemViewModel
                    {
                        ServisNo = g.Key.ServisNo,
                        AnketID = g.Key.AnketID,
                        AnketAdi = anketler.TryGetValue(g.Key.AnketID, out var anketAdi) ? anketAdi : "-",
                        Personel = ilk.PersonelID.HasValue && personeller.TryGetValue(ilk.PersonelID.Value, out var personelAdi) ? personelAdi : "-",
                        Durum = durum,
                        Tarih = ilk.KayitTarih,
                        CevapSayisi = g.Count(),
                        SonCevap = ilk.Cevap
                    };
                })
                .Where(x => !filtre.Durum.HasValue || x.Durum == filtre.Durum.Value)
                .ToList();

            return liste;
        }

        private async Task<List<int>?> YetkiliPersonelIdleri()
        {
            if (User.IsInRole("Admin"))
            {
                return null;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.AppUserPersoneller
                .Where(x => x.UserId == userId)
                .Select(x => x.PersonelID)
                .ToListAsync();
        }

        private async Task FiltreListeleriniDoldur(AnketFilterViewModel filtre, List<int>? yetkiliPersoneller)
        {
            var personelQuery = _context.Personeller.AsQueryable();
            if (yetkiliPersoneller != null)
            {
                personelQuery = personelQuery.Where(x => yetkiliPersoneller.Contains(x.PersonelID));
            }

            filtre.Personeller = await personelQuery
                .OrderBy(x => x.AdSoyad)
                .Select(x => new SelectListItem(x.AdSoyad, x.PersonelID.ToString()))
                .ToListAsync();

            filtre.Anketler = await _context.Anketler
                .OrderBy(x => x.AnketAdi)
                .Select(x => new SelectListItem(x.AnketAdi, x.AnketID.ToString()))
                .ToListAsync();
        }
    }
}
