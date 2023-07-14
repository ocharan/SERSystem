using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SER.Configuration;
using SER.Services;
using SER.Models.DB;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SER.Hubs;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SER.Services.Implementations;
using SER.Services.Contracts;

namespace SER
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _env = env;
    }

    public IConfiguration Configuration { get; }
    private readonly IWebHostEnvironment _env;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Database
      services.AddDbContext<SERContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("ApplicationContext")));

      // Authentication
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
      {
        option.LoginPath = "/Index";
        option.AccessDeniedPath = "/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);

      });
      services.AddAuthorization();

      // AutoMapper
      services.AddAutoMapper(typeof(Startup));

      // Services API
      services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
      services.AddTransient<IEmailService, EmailService>();
      services.Configure<TokenSettings>(Configuration.GetSection(nameof(TokenSettings)));
      services.AddTransient<ITokenService, TokenService>();
      services.AddScoped<IFileService, FileService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IStudentService, StudentService>();
      services.AddScoped<ICourseService, CourseService>();
      services.AddScoped<IProfessorService, ProfessorService>();
      services.AddScoped<IAcademicBodyService, AcademicBodyService>();
      services.AddScoped<IOfficeDocumentService, OfficeDocumentService>();

      // SignalR
      services.AddSignalR();

      // Framework services
      // services.AddRazorPages();
      services.AddRazorPages(options =>
      {
        options.Conventions.AddPageApplicationModelConvention("/Courses/Create",
          model =>
          {
            model.Filters.Add(new RequestSizeLimitAttribute(107374182));
          }
        );
      });

      // Size file limit    
      services.Configure<KestrelServerOptions>(options =>
      {
        options.Limits.MaxRequestBodySize = 1073741824;
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // app.UseExceptionHandler("/Error");
        app.UseDeveloperExceptionPage();
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();
      }

      app.UseStatusCodePagesWithReExecute("/errors/NotFound");
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
        endpoints.MapHub<ProfessorSearchHub>("/professorSearchHub");
        endpoints.MapHub<StudentSearchHub>("/studentSearchHub");
        endpoints.MapHub<MemberSearchHub>("/memberSearchHub");
      });
    }
  }
}
