using AutoMobile.Domain.EventHandlers;
using AutoMobile.Domain.Events;
using AutoMobile.Domain.Interface;
using AutoMobile.Infrastructure.Common;
using AutoMobile.Infrastructure.Repository;
using MediatR;
using MicroRabbit.Bus;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IEventBus, RabbitMQBus>(x =>
{
    var scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
    return new RabbitMQBus(x.GetService<IMediator>(), scopeFactory);
});

builder.Services.AddTransient<OrderEventHandler>();

builder.Services.AddTransient<IEventHandler<OrderCreatedEvent>, OrderEventHandler>();

builder.Services.AddTransient<IOrderDetailsRepository, OrderDetailsRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ConfigureEventBus(app);

app.Run();


void ConfigureEventBus(WebApplication app)
{
    var eventBus = app.Services.GetRequiredService<IEventBus>();

    eventBus.Subscriber<OrderCreatedEvent, OrderEventHandler>();
}