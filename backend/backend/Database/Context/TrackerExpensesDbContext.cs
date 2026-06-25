
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace backend.Database.Context;

public class TrackerExpensesDbContext: DbContext
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    public TrackerExpensesDbContext(DbContextOptions<TrackerExpensesDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(Expense.MaxDescriptionLength);

            entity.Property(e => e.Amount)
                .IsRequired();

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();
            
            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(Category.MaxNameLength);

            entity.HasIndex(c => c.Name)
                .IsUnique();
            
            var seedCreatedAt = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc);

            entity.HasData(
                new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Еда", CreatedAt = seedCreatedAt },
                new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Транспорт", CreatedAt = seedCreatedAt },
                new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Жильё", CreatedAt = seedCreatedAt },
                new Category { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Развлечения", CreatedAt = seedCreatedAt },
                new Category { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Здоровье", CreatedAt = seedCreatedAt },
                new Category { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Другое", CreatedAt = seedCreatedAt }
            );
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
