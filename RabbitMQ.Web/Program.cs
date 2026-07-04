using RabbitMQ.Application.Interface;
using RabbitMQ.Application.Service;
using RabbitMQ.Infrastructure.RabbitMQ;
using RabbitMQ.Infrastructure.RabbitMQ.Interface;
using RabbitMQ.Infrastructure.RabbitMQ.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();//چون کانکشن هست یدونه کافیه
builder.Services.AddSingleton<IRabbitMqTopology, RabbitMqTopology>();

builder.Services.AddScoped<IRabbitProducer, RabbitProducer>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
