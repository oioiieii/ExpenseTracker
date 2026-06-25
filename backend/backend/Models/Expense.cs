namespace backend.Models;

public class Expense
{
    public const int MaxDescriptionLength = 500;

    public Guid Id { get; set; }

    public string Description
    {
        get => field;
        set
        {
            if(string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (value.Length > MaxDescriptionLength)
                throw new ArgumentException($"Длина описания не может превышать {MaxDescriptionLength} символов");
            field = value;
        }
    }

    public decimal Amount
    {
        get => field;
        set
        {
            if(value <= 0) throw new ArgumentOutOfRangeException(nameof(value),  $"Значение {nameof(value)} должно быть больше 0.");
            field =  value;
        }
    }

    public DateOnly Date
    {
        get;
        set
        {
            if (value > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException($"Дата расхода не может быть в будущем.");
            field = value;
        }
    }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public DateTime CreatedAt
    {
        get => field;
        set
        {
            if(value > DateTime.UtcNow) throw new ArgumentOutOfRangeException(nameof(value),  $"Значение {nameof(value)} не может быть больше {DateTime.UtcNow}.");
            field = value;
        }
    }

    public DateTime? UpdatedAt
    {
        get => field;
        set
        {
            if(value > DateTime.UtcNow) throw new ArgumentOutOfRangeException(nameof(value),  $"Значение {nameof(value)} не может быть больше {DateTime.UtcNow}.");
            field = value;
        }
    }
}