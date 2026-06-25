namespace backend.Models;

public class Category
{
    public const int MaxNameLength = 70;
    public Guid Id { get; set; }

    public string Name
    {
        get;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (value.Length > MaxNameLength)
                throw new ArgumentException($"Название не должно превышать {MaxNameLength} символов.");
            field = value;
        }
    }
    
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