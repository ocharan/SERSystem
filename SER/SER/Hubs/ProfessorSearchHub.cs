using Microsoft.AspNetCore.SignalR;
using SER.Services;

namespace SER.Hubs
{
  public class ProfessorSearchHub : Hub
  {
    private readonly IProfessorService _professorService;

    public ProfessorSearchHub(IProfessorService professorService)
    {
      _professorService = professorService;
    }

    public async Task SearchProfessor(string search)
    {
      bool isSearchEmpty = string.IsNullOrWhiteSpace(search);

      if (!isSearchEmpty)
      {
        var professors = await _professorService.SearchProfessor(search);
        await Clients.All.SendAsync("ReceiveProfessor", professors);
      }
    }
  }
}