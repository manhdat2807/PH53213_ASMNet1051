using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class Combo
    {
        [Key]
        public int MaCombo { get; set; }

        [Required]
        [StringLength(100)]
        public string TenCombo { get; set; } = string.Empty;

        public string? MoTa { get; set; }
        public byte[]? HinhAnhURL { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaTien { get; set; }

        public bool TrangThai { get; set; } = true;

        public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

    }
}
