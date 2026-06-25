using backend.Dtos.Stats;
using backend.Models.Interfaces;

namespace backend.Services.Interfaces;

public interface IStatisticService
{
    Task<IResult<SummaryResponse>> GetSummaryAsync(DateOnly dateFrom, DateOnly dateTo);
}