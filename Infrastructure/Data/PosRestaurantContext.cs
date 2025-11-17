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


        // --- SKONFIGURUJ RELACJE ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //konieczne dla IdentityDbContext

            // --- Konfiguracja dla Product ---
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

            // --- Konfiguracja dla Category ---
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.Restaurant)
                      .WithMany(r => r.Categories)
                      .HasForeignKey(c => c.RestaurantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Konfiguracja dla ProductIngredient (tabela łącząca wiele-do-wielu) ---
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
                entity.HasKey(sa => new { sa.UserId, sa.RestaurantId, sa.RoleId });
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


        // --- ZMIANA: PRZEBUDOWANA LOGIKA ZAPISYWANIA ZMIAN ---

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

        // --- Metoda pomocnicza, która wywołuje inne, wyspecjalizowane metody ---
        private void ApplyCustomLogicBeforeSaving()
        {
            SetAuditProperties();
            ValidateTenantSecurity();
        }

        /// <summary>
        /// Prywatna metoda pomocnicza, która zawiera logikę ustawiania pól audytowych (CreatedAt, LastModified).
        /// Używamy jej, aby uniknąć powtarzania tego samego kodu w obu wersjach SaveChangesAsync.
        /// </summary>
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
            if (restaurantId == null) return; // Zezwól na operacje bez kontekstu (np. seedowanie, SuperAdmin)

            foreach (var entry in ChangeTracker.Entries<ITenantEntity>()) // Używamy interfejsu ITenantEntity
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Automatycznie przypisz nową encję do restauracji zalogowanego użytkownika.
                        entry.Entity.RestaurantId = restaurantId.Value;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        // KRYTYCZNE ZABEZPIECZENIE: Sprawdź, czy użytkownik nie próbuje
                        // modyfikować lub usuwać danych z innej restauracji.
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