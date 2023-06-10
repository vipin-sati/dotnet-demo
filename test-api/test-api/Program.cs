using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.SwaggerGen;
using test_api.Authentication;
using test_api.Dapper;
using test_api.GlobalErrorHandler;
using test_api.GlobalErrorHandler.Extensions;
using test_api.Interfaces;
using test_api.Interfaces.Repositories;
using test_api.Repositories;
using test_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<IDbConnection, SqlConnection>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAuthentication("BasicAuthentication").
    AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


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

var logger = app.Services.GetRequiredService<ILogger<ErrorDetails>>();

app.ConfigureExceptionHandler(logger);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
