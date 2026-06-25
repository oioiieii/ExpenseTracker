using backend.Models.Interfaces;
using IResult = backend.Models.Interfaces.IResult;

namespace backend.Models;
/// <summary>
/// Представляет результат операции с информацией о статусе (успех или ошибка)
/// </summary>
public class Result : IResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public ErrorCode? ErrorCode { get; set; }

    public static Result Success()
    {
        return new Result { IsSuccess = true };
    }

    public static Result Failure(ErrorCode errorCode = Models.ErrorCode.UnknownError, string? errorMessage = null)
    {
        return new Result { ErrorCode = errorCode, ErrorMessage = errorMessage, IsSuccess = false };
    }
}

public class Result<T> : IResult<T>
{
    public T? Value { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public ErrorCode?  ErrorCode { get; set; }
    
    public static Result<T> Success(T value)
    {
        return new Result<T> { Value = value, IsSuccess = true };
    }

    public static Result<T> Failure(ErrorCode errorCode = Models.ErrorCode.UnknownError, string? errorMessage = null)
    {
        return new Result<T> {ErrorCode = errorCode, ErrorMessage = errorMessage, IsSuccess = false };
    }
}