namespace backend.Models;

public class ExpenseFilter
{
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public Guid? CategoryId { get; set; }
    public string? SearchText { get; set; }
}