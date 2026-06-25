using backend.Models;

namespace backend.Database.Repositories.Interfaces;

public interface IStatisticRepository
{
    Task<decimal> GetTotalAsync(DateOnly start, DateOnly end);
    Task<IEnumerable<ExpenseGroupByDay>> GetTotalGroupByDayAsync(DateOnly start, DateOnly end);
    Task<IEnumerable<ExpenseGroupByCategory>> GetTotalGroupByCategoryAsync(DateOnly start, DateOnly end);
}