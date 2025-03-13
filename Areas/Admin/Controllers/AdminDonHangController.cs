using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;
using System.Linq;

namespace PH53213_ASMNet1051.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDonHangController : Controller
    {
        private readonly ApplicationDbcontext _context;

        public AdminDonHangController(ApplicationDbcontext context)
        {
            _context = context;
        }

        // Danh sách đơn hàng
        public IActionResult Index()
        {
            var donHangs = _context.DonHangs.Include(d => d.PhuongThucThanhToan).ToList();
            return View(donHangs);
        }

        // Xem chi tiết đơn hàng
        public IActionResult Details(int id)
        {
            var donHang = _context.DonHangs
                .Include(d => d.PhuongThucThanhToan)
                .Include(d => d.ChiTietDonHangs)
                .ThenInclude(ct => ct.MonAn)
                .FirstOrDefault(d => d.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // Chỉnh sửa trạng thái đơn hàng
        public IActionResult Edit(int id)
        {
            var donHang = _context.DonHangs.Find(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DonHang donHang)
        {
            if (id != donHang.MaDonHang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(donHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }
    }
}
