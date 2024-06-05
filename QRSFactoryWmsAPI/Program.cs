using DB.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Qiu.Utils.Security;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.Extensions.Caching.Memory;
using Qiu.Utils.Env;
using Services.Sys;
using IServices.Sys;
using Repository.Sys;
using IRepository.Sys;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMvc().AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});
builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => {
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).GetTypeInfo().Assembly);
});

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<QrsfactoryWmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString:" + builder.Configuration.GetConnectionString("ConnectionString:DbType") + "ConnectionString"))
    .EnableSensitiveDataLogging() // ��������������־��¼
           .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
           ); // ʹ�ÿ���̨��־��¼;

builder.Services.AddScoped<Xss>();
// ע�� ISys ����
builder.Services.AddScoped<ISys_LogRepository, Sys_LogRepository>();
builder.Services.AddScoped<ISys_UserRepository, Sys_UserRespository>();
builder.Services.AddScoped<ISys_IdentityRepository, Sys_IdentityRepository>();
builder.Services.AddScoped<ISys_MenuRepository, Sys_MenuRepository>();
builder.Services.AddScoped<ISys_RoleRepository, Sys_RoleRepository>();
builder.Services.AddScoped<ISys_RoleMenuRepository, Sys_RoleMenuRepository>();

builder.Services.AddScoped<ISys_LogService, Sys_LogService>();
builder.Services.AddScoped<ISys_UserService, Sys_UserService>();
builder.Services.AddScoped<ISys_IdentityService, Sys_IdentityService>();
builder.Services.AddScoped<ISys_MenuService, Sys_MenuService>();
builder.Services.AddScoped<ISys_RoleService, Sys_RoleService>();
builder.Services.AddScoped<ISys_RoleMenuService, Sys_RoleMenuService>();
var app = builder.Build();

GlobalCore.Configure(app);
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
