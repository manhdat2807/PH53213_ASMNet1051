using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string? Fullname { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string UserName { get; set; }
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
