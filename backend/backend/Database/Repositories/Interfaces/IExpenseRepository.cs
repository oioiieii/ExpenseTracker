using backend.Models;

namespace backend.Database.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<List<Expense>> GetExpensesAsync(ExpenseFilter? filter);
    Task<Expense?> GetExpenseByIdAsync(Guid id);
    Task AddExpenseAsync(Expense expense);
    Task UpdateExpenseAsync(Expense expense);
    Task DeleteExpenseAsync(Expense expense);
}
