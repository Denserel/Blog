using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    context.Database.EnsureCreated();

    var adminRole = new IdentityRole("Admin");

    if (!context.Roles.Any())
    {
        // Create Admin role
        roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
    }

    if (!context.Users.Any(user => user.UserName == "admin"))
    {
        //Create admin user
        var adminUser = new IdentityUser
        {
            UserName = "admin",
            Email = "admin@email.com"
        };
        userManager.CreateAsync(adminUser, "Strong0Password!").GetAwaiter().GetResult();
        // Add Admin role to admin user
        userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseAuthentication();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
