using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;
using PH53213_ASMNet1051.ViewModels;

namespace PH53213_ASMNet1051.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminComboController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public AdminComboController(ApplicationDbcontext context)
        {
            _context= context;
        }
        public IActionResult Index(string? tencombo, decimal? fromPrice, decimal? toPrice)
        {

            var combo = _context.Combos.AsQueryable();
            if (!string.IsNullOrEmpty(tencombo))
            {
                combo = combo.Where(x => x.TenCombo == tencombo);
            }
            if (fromPrice.HasValue)
            {
                combo = combo.Where(p => p.GiaTien >= fromPrice.Value);
            }

            // Lọc theo giá đến
            if (toPrice.HasValue)
            {
                combo = combo.Where(p => p.GiaTien <= toPrice.Value);
            }
            return View(combo.ToList());
        }
        public async Task<IActionResult> Details(int id)
        {
            var combovs = from cb in _context.Combos
            join ct in _context.ChiTietCombos on cb.MaCombo equals ct.MaCombo
                          join nn in _context.MonAns on ct.MaMonAn equals nn.MaMonAn
                          where cb.MaCombo == id // Lọc theo id của combo
                          group ct by new { cb.MaCombo, cb.TenCombo, cb.MoTa, cb.HinhAnhURL, cb.GiaTien } into comboGroup
                          select new ComboViewModel()
                          {
                              MaCombo = comboGroup.Key.MaCombo,
                              TenCombo = comboGroup.Key.TenCombo,
                              MoTa = comboGroup.Key.MoTa,
                              HinhAnhURL = comboGroup.Key.HinhAnhURL,
                              GiaTien = comboGroup.Key.GiaTien,
                              TenMonAn = comboGroup.Select(x => x.MonAn.TenMonAn).ToList(),
                              SoLuong = comboGroup.Sum(x => x.SoLuong)
                          };
            var combodetails = await combovs.FirstOrDefaultAsync(); // Lấy chi tiết combo theo id
            if (combodetails == null)
            {
                return NotFound();
            }

            return View(combodetails);
        }
        public IActionResult Create()
        {
            ViewBag.MonAns = _context.MonAns.ToList();
            return View();
        }

        // Thêm Combo (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Combo combo, IFormFile uploadedFile, List<int> MonAnIds, List<int> SoLuongs)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        uploadedFile.CopyTo(memoryStream);
                        combo.HinhAnhURL = memoryStream.ToArray();
                    }
                }

                _context.Combos.Add(combo);
                _context.SaveChanges();
                for (int i = 0; i < MonAnIds.Count; i++)
                {
                    var chiTietCombo = new ChiTietCombo
                    {
                        MaCombo = combo.MaCombo,
                        MaMonAn = MonAnIds[i],
                        SoLuong = SoLuongs[i]
                    };
                    _context.ChiTietCombos.Add(chiTietCombo);
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(combo);
        }
        public IActionResult Edit(int id)
        {
            var combo = _context.Combos
                .Include(c => c.ChiTietCombos)
                .FirstOrDefault(c => c.MaCombo == id);

            if (combo == null)
            {
                return NotFound();
            }

            ViewBag.MonAns = _context.MonAns.ToList();
            return View(combo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Combo combo, IFormFile uploadedFile, List<int> MonAnIds, List<int> SoLuongs)
        {
            var existingCombo = _context.Combos
                .Include(c => c.ChiTietCombos)
                .FirstOrDefault(c => c.MaCombo == combo.MaCombo);

            if (existingCombo == null)
            {
                return NotFound();
            }

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    uploadedFile.CopyTo(memoryStream);
                    existingCombo.HinhAnhURL = memoryStream.ToArray();
                }
            }

            existingCombo.TenCombo = combo.TenCombo;
            existingCombo.MoTa = combo.MoTa;
            existingCombo.GiaTien = combo.GiaTien;
            existingCombo.TrangThai = combo.TrangThai;

            // Xóa các món ăn cũ trong Combo
            _context.ChiTietCombos.RemoveRange(existingCombo.ChiTietCombos);
            _context.SaveChanges();

            // Thêm các món ăn mới vào Combo
            for (int i = 0; i < MonAnIds.Count; i++)
            {
                var chiTietCombo = new ChiTietCombo
                {
                    MaCombo = existingCombo.MaCombo,
                    MaMonAn = MonAnIds[i],
                    SoLuong = SoLuongs[i]
                };
                _context.ChiTietCombos.Add(chiTietCombo);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
