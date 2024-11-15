using System.IO;
using System.Text;
using System.Text.Json;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Web_253502_Yarashuk.UI.Services.Authentication;
using Web_253502_Yarashuk.UI.Services.FileService;

namespace Web_253502_Yarashuk.UI.Services.ProductService;

public class ApiProductService : IProductService
{
    private HttpClient _httpClient;
    private ILogger<ApiProductService> _logger;
    private readonly JsonSerializerOptions _serializerOptions;
    private string _pageSize;
    private readonly IFileService _fileService;
    private readonly ITokenAccessor _tokenAccessor;
    public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetSection("ItemsPerPage").Value;

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;

        _fileService = fileService;
        _tokenAccessor = tokenAccessor;
    }

    public async Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile file)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        product.ImagePath = "Images/noimage.jpg";
        if (file != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(file);
            if (!string.IsNullOrEmpty(imageUrl))
                product.ImagePath = imageUrl;
        }

        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products");
        var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response
            .Content
            .ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
            return data;
        }

        _logger.LogError($"-----> object not created. Error:{response.StatusCode}");
        return ResponseData<Product>.Error($"Объект не добавлен. Error:{response.StatusCode}");
    }

    public async Task DeleteProductAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}Products/{id}";
        var uri = new Uri(urlString);
        var product = await GetProductByIdAsync(id);
        var path = "";
        if (product != null)
            path = product.Data.ImagePath;


        var filename = GetFileName(path);
        await _fileService.DeleteFileAsync(filename);
        var response = await _httpClient.DeleteAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = $"Object not deleted. Error: {response.StatusCode}";
            _logger.LogError(errorMessage);
        }
    }

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}Products/id-{id}";
        var uri = new Uri(urlString);

        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = $"Object not recieved. Error {response.StatusCode}";
            _logger.LogError(errorMessage);

            return new ResponseData<Product>
            {
                Successfull = false,
                ErrorMessage = errorMessage,
            };

        }

        var data = await response.Content.ReadFromJsonAsync<ResponseData<Product>>(_serializerOptions);
        return data!;
    }


    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName,int pageNo = 1)
    {
        //await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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
        //var a = await response                .Content                .ReadFromJsonAsync<ResponseData<ListModel<Product>>>                (_serializerOptions);
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
    public static string GetFileName(string path)
    {

        // Если это URL, извлекаем все после последнего '/'
        if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
        {
            Uri uri = new Uri(path);
            return Path.GetFileName(uri.LocalPath);
        }

        // Если это относительный путь, просто извлекаем имя файла
        return Path.GetFileName(path);
    }
    public async Task UpdateProductAsync(Product product, IFormFile formFile)
    {
        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

        if (formFile != null)
        {
            var path = product.ImagePath;
            var filename = GetFileName(path);
            await _fileService.DeleteFileAsync(filename);
            //await _fileService.DeleteFileAsync(path);
            var imageUrl = await _fileService.SaveFileAsync(formFile);
            if (!string.IsNullOrEmpty(imageUrl))
                product.ImagePath = imageUrl;
        }

        var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Products/" + product.Id);
        await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);
    }
}
