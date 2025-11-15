using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Data;
using MyOwnMessageBroker.Models;
using MyOwnMessageBroker.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddScoped<TopicService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(
    builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();