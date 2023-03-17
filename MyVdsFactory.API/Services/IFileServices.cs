namespace MyVdsFactory.API.Services;

public interface IFileServices
{
    Task<bool> SaveFile(IFormFile file, string modalPath);
}