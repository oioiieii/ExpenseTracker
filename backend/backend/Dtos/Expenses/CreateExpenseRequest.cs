namespace backend.Dtos.Expenses;

public class CreateExpenseRequest
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid CategoryId { get; set; }
    public DateOnly Date { get; set; }
}