using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;
using PH53213_ASMNet1051.ViewModels;

namespace PH53213_ASMNet1051.Controllers
{
    public class MonAnController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public MonAnController(ApplicationDbcontext context)
        {
            _context = context;
        }
        public ActionResult Index(string? tenmonan, decimal? fromPrice, decimal? toPrice)
        {

            var food = _context.MonAns.AsQueryable();
            if (!string.IsNullOrEmpty(tenmonan))
            {
                food = food.Where(x => x.TenMonAn == tenmonan);
            }
            if (fromPrice.HasValue)
            {
                food = food.Where(p => p.GiaTien >= fromPrice.Value);
            }

            // Lọc theo giá đến
            if (toPrice.HasValue)
            {
                food = food.Where(p => p.GiaTien <= toPrice.Value);
            }
            return View(food.ToList());
        }
        public IActionResult Details(int id)
        {
            var query = from ma in _context.MonAns
                        join dm in _context.danhMucs on ma.MaDanhMuc equals dm.MaDanhMuc
                        where ma.MaMonAn == id
                        select new MonAnViewModel()
                        {
                            TenMonAn = ma.TenMonAn,
                            GiaTien = ma.GiaTien,
                            HinhAnhURL = ma.HinhAnhURL,
                            MoTa = ma.MoTa,
                            TenDanhMuc = dm.TenDanhMuc,
                            TrangThai = ma.TrangThai
                        };

            var food = query.FirstOrDefault();
            return View(food);
        }
     

        public IActionResult OrderNow(int id)
        {
            var acc = HttpContext.Session.GetInt32("id");
            if (acc == null)
            {
                TempData["mess"] = "Bạn cần đăng nhập trước khi đặt hàng.";
                return RedirectToAction("Login", "Account");
            }

            var monAn = _context.MonAns.FirstOrDefault(m => m.MaMonAn == id);
            if (monAn == null)
            {
                return NotFound("Món ăn không tồn tại.");
            }

            var viewModel = new OrderPaymentViewModel
            {
                MaMonAn = monAn.MaMonAn,
                TenMonAn = monAn.TenMonAn,
                GiaTien = monAn.GiaTien,
                HinhAnhURL = monAn.HinhAnhURL,
                SoLuong = 1,
                DanhSachPhuongThucTT = _context.PTTTs.ToList()
            };

            return View(viewModel);
        }

        // Xử lý thanh toán
        [HttpPost]
        public IActionResult OrderNow(OrderPaymentViewModel model)
        {
            var acc = HttpContext.Session.GetInt32("id");
            if (acc == null)
            {
                TempData["mess"] = "Bạn cần đăng nhập trước khi đặt hàng.";
                return RedirectToAction("Login", "Account");
            }

            var monAn = _context.MonAns.FirstOrDefault(m => m.MaMonAn == model.MaMonAn);
            if (monAn == null)
            {
                return NotFound("Món ăn không tồn tại.");
            }

            // Tạo đơn hàng
            var donHang = new DonHang
            {
                Id = acc.Value,
                ThoiGianDat = DateTime.Now,
                TongTien = monAn.GiaTien * model.SoLuong,
                TrangThai = "Đang xử lý",
                MaPhuongThucTT = model.MaPhuongThucTT
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            // Tạo chi tiết đơn hàng
            var chiTietDonHang = new ChiTietDonHang
            {
                MaDonHang = donHang.MaDonHang,
                MaMonAn = monAn.MaMonAn,
                SoLuong = model.SoLuong,
                GiaTien = monAn.GiaTien * model.SoLuong
            };

            _context.ctDonHangs.Add(chiTietDonHang);
            _context.SaveChanges();

            model.IsSuccess = true;
            model.Message = "Đặt hàng thành công!";
            return View(model);
        }
        public ActionResult OrderHistory()
        {
            var acc = HttpContext.Session.GetInt32("id");

            // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            if (acc == null)
            {
                TempData["mess"] = "Bạn cần đăng nhập để xem lịch sử đặt hàng.";
                return RedirectToAction("Login", "Account");
            }

            var getAcc = _context.NguoiDungs.FirstOrDefault(x => x.Id == acc);
            if (getAcc == null)
            {
                return Content("Tài khoản không tồn tại.");
            }

            // Lấy danh sách đơn hàng của người dùng
            var orderHistory = _context.DonHangs
                .Where(x => x.Id == getAcc.Id)  // Không cần chuyển sang string
                .Include(x => x.ChiTietDonHangs)
                .ToList();

            return View(orderHistory);
        }


    }
}

