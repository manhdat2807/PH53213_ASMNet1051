using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class PhuongThucThanhToan
    {
        [Key]
        public int MaPhuongThucTT { get; set; }

        [Required]
        [StringLength(100)]
        public string TenPhuongThuc { get; set; }

        public string? MoTa { get; set; }

        // Thêm mối quan hệ một-nhiều với DonHang
        public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    }
}
