using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SER.Models.Responses
{
  public class FieldError
  {
    public string FieldName { get; set; } = null!;
    public string Message { get; set; } = null!;
  }
}