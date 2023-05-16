using SER.Models.Responses;

namespace SER.Services
{
  public interface IFileService
  {
    Task<Response> ValidateFile(IFormFile file);
    Task<string> SaveFile(IFormFile file, string name, string nameFolder);
    string DeleteFile(string path);
  }
}