using backend.Database.Repositories;
using backend.Database.Repositories.Interfaces;
using backend.Dtos.Category;
using backend.Models;
using backend.Models.Interfaces;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IResult<Guid?>> AddCategoryAsync(CreateCategoryRequest request)
    {
        var existingCategory = await _categoryRepository.GetCategoryByName(request.Name);
        if (existingCategory != null)
            return Result<Guid?>.Failure(ErrorCode.InvalidArgument,
                $"Категория с именем '{request.Name}' уже существует.");
        
        var category = new Category()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };
        
        await _categoryRepository.AddCategoryAsync(category);
        return Result<Guid?>.Success(category.Id);
    }
    
    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync()
    {
        var result = await _categoryRepository.GetCategoriesAsync();
        return result.Select(category => CategoryResponse.FromEntity(category));
    }

    public async Task<IResult<CategoryResponse?>> GetCategoryByIdAsync(Guid id)
    {
        var result = await _categoryRepository.GetCategoryByIdAsync(id);
        if (result == null)
            return Result<CategoryResponse?>.Failure(ErrorCode.NotFound, $"Категория не найдена.");
        
        return Result<CategoryResponse?>.Success(CategoryResponse.FromEntity(result));
    }

    public async Task<IResult> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category is null) return Result.Failure(ErrorCode.NotFound, "Категория не найдена.");

        var existingCategory = await _categoryRepository.GetCategoryByName(request.Name);
        if (existingCategory is not null && existingCategory.Id != id)
            return Result.Failure(ErrorCode.InvalidArgument,
                $"Категория с именем '{request.Name}' уже существует.");

        category.Name = request.Name;
        category.UpdatedAt = DateTime.UtcNow;
        
        await _categoryRepository.UpdateCategoryAsync(category);

        return Result.Success();
    }

    public async Task<IResult> DeleteCategoryAsync(Guid id)
    {
        var hasExpenses = await _categoryRepository.HasExpensesAsync(id);
        if (hasExpenses) return Result.Failure(ErrorCode.InvalidArgument, "Нельзя удалить категорию, так как на нее ссылаются расходы.");
        
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category is null) return Result.Failure(ErrorCode.NotFound, $"Категория не найдена");
        
        await _categoryRepository.DeleteCategoryAsync(category);
        return Result.Success();
    }
}
