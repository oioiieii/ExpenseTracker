using backend.Models;

namespace backend.Dtos.Expenses;

public class ExpenseResponse
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public required string Description { get; set; } 

    public static ExpenseResponse FromEntity(Expense entity)
    {
        return new ExpenseResponse
        {
            Id = entity.Id,
            Date = entity.Date,
            CategoryId = entity.CategoryId,
            Amount = entity.Amount,
            Description = entity.Description,
        };
    }
}