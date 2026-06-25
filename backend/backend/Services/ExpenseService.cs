using backend.Database.Repositories;
using backend.Database.Repositories.Interfaces;
using backend.Dtos.Expenses;
using backend.Models;
using backend.Models.Interfaces;
using backend.Services.Interfaces;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Services;

public class ExpenseService: IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ExpenseService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository)
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IResult<IEnumerable<ExpenseResponse>>> GetExpensesAsync(GetExpensesRequest request)
    {
        if (request.DateFrom.HasValue && request.DateTo.HasValue && request.DateFrom > request.DateTo)
            return Result<IEnumerable<ExpenseResponse>>.Failure(ErrorCode.InvalidArgument,"Нижняя дата диапозона не может быть больше верхней.");
        
        var expenseFilter = new ExpenseFilter() 
        {
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            CategoryId = request.CategoryId,
            SearchText = request.Search,
        };
        
        var result = await _expenseRepository.GetExpensesAsync(expenseFilter);
        return Result<IEnumerable<ExpenseResponse>>.Success(result.Select(ExpenseResponse.FromEntity));
    }

    public async Task<ExpenseResponse?> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetExpenseByIdAsync(id);
        return expense is not null ? ExpenseResponse.FromEntity(expense) : null;
    }

    public async Task<IResult<Guid?>> CreateExpenseAsync(CreateExpenseRequest request)
    {
        if (request.Date > DateOnly.FromDateTime(DateTime.Today))
            return Result<Guid?>.Failure(ErrorCode.InvalidArgument, "Дата не может быть в будущем.");
        var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
        if(category == null) return Result<Guid?>.Failure(ErrorCode.NotFound,$"Категория не найдена.");

        var expense = new Expense()
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Amount =  request.Amount,
            Date = request.Date,
            Category = category,
            CreatedAt = DateTime.UtcNow,
        };

        await _expenseRepository.AddExpenseAsync(expense);
        
        return Result<Guid?>.Success(expense.Id);
    }

    public async Task<IResult> UpdateExpenseAsync(Guid id, UpdateExpenseRequest request)
    {
        if (request.Date > DateOnly.FromDateTime(DateTime.Today))
            return Result<Guid?>.Failure(ErrorCode.InvalidArgument, "Дата не может быть в будущем.");
        var expense = await _expenseRepository.GetExpenseByIdAsync(id);
        if (expense is null) return Result.Failure(ErrorCode.NotFound, "Расход не найден.");

        var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
        if(category == null) return Result.Failure(ErrorCode.NotFound, "Категория не найдена.");

        expense.Description = request.Description;
        expense.Amount = request.Amount;
        expense.Date = request.Date;
        expense.CategoryId = category.Id;
        expense.Category = category;
        expense.UpdatedAt = DateTime.UtcNow;

        await _expenseRepository.UpdateExpenseAsync(expense);

        return Result.Success();
    }

    public async Task<IResult> DeleteExpenseAsync(Guid id)
    {
        var expense = await _expenseRepository.GetExpenseByIdAsync(id);
        if (expense is null) return Result.Failure(ErrorCode.NotFound,"Расход не найден.");
        
        await _expenseRepository.DeleteExpenseAsync(expense);
        return Result.Success();
    }
    
}
