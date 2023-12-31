using AutoMobile.Application.Services.Interface;
using AutoMobile.Application.Services;
using AutoMobile.Domain.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMobile.Application.ApplicationContants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region AutoMapper

builder.Services.AddAutoMapper(typeof(MappingProfile));

#endregion

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpClient<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

#region Configure Authorization


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(CustomClaimPolicy.JuniorManagerPolicy, policy =>
    {
        policy.RequireClaim(CustomClaimType.ManagerType, CustomClaimValue.JuniorManager);
    });

    options.AddPolicy(CustomClaimPolicy.SeniorManagerPolicy, policy =>
    {
        policy.RequireClaim(CustomClaimType.ManagerType, CustomClaimValue.SeniorManager);
    });

    options.AddPolicy(CustomClaimPolicy.AssistantManagerPolicy, policy =>
    {
        policy.RequireClaim(CustomClaimType.ManagerType, CustomClaimValue.AssistantManager);
    });

    options.AddPolicy(CustomClaimPolicy.AssociateProductManagerPolicy, policy =>
    {
        policy.RequireClaim(CustomClaimType.ManagerType, CustomClaimValue.AssociateProductManager);
    });
});

#endregion


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
