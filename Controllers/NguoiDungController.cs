using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PH53213_ASMNet1051.Models;

namespace PH53213_ASMNet1051.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public NguoiDungController(ApplicationDbcontext context)
        {
            _context=context;
        }
        public IActionResult Profile()
        {
            var userId =  HttpContext.Session.GetInt32("id");
            if (userId == null)
            {

                return RedirectToAction("Login", "Account");
            }


            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public ActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("Login", "Account");
            var user = _context.NguoiDungs.Find(userId);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]

        public ActionResult EditProfile(NguoiDung updatedUser)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = _context.NguoiDungs.Find(userId);
            if (user == null) return NotFound();
            user.Fullname = updatedUser.Fullname;
            user.Email = updatedUser.Email;
            user.DOB = updatedUser.DOB;
            user.PhoneNumber = updatedUser.PhoneNumber;
            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction("Profile");
        }
    }
}
