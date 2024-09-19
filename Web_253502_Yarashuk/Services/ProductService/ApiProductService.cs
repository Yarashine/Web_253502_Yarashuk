using System.Text;
using System.Text.Json;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.UI.Services.ProductService;

public class ApiProductService : IProductService
{
    private HttpClient _httpClient;
    private ILogger<ApiProductService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;
    private string _pageSize;
    public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }

    public async Task<ResponseData<Product>> CreateProductAsync(Product product,IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Productes");

        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);

            return data; // Product;
        }
        _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
        return ResponseData<Product>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName,int pageNo = 1)
    {
        // подготовка URL запроса
        var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Products");
        // добавить категорию в маршрут
        if (categoryNormalizedName != null)
        {
            urlString.Append($"/{categoryNormalizedName}?");
        }
        else
            urlString.Append("?");
        // добавить номер страницы в маршрут
        if (pageNo > 1)
        {
            urlString.Append($"pageNo={pageNo}");
        }
        else
        {
            urlString.Append("pageNo=1");
        }
        // добавить размер страницы в строку запроса
        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("&pageSize", _pageSize));
        }
        else
        {
            urlString.Append("&pageNo=1");
        }
        //GET https://localhost:7002/api/Products?category=Все&pageNo=2&pageSize=3
        //{https://localhost:7002/api/Products/Все?pageNo=2}
        // отправить запрос к API
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response
                .Content
                .ReadFromJsonAsync<ResponseData<ListModel<Product>>>
                (_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return ResponseData<ListModel<Product>>
                .Error($"Ошибка: {ex.Message}");
            }
        }
        _logger.LogError($"-----> Данные не получены от сервера. Error:{ response.StatusCode.ToString()}");
        return ResponseData<ListModel<Product>>.Error($"Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
}








    public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
