/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- This code configures and sets up the PcPulse application.
- It establishes database connections, sets up Identity authentication, adds session management, and configures error handling.
- The application uses Entity Framework Core for database operations and Identity for user authentication.
- Session management is configured with a timeout of 10 minutes.
- Notyf toast notifications are added with a duration of 5 seconds, positioned at the top-right corner.
- Default roles are seeded to the database on application startup.

Functions:
- ConfigureServices: Configures services required by the application such as database context, Identity, session, and Notyf.
- Configure: Configures middleware for HTTP request processing, including error handling, HTTPS redirection, static files, authentication, authorization, and routing.
- SeedDatabase: Seeds default roles to the database on application startup.

*/

using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PcPulse.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PcPulseDatabaseConnection") ?? throw new InvalidOperationException("Sorry, Connection string 'DatabaseConnection' not found.");

builder.Services.AddDbContext<PcPulseDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)

    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PcPulseDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();


builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = service.GetRequiredService<PcPulseDbContext>();

        //Creating database 
        if (context.Database.CanConnect())
        {
            Console.WriteLine(" Sorry, The database already exists.");
        }
        else
        {
            context.Database.Migrate();
        }
        //Adding default roles
        await ContextSeed.seedRolesAsync(service.GetRequiredService<RoleManager<IdentityRole>>());

    }
    catch (Exception ex)
    {
        var logger = LoggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Sorry ,An error occurred seeding the DB.Please try again");
    }

}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();
app.MapRazorPages();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
