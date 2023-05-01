namespace SER.Models.Responses
{
  public class Response
  {
    public List<FieldError> Errors { get; set; } = new List<FieldError>();
    public bool IsSuccess { get; set; }
  }
}