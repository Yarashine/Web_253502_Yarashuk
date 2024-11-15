using Web_253502_Yarashuk.Domain.Entities;

namespace Web_253502_Yarashuk.Domain.Models
{
    public class ListModel<T>
    {
        // Запрошенный список объектов
        public List<T> Items { get; set; } = new();

        // Номер текущей страницы
        public int CurrentPage { get; set; } = 1;

        // Общее количество страниц
        public int TotalPages { get; set; } = 1;

    }
}
