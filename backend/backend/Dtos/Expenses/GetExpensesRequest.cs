namespace backend.Dtos.Expenses;

public class GetExpensesRequest
{
    public DateOnly? DateFrom { get; set; } 
    public DateOnly? DateTo { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Search { get; set; }
}