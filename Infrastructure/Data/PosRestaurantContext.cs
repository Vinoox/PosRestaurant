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


        // --- SKONFIGURUJ RELACJE ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // --- Konfiguracja dla Product ---
            modelBuilder.Entity<Product>(entity =>
            {
                // TO JEST ROZWIĄZANIE:
                // Jawnie mówimy EF, że kolumna 'Price' w bazie danych
                // ma być typu decimal z precyzją 18 i skalą 2.
                entity.Property(p => p.Price).HasColumnType("decimal(18, 2)");
            });




            // --- Konfiguracja dla ProductIngredient ---

            // 1. Zdefiniuj klucz złożony (Composite Primary Key)
            // Mówimy EF, że unikalnym identyfikatorem wiersza w tej tabeli
            // jest kombinacja ProductId i IngredientId. Nie można mieć dwa razy
            // tego samego składnika dla tego samego produktu.
            modelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            // 2. Skonfiguruj relację "od strony" Product
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product) // Każdy wpis w ProductIngredient ma JEDEN Produkt...
                .WithMany(p => p.ProductIngredients) // ...a każdy Produkt ma WIELE wpisów w ProductIngredient.
                .HasForeignKey(pi => pi.ProductId); // Łączy je klucz ProductId.

            // 3. Skonfiguruj relację "od strony" Ingredient
            modelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient) // Każdy wpis w ProductIngredient ma JEDEN Składnik...
                .WithMany() // ...a każdy Składnik może być w WIELE wpisów (ale nie potrzebujemy listy w encji Ingredient).
                .HasForeignKey(pi => pi.IngredientId); // Łączy je klucz IngredientId.
        }


        // --- DODANA METODA DO AUTOMATYCZNEGO USTAWIANIA DAT ---
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Przechodzimy przez wszystkie encje, które EF Core śledzi
            // i które dziedziczą po naszej klasie bazowej AuditableEntity.
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                // Używamy instrukcji switch, aby sprawdzić, czy encja jest
                // nowo dodawana (Added) czy modyfikowana (Modified).
                switch (entry.State)
                {
                    // Przypadek 1: Nowy rekord jest dodawany do bazy.
                    case EntityState.Added:
                        // Automatycznie ustawiamy datę utworzenia na aktualny czas UTC.
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;

                    // Przypadek 2: Istniejący rekord jest modyfikowany.
                    case EntityState.Modified:
                        // Automatycznie ustawiamy datę ostatniej modyfikacji.
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            // Na końcu wywołujemy oryginalną metodę SaveChangesAsync z klasy bazowej,
            // która fizycznie zapisze zmiany (wraz z naszymi nowymi datami) do bazy danych.
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}