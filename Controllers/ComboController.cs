using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;
using PH53213_ASMNet1051.ViewModels;

namespace PH53213_ASMNet1051.Controllers
{
    public class ComboController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public ComboController(ApplicationDbcontext context)
        {
            _context = context;
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

    }
}
