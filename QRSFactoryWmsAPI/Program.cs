using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Qiu.Utils.Env;
using Qiu.Utils.Json;
using Qiu.Utils.Jwt;
using Qiu.Utils.Security;
using Repository.Sys;
using Services.Sys;
using System.Reflection;
using System.Text;




var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyConfig =>
    {
        policyConfig.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true, //是否验证Issuer
        ValidIssuer = configuration["Jwt:Issuer"], //发行人Issuer
        ValidateAudience = false,
        ValidAudience = configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true, //是否验证SecurityKey
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), //SecurityKey
        ValidateLifetime = true, //是否验证失效时间
        ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
        RequireExpirationTime = true,
    };
    // 启用日志记录
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var user = context.Principal;
            // 记录令牌验证成功时的用户信息
            Console.WriteLine("Token validated successfully for user: ");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            // 记录令牌验证失败时的信息
            Console.WriteLine("Token validation failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
}).AddCookie();


builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});

builder.Services.AddSingleton<JsonConfig>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton(new Jwt(builder.Configuration));

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).GetTypeInfo().Assembly);
});

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDbContext<QrsfactoryWmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString:" + builder.Configuration.GetConnectionString("ConnectionString:DbType") + "ConnectionString"))
    .EnableSensitiveDataLogging() // 启用敏感数据日志记录
           .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
           ); // 使用控制台日志记录;

builder.Services.AddScoped<Xss>();
// 注册 ISys 服务
builder.Services.AddScoped<ISys_LogRepository, Sys_LogRepository>();
builder.Services.AddScoped<ISys_UserRepository, Sys_UserRespository>();
builder.Services.AddScoped<ISys_IdentityRepository, Sys_IdentityRepository>();
builder.Services.AddScoped<ISys_MenuRepository, Sys_MenuRepository>();
builder.Services.AddScoped<ISys_RoleRepository, Sys_RoleRepository>();
builder.Services.AddScoped<ISys_RoleMenuRepository, Sys_RoleMenuRepository>();
builder.Services.AddScoped<ISys_DeptRepository, Sys_DeptRepository>();

builder.Services.AddScoped<ISys_LogService, Sys_LogService>();
builder.Services.AddScoped<ISys_UserService, Sys_UserService>();
builder.Services.AddScoped<ISys_IdentityService, Sys_IdentityService>();
builder.Services.AddScoped<ISys_MenuService, Sys_MenuService>();
builder.Services.AddScoped<ISys_RoleService, Sys_RoleService>();
builder.Services.AddScoped<ISys_RoleMenuService, Sys_RoleMenuService>();
builder.Services.AddScoped<ISys_DeptService, Sys_DeptService>();
var app = builder.Build();

GlobalCore.Configure(app);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
