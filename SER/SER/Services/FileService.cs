using SER.Models.Responses;

namespace SER.Services
{
  public class FileService : IFileService
  {
    private readonly long _fileSizeLimit;
    private IWebHostEnvironment _environment;

    public FileService(IConfiguration config, IWebHostEnvironment environment)
    {
      _environment = environment;
      _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
    }

    public async Task<string> SaveFile(IFormFile file, string name)
    {
      string wwwPath = _environment.WebRootPath;
      string nameFolder = "files";
      string directoryPath = Path.Combine(wwwPath, nameFolder);
      string fileName = name + "_" + file.FileName;

      if (!Directory.Exists(directoryPath)) { Directory.CreateDirectory(directoryPath); }

      string fullPath = $"{directoryPath}\\{fileName}";
      string path = fullPath.Substring(fullPath.IndexOf("wwwroot"));

      using (var fileStream = new FileStream(fullPath, FileMode.Create))
      {
        await file.CopyToAsync(fileStream);
      }

      return path;
    }

    public Task<Response> ValidateFile(IFormFile file)
    {
      Response response = new();

      if (file.Length > _fileSizeLimit)
      {
        response.Errors.Add(new FieldError
        {
          FieldName = "fileUpload",
          Message = $"El tamaño máximo permitido del archivo es de {_fileSizeLimit / 1024 / 1024} mb"
        });
      }

      if (!file.ContentType.Contains("pdf"))
      {
        response.Errors.Add(new FieldError
        {
          FieldName = "fileUpload",
          Message = "El archivo debe ser en formato PDF"
        });
      }

      if (response.Errors.Count <= 0)
      {
        response.IsSuccess = true;
      }

      return Task.FromResult(response);
    }
  }
}