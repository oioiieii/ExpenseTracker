namespace backend.Dtos.Stats;

public class SummaryResponse
{
    public decimal TotalExpenses { get; set; }
    public required IEnumerable<ExpenseGroupByDayResponse> TotalExpensesGroupByDay {get; set;}
    public required  IEnumerable<ExpenseGroupByCategoryResponse> TotalExpensesGroupByCategory {get; set;}
}