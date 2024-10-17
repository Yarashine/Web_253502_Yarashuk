namespace Web_253502_Yarashuk.UI.Services.FileService;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile formFile);
    Task DeleteFileAsync(string fileName);
}
