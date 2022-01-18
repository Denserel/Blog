
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRepository, Repository>();

var app = builder.Build();

try
{
    var context = app.Services.GetRequiredService<AppDbContext>();
    var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = app.Services.GetRequiredService<UserManager<IdentityUser>>();

    context.Database.EnsureCreated();

    var role = new IdentityRole("Administrator");
    // Create Administrator role if it doesn't exist
    if (!roleManager.RoleExistsAsync("Administrator").Result)
    {
        roleManager.CreateAsync(role).Wait();
    }

    // Create Administrator user if it doesn't exist
    if (userManager.FindByNameAsync("administrator").Result == null)
    {
        var user = new IdentityUser
        {
            UserName = "administrator",
            Email = "administrator@email.com"
        };
        userManager.CreateAsync(user, "Strong0Password!").Wait();

        userManager.AddToRoleAsync(user, role.Name).Wait();
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

app.UseAuthentication();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
