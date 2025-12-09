using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using System.Security;

namespace Infrastructure.Data
{
    public class PosRestaurantContext : IdentityDbContext<User>
    {
        private readonly ICurrentUserService _currentUserService;

        public PosRestaurantContext(DbContextOptions<PosRestaurantContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<StaffAssignment> StaffAssignments { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(p => p.Restaurant)
                      .WithMany(r => r.Products)
                      .HasForeignKey(p => p.RestaurantId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.OwnsOne(o => o.DeliveryAddress, a =>
                {
                    a.Property(x => x.Street).HasMaxLength(100).HasColumnName("DeliveryAddress_Street");
                    a.Property(x => x.City).HasMaxLength(50).HasColumnName("DeliveryAddress_City");
                    a.Property(x => x.PostalCode).HasMaxLength(10).HasColumnName("DeliveryAddress_PostalCode");
                    a.Property(x => x.LocalNumber).HasMaxLength(10).HasColumnName("DeliveryAddress_LocalNumber");
                });

                b.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                b.Property(o => o.Type)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                b.HasMany(o => o.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(o => o.Driver)
                    .WithMany()
                    .HasForeignKey(o => o.DriverId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.Property(oi => oi.UnitPrice)
                    .HasPrecision(18, 2);

                b.Property(oi => oi.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.Restaurant)
                      .WithMany(r => r.Categories)
                      .HasForeignKey(c => c.RestaurantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductIngredient>(entity =>
            {
                entity.HasKey(pi => new { pi.ProductId, pi.IngredientId });

                entity.HasOne(pi => pi.Product)
                      .WithMany(p => p.ProductIngredients)
                      .HasForeignKey(pi => pi.ProductId);

                entity.HasOne(pi => pi.Ingredient)
                      .WithMany()
                      .HasForeignKey(pi => pi.IngredientId);

                entity.Property(pi => pi.Unit)
                    .HasConversion<string>();
                entity.Property(pi => pi.Amount).HasPrecision(18, 4);
            });

            modelBuilder.Entity<StaffAssignment>(entity =>
            {
                entity.HasKey(sa => sa.Id);

                entity.HasIndex(sa => new { sa.UserId, sa.RestaurantId, sa.RoleId }).IsUnique();

                entity.HasOne(sa => sa.User)
                      .WithMany(u => u.StaffAssignments)
                      .HasForeignKey(sa => sa.UserId);

                entity.HasOne(sa => sa.Restaurant)
                      .WithMany(r => r.StaffAssignments)
                      .HasForeignKey(sa => sa.RestaurantId);

                entity.HasOne(sa => sa.Role)
                      .WithMany()
                      .HasForeignKey(sa => sa.RoleId);
            });
        }



        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyCustomLogicBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyCustomLogicBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyCustomLogicBeforeSaving()
        {
            SetAuditProperties();
            ValidateTenantSecurity();
        }

        private void SetAuditProperties()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
        }

        private void ValidateTenantSecurity()
        {
            var restaurantId = _currentUserService.RestaurantId;
            if (restaurantId == null) return;

            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.RestaurantId = restaurantId.Value;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        if (entry.Entity.RestaurantId != restaurantId.Value)
                        {
                            throw new SecurityException("Próba modyfikacji danych należących do innej restauracji.");
                        }
                        break;
                }
            }
        }
    }
}