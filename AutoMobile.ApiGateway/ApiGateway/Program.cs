using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var proxyBuilder = builder.Services.AddReverseProxy();

proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicy", policy =>
         policy.RequireAuthenticatedUser());
});

builder.Services.AddCors();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(); // Add the CORS middleware here

app.MapReverseProxy().RequireCors("CustomCorsPolicy");

app.MapGet("/", () => "Hello World!");

app.Run();
