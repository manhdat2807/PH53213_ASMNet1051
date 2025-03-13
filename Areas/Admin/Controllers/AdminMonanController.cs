using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;
using PH53213_ASMNet1051.ViewModels;

namespace PH53213_ASMNet1051.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMonanController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public AdminMonanController(ApplicationDbcontext context)
        {
            _context = context;
        }
        // GET: AdminMonanController
        public ActionResult Index(string? tenmonan, decimal? fromPrice, decimal? toPrice)
        {
            var food = _context.MonAns.AsQueryable();

            if (!string.IsNullOrEmpty(tenmonan))
            {
                food = food.Where(x => x.TenMonAn.Contains(tenmonan));
            }
            if (fromPrice.HasValue)
            {
                food = food.Where(p => p.GiaTien >= fromPrice.Value);
            }
            if (toPrice.HasValue)
            {
                food = food.Where(p => p.GiaTien <= toPrice.Value);
            }

            return View(food.ToList());
        }

        // Chi tiết món ăn
        public async Task<IActionResult> Details(int id)
        {
            var food = await _context.MonAns
                .Include(ma => ma.DanhMuc)
                .Where(ma => ma.MaMonAn == id)
                .Select(ma => new MonAnViewModel
                {
                    MaMonAn = ma.MaMonAn,
                    TenMonAn = ma.TenMonAn,
                    GiaTien = ma.GiaTien,
                    HinhAnhURL = ma.HinhAnhURL,
                    MoTa = ma.MoTa,
                    TenDanhMuc = ma.DanhMuc.TenDanhMuc,
                    TrangThai = ma.TrangThai
                }).FirstOrDefaultAsync();

            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DanhMucs = _context.danhMucs
                .Select(dm => new SelectListItem
                {
                    Value = dm.MaDanhMuc.ToString(),
                    Text = dm.TenDanhMuc
                }).ToList();

            return View();
        }

        // POST: Tạo món ăn mới
        [HttpPost]
        public IActionResult Create(MonAn mo, IFormFile uploadedFile)
        {
            // Xử lý upload hình ảnh
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    uploadedFile.CopyTo(memoryStream);
                    mo.HinhAnhURL = memoryStream.ToArray();
                }
            }

            _context.MonAns.Add(mo);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET: Edit MonAn
        public IActionResult Edit(int id)
        {
            var itemEdit = _context.MonAns.Find(id);
            if (itemEdit == null)
            {
                return NotFound();
            }
            ViewBag.DanhMucs = _context.danhMucs.ToList();
            return View(itemEdit);
        }

        // POST: Edit MonAn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MonAn mo, IFormFile uploadedFile)
        {
            var itemEdit = _context.MonAns.Find(mo.MaMonAn);
            if (itemEdit == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin món ăn
            itemEdit.TenMonAn = mo.TenMonAn;
            itemEdit.MoTa = mo.MoTa;
            itemEdit.GiaTien = mo.GiaTien;
            itemEdit.MaDanhMuc = mo.MaDanhMuc;
            itemEdit.TrangThai = mo.TrangThai;

            // Nếu có ảnh mới thì cập nhật
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    uploadedFile.CopyTo(memoryStream);
                    itemEdit.HinhAnhURL = memoryStream.ToArray(); // Lưu ảnh vào database dạng byte[]
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
