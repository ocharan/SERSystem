namespace SER.Configuration
{
  public static class ExceptionLogger
  {
    public static void LogException(Exception ex)
    {
      string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Configuration", "Exceptions");

      if (!Directory.Exists(folderPath))
      {
        Directory.CreateDirectory(folderPath);
      }

      string filePath = Path.Combine(folderPath, "log_exception_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
      string separator = "+-------------------------------------------------------------------------------------------------+";
      string logText = ex.ToString() + Environment.NewLine + separator + Environment.NewLine;

      File.AppendAllText(filePath, logText);
    }
  }
}