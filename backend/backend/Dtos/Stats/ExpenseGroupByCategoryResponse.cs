using backend.Models;

namespace backend.Dtos.Stats;

public class ExpenseGroupByCategoryResponse
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public static ExpenseGroupByCategoryResponse FromEntity(ExpenseGroupByCategory entity)
    {
        return new ExpenseGroupByCategoryResponse()
        {
            CategoryId = entity.CategoryId,
            CategoryName = entity.CategoryName,
            Amount = entity.Amount
        };
    }
}