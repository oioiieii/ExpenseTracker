using System.Diagnostics;
using backend.Dtos.Expenses;
using backend.Dtos.Stats;
using backend.Models;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("/api/expenses/")]
public class ExpenseController: ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IStatisticService _statisticService;

    public ExpenseController(IExpenseService expenseService, IStatisticService statisticService)
    {
        _expenseService = expenseService;
        _statisticService = statisticService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetExpensesAsync([FromQuery] GetExpensesRequest request)
    {
        var result = await _expenseService.GetExpensesAsync(request);
        return this.ToActionResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseResponse>> GetExpenseAsync(Guid id)
    {
        var result = await _expenseService.GetExpenseByIdAsync(id);
        return result != null ? Ok(result) : NotFound($"Пользователя с id = '{id}' не существует.");
    }

    [HttpPost]
    public async Task<ActionResult<Guid?>> PostExpenseAsync([FromBody] CreateExpenseRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Description)) return BadRequest("Описание не должно быть пустым.");
        if (request.Description.Length > Expense.MaxDescriptionLength)
            return BadRequest($"Название не должно превышать {Expense.MaxDescriptionLength} символов.");
        if (request.Amount <= 0) return BadRequest($"Сумма расхода '{request.Amount}' должна быть положительной.");
        
        var result = await _expenseService.CreateExpenseAsync(request);
        return this.ToActionResult(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> PutExpenseAsync(Guid id, [FromBody] UpdateExpenseRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Description)) return BadRequest("Описание не должно быть пустым.");
        if (request.Description.Length > Expense.MaxDescriptionLength)
            return BadRequest($"Название не должно превышать {Expense.MaxDescriptionLength} символов.");
        if (request.Amount <= 0) return BadRequest($"Сумма расхода '{request.Amount}' должна быть положительной.");

        var result = await _expenseService.UpdateExpenseAsync(id, request);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExpenseAsync(Guid id)
    {
        var result = await _expenseService.DeleteExpenseAsync(id);
        return this.ToActionResult(result);
    }

    [HttpGet("summary/")]
    public async Task<ActionResult<SummaryResponse>> GetSummaryAsync([FromQuery] DateOnly dateFrom, [FromQuery] DateOnly dateTo) 
    {
        var result = await _statisticService.GetSummaryAsync(dateFrom, dateTo);
        return this.ToActionResult(result);
    }
}
