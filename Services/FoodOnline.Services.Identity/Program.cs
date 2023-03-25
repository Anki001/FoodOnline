using Duende.IdentityServer.Services;
using FoodOnline.Services.Identity.Common;
using FoodOnline.Services.Identity.DBContexts;
using FoodOnline.Services.Identity.Initializer;
using FoodOnline.Services.Identity.Initializer.Interfaces;
using FoodOnline.Services.Identity.Models;
using FoodOnline.Services.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

var identityBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;    
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(Constants.IdentityResources)
.AddInMemoryApiScopes(Constants.ApiScopes)
.AddInMemoryClients(Constants.Clients)
.AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IProfileService, ProfileService>();

identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Add identity server to pipeline
app.UseIdentityServer();
app.UseAuthorization();
SeedDatabase();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}
