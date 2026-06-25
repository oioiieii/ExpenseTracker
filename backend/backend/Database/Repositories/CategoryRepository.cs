using backend.Database.Context;
using backend.Database.Repositories.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TrackerExpensesDbContext _dbContext;

    public CategoryRepository(TrackerExpensesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _dbContext.Categories.OrderByDescending(c => c.CreatedAt).ThenByDescending(e => e.UpdatedAt).ToListAsync();
    }
    
    public async Task DeleteCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> HasExpensesAsync(Guid categoryId)
    {
        return await _dbContext.Expenses
            .AnyAsync(e => e.CategoryId == categoryId);
    }
    

    public async Task AddCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByName(string name)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}
