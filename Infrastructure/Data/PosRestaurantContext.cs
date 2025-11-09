using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class PosRestaurantContext : IdentityDbContext<User>
    {
        public PosRestaurantContext(DbContextOptions<PosRestaurantContext> options) : base(options)
        {
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
            });


            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasMany(u => u.Restaurants)
            //          .WithMany(r => r.Staff)
            //          .UsingEntity<StaffAssignment>(
            //              j => j
            //                  .HasOne(sa => sa.Restaurant)
            //                  .WithMany()
            //                  .HasForeignKey(sa => sa.RestaurantId),
            //              j => j
            //                  .HasOne(sa => sa.User)
            //                  .WithMany()
            //                  .HasForeignKey(sa => sa.UserId),
            //              j =>
            //              {
            //                  j.HasKey(sa => new { sa.UserId, sa.RestaurantId, sa.RoleId });
            //                  j.HasOne(sa => sa.Role)
            //                  .WithMany()
            //                  .HasForeignKey(sa => sa.RoleId);
            //              });
            //});

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

        /// <summary>
        /// Przesłonięcie standardowej metody SaveChangesAsync.
        /// Najpierw uruchamia naszą logikę audytową, a następnie wywołuje bazową implementację.
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Przesłonięcie DRUGIEJ, bardziej złożonej wersji SaveChangesAsync.
        /// Robimy to, aby mieć pewność, że nasza logika audytowa zadziała ZAWSZE,
        /// niezależnie od tego, jakiej wersji metody użyje framework (np. UserManager).
        /// </summary>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            SetAuditProperties();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}