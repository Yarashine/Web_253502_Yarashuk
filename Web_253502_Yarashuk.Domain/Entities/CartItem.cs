namespace Web_253502_Yarashuk.Domain.Entities;

public class CartItem
{
    public Product Item { get; set; } // Объект продукта
    public int Count { get; set; }    // Количество этого продукта в корзине

    public CartItem(Product item, int count)
    {
        Item = item;
        Count = count;
    }
}
