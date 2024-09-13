namespace Web_253502_Yarashuk.Domain.Entities;

public class Category
{
    public int Id { get; set; } // Уникальный номер
    public required string Name { get; set; } // Название категории
    public required string NormalizedName { get; set; }
}
