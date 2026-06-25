namespace backend.Models;

public class ExpenseGroupByDay
{
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
}