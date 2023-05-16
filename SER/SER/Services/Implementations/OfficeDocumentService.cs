using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SER.Services.Contracts;

namespace SER.Services.Implementations
{
  public class OfficeDocumentService : IOfficeDocumentService
  {
    public async Task<byte[]> CreateSpreadsheet<T>(string sheetName, string fileName, string[] headers, List<T> data)
    {
      XLWorkbook workbook = new XLWorkbook();
      IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);

      for (int i = 0; i < headers.Length; i++)
      {
        worksheet.Cell(1, i + 1).Value = headers[i];
      }

      for (int i = 0; i < data.Count; i++)
      {
        var properties = data[i]!.GetType().GetProperties();

        for (int j = 0; j < properties.Length; j++)
        {
          // worksheet.Cell(i + 2, j + 1).Value = (XLCellValue)properties[j].GetValue(data[i])!;
        }
      }

      using (MemoryStream memoryStream = new MemoryStream())
      {
        workbook.SaveAs(memoryStream);
        return await Task.FromResult(memoryStream.ToArray());
      }
    }
  }
}