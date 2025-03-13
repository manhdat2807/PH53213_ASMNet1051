using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();

    }
}
