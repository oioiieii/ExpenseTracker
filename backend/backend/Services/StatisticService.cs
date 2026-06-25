using backend.Database.Repositories;
using backend.Database.Repositories.Interfaces;
using backend.Dtos.Stats;
using backend.Models;
using backend.Models.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class StatisticService : IStatisticService
{
    private readonly IStatisticRepository _statisticRepository;

    public StatisticService(IStatisticRepository statisticRepository)
    {
        _statisticRepository = statisticRepository;
    }

    public async Task<IResult<SummaryResponse>> GetSummaryAsync(DateOnly dateFrom, DateOnly dateTo)
    {
        if (dateFrom > dateTo)
            return Result<SummaryResponse>.Failure(ErrorCode.InvalidArgument,"Нижняя дата диапозона не может быть больше верхней.");
        
        var totalExpenses = await _statisticRepository.GetTotalAsync(dateFrom, dateTo);
        var totalExpensesGroupByDay = await _statisticRepository.GetTotalGroupByDayAsync(dateFrom, dateTo);
        var totalExpensesGroupByCategory = await _statisticRepository.GetTotalGroupByCategoryAsync(dateFrom, dateTo);

        return Result<SummaryResponse>.Success(new SummaryResponse()
        {
            TotalExpenses = totalExpenses,
            TotalExpensesGroupByDay = totalExpensesGroupByDay.Select(g => ExpenseGroupByDayResponse.FromEntity(g)),
            TotalExpensesGroupByCategory = totalExpensesGroupByCategory.Select(g => ExpenseGroupByCategoryResponse.FromEntity(g)),
        });
    }
}