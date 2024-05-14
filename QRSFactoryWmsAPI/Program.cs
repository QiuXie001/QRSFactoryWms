using IResponsitory;
using IServices;
using DB.Models;
using Microsoft.AspNetCore.Hosting;
using Qiu.Utils.Security;
using Responsitory;
using Services;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).GetTypeInfo().Assembly);
});

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<QrsfactoryWmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString:"+ builder.Configuration.GetConnectionString("ConnectionString:DbType")+ "ConnectionString")));

builder.Services.AddScoped<Xss>();
// зЂВс ISys ЗўЮё
builder.Services.AddScoped<ISys_LogResponsitory, Sys_LogResponsitory>();
builder.Services.AddScoped<ISys_UserResponsitory, Sys_UserRespository>();
builder.Services.AddScoped<ISys_LogService, Sys_LogService>();
builder.Services.AddScoped<ISys_UserService, Sys_UserService>();

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
