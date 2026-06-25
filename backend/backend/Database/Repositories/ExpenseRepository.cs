using backend.Database.Context;
using backend.Database.Repositories.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database.Repositories;


public class ExpenseRepository : IExpenseRepository
{
    private readonly TrackerExpensesDbContext _dbContext;

    public ExpenseRepository(TrackerExpensesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Expense>> GetExpensesAsync(ExpenseFilter? filter)
    {
        var query = _dbContext.Expenses.Include(e => e.Category).OrderByDescending(e=>e.Date)
            .ThenByDescending(e => e.CreatedAt).ThenByDescending(e => e.UpdatedAt).AsQueryable(); 

        if (filter != null)
        {
            if (filter.DateFrom != null)
            {
                query = query.Where(e => e.Date >= filter.DateFrom);
            }

            if (filter.DateTo != null)
            {
                query = query.Where(e => e.Date <= filter.DateTo);
            }

            if (filter.CategoryId != null)
            {
                query = query.Where(e => e.CategoryId == filter.CategoryId);
            }

            if (filter.SearchText != null)
            {
                query = query.Where(e => e.Description.ToLower().Contains(filter.SearchText.ToLower()));
            }
        }
        
        return await query.ToListAsync();
    }
    
    public async Task<Expense?> GetExpenseByIdAsync(Guid id)
    {
        return await _dbContext.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddExpenseAsync(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);

        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateExpenseAsync(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        
        _dbContext.Expenses.Update(expense);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteExpenseAsync(Expense expense)
    {
        ArgumentNullException.ThrowIfNull(expense);
        
        _dbContext.Expenses.Remove(expense);
        await _dbContext.SaveChangesAsync();
    }
}
