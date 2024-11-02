

using Microsoft.AspNetCore.Http;
using Web_253502_Yarashuk.UI.Services.Authentication;

namespace Web_253502_Yarashuk.UI.Services.FileService;



public class ApiFileService : IFileService
{
    private readonly HttpClient _httpClient;

    private readonly ITokenAccessor _tokenAccessor;

    public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
    {

        _tokenAccessor = tokenAccessor;
        _httpClient = httpClient;
    }

    public async Task DeleteFileAsync(string fileName)
    {

        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var requestUri = new Uri(_httpClient.BaseAddress, $"/api/Files/DeleteFile?fileName={fileName}");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = requestUri
        };


        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Ошибка при удалении файла: {response.ReasonPhrase}");
        }
    }

    public async Task<string> SaveFileAsync(IFormFile formFile)
    {

        await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post
        };

        var extension = Path.GetExtension(formFile.FileName);
        var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(formFile.OpenReadStream());
        content.Add(streamContent, "file", newName);
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return string.Empty;
    }
}

