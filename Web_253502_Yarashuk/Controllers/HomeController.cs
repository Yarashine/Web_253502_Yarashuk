using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Web_253502_Yarashuk.Models;

namespace Web_253502_Yarashuk.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Передаем текст с помощью ViewData
        ViewData["Title"] = "Лабораторная работа №2";

        // Создаем список экземпляров ListDemo
        List<ListDemo> items = new List<ListDemo>
    {
        new ListDemo { Id = 1, Name = "Первый элемент" },
        new ListDemo { Id = 2, Name = "Второй элемент" },
        new ListDemo { Id = 3, Name = "Третий элемент" }
    };

        // Передаем список в представление с помощью SelectList
        ViewBag.ItemList = new SelectList(items, "Id", "Name");

        return View();
    }

    [HttpPost]
    public IActionResult PostAction(int selectedItem)
    {
        // Здесь вы можете обработать выбранный элемент
        // Например, просто перенаправим назад на Index
        return RedirectToAction("Index");
    }
}
