using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class ChiTietDonHang
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("DonHang")]
        public int MaDonHang { get; set; }
        public virtual DonHang? DonHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("MonAn")]
        public int MaMonAn { get; set; }
        public virtual MonAn? MonAn { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaTien { get; set; }

    }
}
