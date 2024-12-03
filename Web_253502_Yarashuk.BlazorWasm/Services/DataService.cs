namespace Web_253502_Yarashuk.BlazorWasm.Services;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly string _apiBaseUrl;
        private readonly int _pageSize;

        public event Action? DataLoaded;

        private readonly IAccessTokenProvider _tokenProvider;
        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalPages { get; set; } = 0;
        public int CurrentPage { get; set; } = 1;
        public Category? SelectedCategory { get; set; } = null;

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            // Настройки из appsettings.json
            _apiBaseUrl = configuration.GetValue<string>("ApiBaseUrl") ?? throw new InvalidOperationException("ApiBaseUrl is not configured");
            _pageSize = configuration.GetValue<int>("PageSize");

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

    public async Task GetProductListAsync(int pageNo = 1)
    {
        try
        {
            // Получить JWT токен из IAccessTokenProvider
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (!tokenRequest.TryGetToken(out var token))
            {
                ErrorMessage = "Не удалось получить токен авторизации.";
                Success = false;
                return;
            }

            // Создание базового маршрута
            var route = new StringBuilder("Products");

            // Добавить категорию в маршрут, если она выбрана
            if (SelectedCategory is not null)
            {
                route.Append($"/{SelectedCategory.NormalizedName}");
            }

            // Формирование параметров запроса
            List<KeyValuePair<string, string>> queryData = new();

            // Добавить номер страницы
            if (pageNo > 1)
            {
                queryData.Add(new KeyValuePair<string, string>("pageNo", pageNo.ToString()));
            }

            // Добавить размер страницы
            if (_pageSize != 3) // Условие проверки размера страницы
            {
                queryData.Add(new KeyValuePair<string, string>("pageSize", _pageSize.ToString()));
            }

            // Преобразовать параметры в строку запроса и добавить к маршруту
            if (queryData.Count > 0)
            {
                route.Append(QueryString.Create(queryData));
            }

            // Финальная строка URL
            string url = $"{_apiBaseUrl}/{route}";

            // Создание HTTP-запроса с добавлением токена в заголовки
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

            // Запрос к API
            var response = await _httpClient.SendAsync(requestMessage);

            // Обработка ответа
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Product>>>(_serializerOptions);

                if (responseData?.Successfull == true)
                {
                    Products = responseData.Data?.Items ?? new List<Product>();
                    TotalPages = responseData.Data?.TotalPages ?? 1;
                    CurrentPage = pageNo;
                    Success = true;
                }
                else
                {
                    Success = false;
                    ErrorMessage = responseData?.ErrorMessage ?? "Не удалось получить данные.";
                }
            }
            else
            {
                Success = false;
                ErrorMessage = $"Ошибка при запросе: {response.ReasonPhrase}";
            }
        }
        catch (Exception ex)
        {
            Success = false;
            ErrorMessage = $"Ошибка: {ex.Message}";
        }

        DataLoaded?.Invoke();
    }



    public async Task GetCategoryListAsync()
        {
            try
            {
                string url = $"{_apiBaseUrl}/Categories";
                var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>(url, _serializerOptions);

                if (response?.Successfull == true)
                {
                    Categories = response.Data ?? new List<Category>();
                    Success = true;
                }
                else
                {
                    Success = false;
                    ErrorMessage = response?.ErrorMessage ?? "Не удалось получить данные.";
                }
            }
            catch (Exception ex)
            {
                Success = false;
                ErrorMessage = $"Ошибка: {ex.Message}";
            }

            DataLoaded?.Invoke();
        }
    }
