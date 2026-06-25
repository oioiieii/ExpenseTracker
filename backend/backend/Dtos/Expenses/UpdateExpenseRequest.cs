namespace backend.Dtos.Expenses;

public class UpdateExpenseRequest
{
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public Guid CategoryId { get; set; }
    public DateOnly Date { get; set; }
}
