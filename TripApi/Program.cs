using Microsoft.EntityFrameworkCore;
using TripApi.Models;
using TripApi.Services;
using TripApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<TripDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();