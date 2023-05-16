using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SER.Services.Contracts
{
  public interface IOfficeDocumentService
  {
    Task<byte[]> CreateSpreadsheet<T>(string sheetName, string fileName, string[] headers, List<T> data);
  }
}