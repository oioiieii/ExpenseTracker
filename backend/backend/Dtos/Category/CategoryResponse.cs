namespace backend.Dtos.Category;

public class CategoryResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public static CategoryResponse FromEntity(Models.Category entity)
    {
        return new CategoryResponse()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}