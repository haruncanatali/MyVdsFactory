namespace MyVdsFactory.API.Services;

public interface IFileServices
{
    Task<bool> SaveFileAsync(IFormFile file, string modalPath);
    Task<string?> SaveFileAsyncWithReturnFilePath(IFormFile file, string modalPath);
}