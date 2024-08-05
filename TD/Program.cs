using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TD.Areas.Identity.Data;
using TD.Interfaces;
using TD.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using TD.Interface;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TDContextConnection") ?? throw new InvalidOperationException("Connection string 'TDContextConnection' not found.");

builder.Services.AddDbContext<TDContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
builder.Services.AddTransient<IEmailSender, EmailSender>();


builder.Services.AddIdentity<TDUser, IdentityRole>()
    .AddEntityFrameworkStores<TDContext>()
    .AddDefaultTokenProviders();

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
