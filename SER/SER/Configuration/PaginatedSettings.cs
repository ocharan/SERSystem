namespace ContosoUniversity
{
  public class PaginatedSettings
  {
    public string? SortOrder { get; set; }
    public string? CurrentSearch { get; set; }
    public string? SearchString { get; set; }
    public int? PageIndex { get; set; }
    public string? CurrentFilter { get; set; }
  }
}