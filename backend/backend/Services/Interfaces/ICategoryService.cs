using backend.Dtos.Category;
using backend.Models;
using backend.Models.Interfaces;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Services;

public interface ICategoryService
{
    Task<IResult<Guid?>> AddCategoryAsync(CreateCategoryRequest request);

    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync();

    Task<IResult<CategoryResponse?>> GetCategoryByIdAsync(Guid id);

    Task<IResult> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
    
    Task<IResult> DeleteCategoryAsync(Guid id);
}
