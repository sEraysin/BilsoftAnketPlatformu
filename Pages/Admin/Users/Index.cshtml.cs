using BilsoftAnketPlatformu.Data;
using BilsoftAnketPlatformu.Models;
using BilsoftAnketPlatformu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BilsoftAnketPlatformu.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<UserListItemViewModel> Kullanicilar { get; set; } = new();
        public List<SelectListItem> Roller { get; set; } = new();
        public List<SelectListItem> Personeller { get; set; } = new();

        [BindProperty]
        public CreateUserInput YeniKullanici { get; set; } = new();

        [TempData]
        public string? Mesaj { get; set; }

        public async Task OnGetAsync()
        {
            await SayfayiHazirla();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await SayfayiHazirla();
                return Page();
            }

            var rolVarMi = await _context.Roles.AnyAsync(x => x.Name == YeniKullanici.Rol);
            if (!rolVarMi)
            {
                ModelState.AddModelError(string.Empty, "Geçerli bir rol seçiniz.");
            }

            if (YeniKullanici.PersonelID.HasValue)
            {
                var personelVarMi = await _context.Personeller
                    .AnyAsync(x => x.PersonelID == YeniKullanici.PersonelID.Value);
                if (!personelVarMi)
                {
                    ModelState.AddModelError(string.Empty, "Seçilen personel bulunamadı.");
                }
            }

            if (!ModelState.IsValid)
            {
                await SayfayiHazirla();
                return Page();
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var user = new AppUser
            {
                UserName = YeniKullanici.Email,
                Email = YeniKullanici.Email,
                EmailConfirmed = true,
                AdSoyad = YeniKullanici.AdSoyad,
                AktifMi = true,
                LockoutEnabled = true
            };

            var result = await _userManager.CreateAsync(user, YeniKullanici.Sifre);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await SayfayiHazirla();
                return Page();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, YeniKullanici.Rol);
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await SayfayiHazirla();
                return Page();
            }

            if (YeniKullanici.PersonelID.HasValue)
            {
                _context.AppUserPersoneller.Add(new AppUserPersonel
                {
                    UserId = user.Id,
                    PersonelID = YeniKullanici.PersonelID.Value
                });
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            Mesaj = "Kullanıcı eklendi.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Mesaj = "Kullanıcı bulunamadı.";
                return RedirectToPage();
            }

            if (user.Id == _userManager.GetUserId(User) && user.AktifMi)
            {
                Mesaj = "Kendi admin hesabınızı pasif yapamazsınız.";
                return RedirectToPage();
            }

            user.AktifMi = !user.AktifMi;
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                Mesaj = "Kullanıcı durumu güncellendi.";
            }
            else
            {
                Mesaj = "Kullanıcı durumu güncellenemedi.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAssignPersonelAsync(string id, int personelID)
        {
            var userVarMi = await _userManager.FindByIdAsync(id) != null;
            var personelVarMi = await _context.Personeller.AnyAsync(x => x.PersonelID == personelID);
            if (!userVarMi || !personelVarMi)
            {
                Mesaj = "Kullanıcı veya personel bulunamadı.";
                return RedirectToPage();
            }

            var varMi = await _context.AppUserPersoneller
                .AnyAsync(x => x.UserId == id && x.PersonelID == personelID);

            if (varMi)
            {
                Mesaj = "Bu personel kullanıcıya zaten bağlı.";
            }
            else
            {
                _context.AppUserPersoneller.Add(new AppUserPersonel
                {
                    UserId = id,
                    PersonelID = personelID
                });
                await _context.SaveChangesAsync();
                Mesaj = "Personel eşleştirmesi eklendi.";
            }

            return RedirectToPage();
        }

        private async Task SayfayiHazirla()
        {
            var users = await _userManager.Users.OrderBy(x => x.Email).ToListAsync();
            Kullanicilar = new List<UserListItemViewModel>();

            foreach (var user in users)
            {
                var roller = await _userManager.GetRolesAsync(user);
                var personeller = await _context.AppUserPersoneller
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.Personel)
                    .Select(x => x.Personel.AdSoyad)
                    .ToListAsync();

                Kullanicilar.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "-",
                    AdSoyad = user.AdSoyad,
                    AktifMi = user.AktifMi,
                    Roller = roller.Any() ? string.Join(", ", roller) : "-",
                    Personeller = personeller.Any() ? string.Join(", ", personeller) : "-"
                });
            }

            Roller = await _context.Roles
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem(x.Name!, x.Name!))
                .ToListAsync();

            Personeller = await _context.Personeller
                .OrderBy(x => x.AdSoyad)
                .Select(x => new SelectListItem(x.AdSoyad, x.PersonelID.ToString()))
                .ToListAsync();
        }

        public class CreateUserInput
        {
            [Required]
            public string AdSoyad { get; set; } = null!;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = null!;

            [Required]
            [MinLength(6)]
            public string Sifre { get; set; } = null!;

            [Required]
            public string Rol { get; set; } = "Kullanici";

            public int? PersonelID { get; set; }
        }
    }
}
