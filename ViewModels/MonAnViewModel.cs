using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.ViewModels
{
    public class MonAnViewModel
    {
        [Key]
        public int MaMonAn { get; set; }

        [Required]
        [StringLength(100)]
        public string? TenMonAn { get; set; }

        public string? MoTa { get; set; }

        public decimal GiaTien { get; set; }

        public byte[]? HinhAnhURL { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; } = string.Empty;

        public bool TrangThai { get; set; }
    }
}
