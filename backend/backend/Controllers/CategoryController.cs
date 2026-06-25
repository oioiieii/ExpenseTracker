using backend.Database.Repositories;
using backend.Dtos.Category;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("/api/categories/")]
public class CategoryController:ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategoriesAsync()
    {
        var result = await _categoryService.GetCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse?>> GetCategoryByIdAsync(Guid id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        return this.ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid?>> CreateCategoryAsync([FromBody] CreateCategoryRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Название категории не должно быть пустым.");
        if (request.Name.Length > Category.MaxNameLength)
            return BadRequest($"Название не должно превышать {Category.MaxNameLength} символов.");
        var result = await _categoryService.AddCategoryAsync(request);
        return this.ToActionResult(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategoryAsync(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Название категории не должно быть пустым.");
        if (request.Name.Length > Category.MaxNameLength)
            return BadRequest($"Название не должно превышать {Category.MaxNameLength} символов.");

        var result = await _categoryService.UpdateCategoryAsync(id, request);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategoryAsync(Guid id)
    {
        var result = await _categoryService.DeleteCategoryAsync(id);
        return this.ToActionResult(result);
    }
}
