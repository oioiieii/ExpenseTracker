using backend.Models;

namespace backend.Dtos.Stats;

public class ExpenseGroupByDayResponse
{
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }

    public static ExpenseGroupByDayResponse FromEntity(ExpenseGroupByDay entity)
    {
        return new ExpenseGroupByDayResponse()
        {
            Date = entity.Date,
            Amount = entity.Amount
        };
    }
}