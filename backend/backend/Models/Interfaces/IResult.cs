namespace backend.Models.Interfaces;

public interface IResult<T> : IResult
{
    T? Value { get; set; }
}

public interface IResult
{
    bool IsSuccess { get; set; }
    string? ErrorMessage { get; set; }
    ErrorCode? ErrorCode { get; set; }
}