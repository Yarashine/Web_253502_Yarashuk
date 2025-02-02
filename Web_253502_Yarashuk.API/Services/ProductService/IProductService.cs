﻿using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.API.Services.ProductService;

public interface IProductService
{
    /// <summary>
    /// Получение списка всех объектов
    /// </summary>
    /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
    /// <param name="pageNo">номер страницы списка</param>
    /// <param name="pageSize">количество объектов на странице</param>
    /// <returns></returns>
    public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1,int pageSize = 3);
    /// <summary>
    /// Поиск объекта по Id
    /// </summary>
    /// <param name="id">Идентификатор объекта</param>
    /// <returns></returns>
    public Task<ResponseData<Product>> GetProductByIdAsync(int id);
    /// <summary>
    /// Обновление объекта
    /// </summary>
    /// <param name="id">Id изменяемомго объекта</param>
    /// <param name="product">объект с новыми параметрами</param>
    /// <returns></returns>
    public Task UpdateProductAsync(int id, Product product);
    /// <summary>
    /// Удаление объекта
    /// </summary>
    /// <param name="id">Id удаляемомго объекта</param>
    /// <returns></returns>
    public Task DeleteProductAsync(int id);
    /// <summary>
    /// Создание объекта
    /// </summary>
    /// <param name="product">Новый объект</param>
    /// <returns>Созданный объект</returns>
    public Task<ResponseData<Product>> CreateProductAsync(Product product);
    /// <summary>
    /// Сохранить файл изображения для объекта
    /// </summary>
    /// <param name="id">Id объекта</param>
    /// <param name="formFile">файл изображения</param>
    /// <returns>Url к файлу изображения</returns
    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}
