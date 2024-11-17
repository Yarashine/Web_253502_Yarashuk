namespace Web_253502_Yarashuk.Domain.Entities;

public class Cart
{
    // Словарь для хранения элементов корзины, где ключ - ID продукта
    public Dictionary<int, CartItem> CartItems { get; set; } = new();

    /// <summary>
    /// Добавить объект в корзину
    /// </summary>
    /// <param name="product">Продукт, который нужно добавить</param>
    /// <param name="count">Количество добавляемого продукта</param>
    public virtual void AddToCart(Product product, int count)
    {
        if (CartItems.ContainsKey(product.Id))
        {
            // Если продукт уже есть в корзине, увеличиваем его количество
            CartItems[product.Id].Count += count;
        }
        else
        {
            // Если продукта нет, добавляем новый элемент корзины
            CartItems[product.Id] = new CartItem(product, count);
        }
    }

    /// <summary>
    /// Удалить объект из корзины по ID продукта
    /// </summary>
    /// <param name="productId">ID удаляемого продукта</param>
    public virtual void RemoveItem(int productId)
    {
        if (CartItems.ContainsKey(productId))
        {
            CartItems.Remove(productId);
        }
    }

    /// <summary>
    /// Очистить корзину
    /// </summary>
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }

    /// <summary>
    /// Количество объектов в корзине
    /// </summary>
    public int Count => CartItems.Sum(item => item.Value.Count);

    /// <summary>
    /// Общая стоимость всех объектов в корзине
    /// </summary>
    public decimal TotalPrice => CartItems.Sum(item => item.Value.Item.Price * item.Value.Count);
}
