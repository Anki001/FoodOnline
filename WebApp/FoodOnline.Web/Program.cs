using FoodOnline.Web.Common;
using FoodOnline.Web.Services;
using FoodOnline.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IShopingCartService, ShopingCartService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();

Constants.ProductApiBase = builder.Configuration["ServiceUrls:ProductAPI"];
Constants.ShopingCartApiBase = builder.Configuration["ServiceUrls:ShopingCartAPI"];
Constants.CouponApiBase = builder.Configuration["ServiceUrls:CouponService"];

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopingCartService, ShopingCartService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", options => options.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
        options.GetClaimsFromUserInfoEndpoint= true;
        options.ClientId = "foodonline";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        // UserId and Role claim does not exists in users claims
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("foodonline");
        options.SaveTokens = true;
    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
