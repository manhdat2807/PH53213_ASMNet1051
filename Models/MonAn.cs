using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH53213_ASMNet1051.Models
{
    public class MonAn
    {
        [Key]
        public int MaMonAn { get; set; }

        [Required]
        [StringLength(100)]
        public string? TenMonAn { get; set; }

        public string? MoTa { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaTien { get; set; }

        public byte[]? HinhAnhURL { get; set; }

        public int MaDanhMuc { get; set; }

        public bool TrangThai { get; set; }

        // Quan hệ với DanhMuc
        public virtual DanhMuc DanhMuc { get; set; }


        // Quan hệ với ChiTietDonHang
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

        // Quan hệ với ChiTietCombo
        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

    }
}
