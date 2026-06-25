using backend.Database.Context;
using backend.Database.Repositories.Interfaces;
using backend.Dtos.Stats;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database.Repositories;

public class StatisticRepository : IStatisticRepository
{
    private readonly TrackerExpensesDbContext _dbContext;

    public StatisticRepository(TrackerExpensesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<decimal> GetTotalAsync(DateOnly start, DateOnly end)
    {
        return await _dbContext.Expenses.Where(e => e.Date >= start && e.Date <= end).SumAsync(e => e.Amount);
    }
    
    public async Task<IEnumerable<ExpenseGroupByDay>> GetTotalGroupByDayAsync(DateOnly start, DateOnly end) 
    {
        return await _dbContext.Expenses.Where(e => e.Date >= start && e.Date <= end)
            .GroupBy(e => e.Date).Select(g => new ExpenseGroupByDay()
            {
                Date = g.Key,
                Amount = g.Sum(e => e.Amount)
            }).ToListAsync();
    }
    
    public async Task<IEnumerable<ExpenseGroupByCategory>> GetTotalGroupByCategoryAsync(DateOnly start, DateOnly end)
    {
        return await _dbContext.Expenses.Include(e => e.Category).Where(e => e.Date >= start && e.Date <= end)
            .GroupBy(e => e.Category).Select(g => new ExpenseGroupByCategory()
            {
                CategoryId = g.Key!.Id,
                CategoryName = g.Key!.Name,
                Amount = g.Sum(e => e.Amount)
            }).ToListAsync();
    }
}