using backend.Models;

namespace backend.Database.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task DeleteCategoryAsync(Category category);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task<Category?> GetCategoryByName(string name);
    Task<bool> HasExpensesAsync(Guid categoryId);
}
