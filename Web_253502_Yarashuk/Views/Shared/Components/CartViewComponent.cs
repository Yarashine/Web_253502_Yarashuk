using Microsoft.AspNetCore.Mvc;
using Web_253502_Yarashuk.Models;

namespace Web_253502_Yarashuk.Views.Shared.Components;

public class CartViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // Логика получения данных о корзине
        var cartModel = GetCartModel();

        return View(cartModel);
    }

    private CartModel GetCartModel()
    {
        // Ваша логика для получения данных о корзине
        // Например:
        return new CartModel
        {
            ItemCount = 3,
            TotalPrice = 150.0m
        };
    }
}