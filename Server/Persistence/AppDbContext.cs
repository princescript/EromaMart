using Microsoft.EntityFrameworkCore;
using Server.Entities;
namespace Server.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {      
        }
        public DbSet<UserMaster> DbUserMaster { get; set; }
        public DbSet<ProductMaster> DbProductMaster { get; set; }
        public DbSet<CategoryMaster> DbCategorieMaster { get; set; }
        public DbSet<BrandMaster> DbBrandMaster { get; set; }
        public DbSet<ProductImageTran> DbProductImageTran { get; set; }
        public DbSet<InventoryMaster> DbInventoryMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.ToTable("mst_user");
                entity.HasKey(x=>x.user_id);
                entity.Property(x => x.user_id)
                      .HasColumnName("user_id")
                      .ValueGeneratedOnAdd();

            });
            modelBuilder.Entity<ProductMaster>(entity =>
            {
                entity.ToTable("mst_product");
                entity.HasKey(x => x.product_id);
                entity.Property(x => x.product_id)
                      .HasColumnName("product_id")
                      .ValueGeneratedOnAdd();
                entity.Property(x => x.sku)
                      .HasColumnName("sku")
                      .IsRequired()
                      .HasMaxLength(50);
                entity.HasIndex(x => x.sku)
                      .IsUnique();
            });
            modelBuilder.Entity<CategoryMaster>(entity =>
            {
                entity.ToTable("mst_category");
                entity.HasKey(x => x.category_id);
                entity.Property(x => x.category_id)
                      .HasColumnName("category_id")
                      .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<BrandMaster>(entity =>
            {
                entity.ToTable("mst_brand");
                entity.HasKey(x => x.brand_id);
                entity.Property(x => x.brand_id)
                      .HasColumnName("brand_id")
                      .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<ProductImageTran>(entity =>
            {
                entity.ToTable("tran_product_image");
                entity.HasKey(x => x.image_id);
                entity.Property(x => x.image_id)
                      .HasColumnName("image_id")
                      .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<InventoryMaster>(entity =>
            {
                entity.ToTable("mst_inventory");
                entity.HasKey(x => x.inventory_id);
                entity.Property(x => x.inventory_id)
                      .HasColumnName("inventory_id")
                      .ValueGeneratedOnAdd();
            });
        }
    }
}
