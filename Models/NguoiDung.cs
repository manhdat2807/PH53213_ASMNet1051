using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class NguoiDung : IdentityUser<int>
    {
        [Required]
        [StringLength(100)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(100)]
        public DateTime DOB { get; set; }

        [Required]
        public bool VaiTro { get; set; }

    }
}
