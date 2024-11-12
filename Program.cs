using HAIRCRAFT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Dodajemy us�ug� logowania
builder.Services.AddLogging(config =>
{
    config.AddConsole(); // Dodaj logowanie do konsoli
    config.AddDebug();   // Dodaj logowanie do debugowania
    config.AddEventSourceLogger(); // Opcjonalnie dla bardziej zaawansowanego logowania
});

// Dodaj kontekst bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Zarejestruj swoj� klas� User zamiast IdentityUser
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Konfiguracja opcji dotycz�cych hase�
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.SignIn.RequireConfirmedAccount = false;  // Bez potwierdzania konta
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Dodaj kontrolery i widoki
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
