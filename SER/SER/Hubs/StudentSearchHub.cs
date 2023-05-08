using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SER.Models.DB;
using SER.Models.DTO;
using SER.Services;

namespace SER.Hubs
{
  public class StudentSearchHub : Hub
  {
    private readonly IStudentService _studentService;

    public StudentSearchHub(IStudentService studentService)
    {
      _studentService = studentService;
    }

    public async Task SearchStudent(string search)
    {
      bool isSearchEmpty = string.IsNullOrEmpty(search);

      if (!isSearchEmpty)
      {
        var students = await _studentService.SearchStudent(search);
        await Clients.All.SendAsync("ReceiveStudents", students);
      }
    }
  }
}