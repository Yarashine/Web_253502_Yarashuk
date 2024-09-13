
namespace Web_253502_Yarashuk.Domain.Entities;

public class Product
{
    public int Id { get; set; } // Уникальный номер
    public required string Name { get; set; } // Название
    public required string Description { get; set; } // Описание
    public int CategoryId { get; set; } // Идентификатор категории
    public Category? Category { get; set; }// Навигационное свойство для категории
    public decimal Price { get; set; } // Цена
    public string? ImagePath { get; set; } // Путь к файлу изображения
    public string? MimeType { get; set; } // Mime тип изображения
}
