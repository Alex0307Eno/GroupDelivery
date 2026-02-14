using GroupDelivery.Infrastructure.Data;
using isRock.LineBot;
using LineBotService.Handlers;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<isRock.LineBot.Bot>(provider =>
{
    var cfg = provider.GetRequiredService<IConfiguration>();
    return new isRock.LineBot.Bot(cfg["LineBot:ChannelAccessToken"]);
});

builder.Services.AddScoped<MessageHandler>();
builder.Services.AddScoped<PostbackHandler>();
builder.Services.AddDbContext<GroupDeliveryDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});


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

app.Run();