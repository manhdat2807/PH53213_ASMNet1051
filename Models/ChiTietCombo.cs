using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PH53213_ASMNet1051.Models
{
    public class ChiTietCombo
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey("Combo")]
        public int MaCombo { get; set; }
        public virtual Combo Combo { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("MonAn")]
        public int MaMonAn { get; set; }
        public virtual MonAn MonAn { get; set; }

        public int SoLuong { get; set; }

    }
}
