using Microsoft.AspNetCore.SignalR;
using SER.Services;

namespace SER.Hubs
{
  public class MemberSearchHub : Hub
  {
    private readonly IProfessorService _professorService;

    public MemberSearchHub(IProfessorService professorService)
    {
      _professorService = professorService;
    }

    public async Task SearchMember(string search)
    {
      bool isSearchEmpty = string.IsNullOrWhiteSpace(search);

      if (!isSearchEmpty)
      {
        var professors = await _professorService.SearchMember(search);
        await Clients.All.SendAsync("ReceiveProfessor", professors);
      }
    }
  }
}