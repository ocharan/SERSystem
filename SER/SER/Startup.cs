using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SER.Configuration;
using SER.Services;
using SER.Models.DB;

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

      // Services
      services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
      services.AddTransient<IEmailService, EmailService>();
      services.Configure<TokenSettings>(Configuration.GetSection(nameof(TokenSettings)));
      services.AddTransient<ITokenService, TokenService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IStudentService, StudentService>();
      services.AddScoped<ICourseService, CourseService>();

      // Framework services
      services.AddRazorPages();
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

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapRazorPages();
      });

      // app.UseStatusCodePagesWithRedirects("Errors/NotFound");
    }
  }
}
