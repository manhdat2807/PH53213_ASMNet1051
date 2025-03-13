using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PH53213_ASMNet1051.Models
{
    public class ApplicationDbcontext : IdentityDbContext<
        NguoiDung,
        IdentityRole<int>,
        int,
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>> // ✅ Cấu hình đầy đủ Identity
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<MonAn> MonAns { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ChiTietCombo> ChiTietCombos { get; set; }
        public DbSet<PhuongThucThanhToan> PTTTs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ctDonHangs { get; set; }
        public DbSet<DanhMuc> danhMucs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình khóa chính cho IdentityUserLogin<int>
            modelBuilder.Entity<IdentityUserLogin<int>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });

            // Cấu hình khóa chính cho IdentityUserRole<int>
            modelBuilder.Entity<IdentityUserRole<int>>().HasKey(x => new { x.UserId, x.RoleId });

            // Cấu hình khóa chính cho IdentityUserToken<int>
            modelBuilder.Entity<IdentityUserToken<int>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            modelBuilder.Entity<ChiTietCombo>()
                .HasKey(cc => new { cc.MaCombo, cc.MaMonAn });

            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(cdh => new { cdh.MaDonHang, cdh.MaMonAn });


            modelBuilder.Entity<NguoiDung>()
                .Property(nd => nd.VaiTro)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<DonHang>()
                .HasOne(dh => dh.PhuongThucThanhToan)
                .WithMany(pt => pt.DonHangs)
                .HasForeignKey(dh => dh.MaPhuongThucTT)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonAn>()
                .HasOne(ma => ma.DanhMuc)
                .WithMany(dm => dm.MonAns)
                .HasForeignKey(ma => ma.MaDanhMuc);
        }
    }
}
