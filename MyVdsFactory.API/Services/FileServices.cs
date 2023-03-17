namespace MyVdsFactory.API.Services;

public class FileServices : IFileServices
{
    private readonly IWebHostEnvironment _environment;

    public FileServices(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<bool> SaveFile(IFormFile file,string modalPath)
    {
        try
        {
            string directoryPath = Path.Combine(_environment.ContentRootPath, $"DataResources/{modalPath}");
            string filePath = Path.Combine(directoryPath, file.FileName);
            using (var stream = new FileStream(filePath,FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}