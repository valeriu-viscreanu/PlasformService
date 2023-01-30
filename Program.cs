using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
var consuleUri = new Uri(builder.Configuration.GetSection("Consul")["URL"]);
builder.Services.AddSingleton<IHostedService, ConsulServiceRegister>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>( 
    p => new ConsulClient(cfg => cfg.Address = consuleUri));
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.Configure<PlatformConfig>(
     builder.Configuration.GetSection("Platform"));
builder.Services.Configure<ConsulConfig>(
     builder.Configuration.GetSection("Consul"));

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>{
    options.UseInMemoryDatabase("InMem");
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var count = 0;
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.
    Console.WriteLine("INvoked" + ++count);
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});

app.MapControllers();
PrepDb.PrepPopulation(app);
app.Run();
