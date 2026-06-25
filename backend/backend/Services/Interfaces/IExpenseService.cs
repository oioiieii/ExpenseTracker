using backend.Dtos.Expenses;
using backend.Models.Interfaces;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Services.Interfaces;

public interface IExpenseService
{
    Task<IResult<IEnumerable<ExpenseResponse>>> GetExpensesAsync(GetExpensesRequest request);
    Task<ExpenseResponse?> GetExpenseByIdAsync(Guid id);
    Task<IResult<Guid?>> CreateExpenseAsync(CreateExpenseRequest request);
    Task<IResult> UpdateExpenseAsync(Guid id, UpdateExpenseRequest request);
    Task<IResult> DeleteExpenseAsync(Guid id);
}
