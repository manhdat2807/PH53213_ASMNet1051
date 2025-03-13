using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PH53213_ASMNet1051.Models;
using PH53213_ASMNet1051.ViewModels;

namespace PH53213_ASMNet1051.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<NguoiDung> userManager,
                                 SignInManager<NguoiDung> signInManager,
                                 ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra xem Email đã tồn tại chưa
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                return View(model);
            }

            // Tạo người dùng mới
            var user = new NguoiDung
            {
                Fullname = model.Fullname,
                UserName = model.UserName,
                DOB = model.DOB,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                VaiTro = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError("Đăng ký thất bại: " + error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    HttpContext.Session.SetInt32("id", user.Id);

                    if (!user.VaiTro) // Nếu VaiTro = false (admin)
                    {
                        return RedirectToAction("Index", "AdminMonAn", new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Mật khẩu không đúng!");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại!");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Remove("id");
            return RedirectToAction("Login", "Account");
        }
    }
}
