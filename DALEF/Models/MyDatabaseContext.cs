using Microsoft.EntityFrameworkCore;

namespace DALEF.Models
{
    public partial class MyDatabaseContext : DbContext
    {
        // Конструктор, який приймає параметри DbContextOptions
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
        }

        // Додатковий конструктор, якщо потрібно
        public MyDatabaseContext(string connectionString)
            : base(new DbContextOptionsBuilder<MyDatabaseContext>()
                .UseSqlServer(connectionString)
                .Options)
        {
        }

        public virtual DbSet<TblCategory> TblCategories { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.HasKey(e => e.category_id);
                entity.ToTable("categories");

                entity.Property(e => e.category_name)
                    .HasMaxLength(100).HasDefaultValue("entity")
                    .IsRequired();

                entity.Property(e => e.category_description)
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.product_id);
                entity.ToTable("products");

                entity.Property(e => e.product_name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.price)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();

                entity.Property(e => e.quantity)
                    .HasDefaultValue(0);

                entity.HasOne(d => d.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(d => d.category_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_products_categories");

                entity.HasOne(d => d.User)
                    .WithMany(u => u.Products)
                    .HasForeignKey(d => d.user_id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_products_users");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.user_id);
                entity.ToTable("users");

                entity.Property(e => e.user_name)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.user_password)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.role)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

