using Microsoft.AspNetCore.Diagnostics;
using MyVdsFactory.API.Configs;
using MyVdsFactory.API.Services;
using MyVdsFactory.Application;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddSettingsConfig(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>(); 

builder.Services.AddCors(options =>
    options.AddPolicy("myclients", builder =>
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseSwagger();
app.UseSwaggerUI();

app.MigrateDatabase();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("myclients");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();