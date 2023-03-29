using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace MyVdsFactory.API.Configs;

public static class ConfigureExceptionHandlerExtension
{
    public static void ConfigureExceptionHandler<T>(this WebApplication app, ILogger<T> _logger)
    {
        app.UseExceptionHandler(c => c.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                _logger.LogError(contextFeature.Error.Message);
                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new
                {
                    StatusCode = context.Response.StatusCode,
                    Messsage = contextFeature.Error.Message,
                    Title = "Hata alindi"
                }));
            }
        }));
    }
}