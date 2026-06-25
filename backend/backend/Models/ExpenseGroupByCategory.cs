namespace backend.Models;

public class ExpenseGroupByCategory
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}