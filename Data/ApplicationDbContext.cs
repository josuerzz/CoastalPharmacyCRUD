using Microsoft.EntityFrameworkCore;
using CoastalPharmacyCRUD.Models;

namespace CoastalPharmacyCRUD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CDL_Identifier> CDL_Identifiers { get; set; }
        public DbSet<OBJ_Product> OBJ_Products { get; set; }
        public DbSet<SYS_User> SYS_Users { get; set; }
        public DbSet<SYS_Transaction> SYS_Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Setting the CDL_Identifier ---
            modelBuilder.Entity<CDL_Identifier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
                entity.HasIndex(e => e.Code).IsUnique();

                entity.HasOne(e => e.Parent)
               .WithMany(e => e.Children)
               .HasForeignKey(e => e.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Setting the OBJ_Product ---
            modelBuilder.Entity<OBJ_Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
                entity.Property(e => e.Value).HasPrecision(18, 2);
                entity.Property(e => e.CreateDate).HasDefaultValueSql("GETDATE()");

                // Relating to Category
                entity.HasOne(p => p.Category)
                    .WithMany(i => i.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relating to Subcategory
                entity.HasOne(p => p.SubCategory)
                    .WithMany(i => i.Subcategory)
                    .HasForeignKey(p => p.SubCategoryId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relating to User created
                entity.HasOne(p => p.CreateUser)
                    .WithMany()
                    .HasForeignKey(p => p.CreateUserId);
            });

            // --- Setting the SYS_User ---
            modelBuilder.Entity<SYS_User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasOne(u => u.RoleIdentifier)
                    .WithMany(i => i.RoleIdentifier)
                    .HasForeignKey(u => u.RoleIdentifierId);
            });

            // --- Setting the SYS_Transactions ---
            modelBuilder.Entity<SYS_Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

                // NumberTransaction [automatic] (1, 2, 3...)
                entity.Property(e => e.NumberTransaction).ValueGeneratedOnAdd();

                entity.Property(e => e.Date).HasDefaultValueSql("GETDATE()");

                entity.HasOne(t => t.Process)
                    .WithMany(i => i.Process)
                    .HasForeignKey(t => t.ProcessId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
