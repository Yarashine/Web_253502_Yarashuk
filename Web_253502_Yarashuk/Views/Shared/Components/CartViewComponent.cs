using Microsoft.AspNetCore.Mvc;
using Web_253502_Yarashuk.Domain.Entities;

public class CartViewComponent : ViewComponent
{
    private readonly Cart _cart;

    public CartViewComponent(Cart cart)
    {
        _cart = cart;
    }

    public IViewComponentResult Invoke()
    {
        var cartModel = new
        {
            ItemCount = _cart.Count,
            TotalPrice = _cart.TotalPrice
        };

        return View(cartModel);
    }
}
