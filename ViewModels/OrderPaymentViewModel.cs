using PH53213_ASMNet1051.Models;
using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.ViewModels
{
    public class OrderPaymentViewModel
    {
        public int MaMonAn { get; set; }

        public string TenMonAn { get; set; }

        public decimal GiaTien { get; set; }

        public byte[] HinhAnhURL { get; set; }

        public int SoLuong { get; set; }

        public decimal ThanhTien => GiaTien * SoLuong;

        public int MaPhuongThucTT { get; set; }

        public List<PhuongThucThanhToan> DanhSachPhuongThucTT { get; set; }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
