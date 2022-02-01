
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
});

builder.Services.AddWebOptimizer();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
// Seed Admin
try
{
    var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    context.Database.EnsureCreated();

    var role = new IdentityRole("Administrator");
    // Create Administrator role if it doesn't exist
    if (!roleManager.RoleExistsAsync(role.Name).Result)
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
