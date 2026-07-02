using BilsoftAnketPlatformu.Models;
using BilsoftAnketPlatformu.Data;
using BilsoftAnketPlatformu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BilsoftAnketPlatformu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cevaplar = _context.AnketCevaplar.AsQueryable();
            var musteriler = _context.AnketMusterileri.AsQueryable();
            var adminMi = User.IsInRole("Admin");
            var yetkiliPersonelSayisi = await _context.Personeller.CountAsync();

            if (!adminMi)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var personeller = await _context.AppUserPersoneller
                    .Where(x => x.UserId == userId)
                    .Select(x => x.PersonelID)
                    .ToListAsync();

                cevaplar = cevaplar.Where(x => x.PersonelID.HasValue && personeller.Contains(x.PersonelID.Value));
                musteriler = musteriler.Where(x => x.PersonelID.HasValue && personeller.Contains(x.PersonelID.Value));
                yetkiliPersonelSayisi = personeller.Count;
            }

            var bugun = DateTime.Today;
            var cevapAnahtarlari = await cevaplar
                .Select(x => new { x.ServisNo, x.AnketID })
                .Distinct()
                .ToListAsync();
            var servisNolari = cevapAnahtarlari
                .Select(x => x.ServisNo)
                .Distinct()
                .ToList();
            var musteriListesi = await musteriler
                .Where(x => servisNolari.Contains(x.ServisID))
                .ToListAsync();
            var durumlar = musteriListesi
                .GroupBy(x => new { x.ServisID, x.AnketID })
                .ToDictionary(
                    x => (x.Key.ServisID, x.Key.AnketID),
                    x => x.Any(y => y.AnketDurumu));
            var tamamlanan = cevapAnahtarlari.Count(x =>
                durumlar.TryGetValue((x.ServisNo, x.AnketID), out var durum) && durum);

            var model = new DashboardViewModel
            {
                AdminMi = adminMi,
                ToplamCevap = await cevaplar.CountAsync(),
                BugunkuCevap = await cevaplar.CountAsync(x => x.KayitTarih >= bugun),
                Tamamlanan = tamamlanan,
                Bekleyen = cevapAnahtarlari.Count - tamamlanan,
                YetkiliPersonelSayisi = yetkiliPersonelSayisi
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
