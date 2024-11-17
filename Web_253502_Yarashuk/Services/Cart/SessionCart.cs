namespace Web_253502_Yarashuk.UI.Services.Cart;

using Microsoft.AspNetCore.Http;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.UI.Extensions;

public class SessionCart : Cart
{
    private readonly ISession _session;

    public SessionCart(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext.Session;
        LoadCartFromSession();
    }

    // Загружаем данные корзины из сессии
    private void LoadCartFromSession()
    {
        var cart = _session.Get<Cart>("cart");
        if (cart != null)
        {
            CartItems = cart.CartItems;
        }
    }

    // Сохраняем данные корзины в сессию
    private void SaveCartToSession()
    {
        _session.Set("cart", this);
    }

    public override void AddToCart(Product product, int count = 1)
    {
        base.AddToCart(product, count);
        SaveCartToSession();
    }

    public override void RemoveItem(int productId)
    {
        base.RemoveItem(productId);
        SaveCartToSession();
    }

    public override void ClearAll()
    {
        base.ClearAll();
        SaveCartToSession();
    }
}
