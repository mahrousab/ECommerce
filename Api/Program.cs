using Api.Middleware;
using Api.SignalR;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetConnectionString("DefualtConnection") ??
	throw new InvalidOperationException("now connection string was found");
builder.Services.AddDbContext<StoreContext>(options =>
options.UseSqlServer(ConnectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenricRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddCors();

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
	options.InstanceName = builder.Configuration["RedisCacheOptions:InstanceName"];
});
builder.Services.AddSingleton<ICartService, CartService>();

builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<AppUser>().
	AddRoles<IdentityRole>().
	AddEntityFrameworkStores<StoreContext>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hub/notifications");

app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(""));

app.UseMiddleware<ExceptionMiddleware>(); 

try
{
	using var scope = app.Services.CreateScope();

	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<StoreContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();

	await context.Database.MigrateAsync();
	await StoreContextSeed.SeedAsync(context, userManager);

}
catch (Exception ex)
{
	Console.WriteLine(ex);
	throw;
}

app.Run();
