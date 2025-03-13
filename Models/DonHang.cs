using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH53213_ASMNet1051.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public int Id { get; set; }

        public DateTime ThoiGianDat { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; }  // Đang xử lý, Đang giao, Đã giao

        public int MaPhuongThucTT { get; set; }

        // Thêm mối quan hệ với PhuongThucThanhToan
        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }

        // Thêm mối quan hệ với ChiTietDonHang (một đơn hàng có nhiều chi tiết đơn hàng)
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    }
}
