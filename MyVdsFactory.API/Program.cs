using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using MyVdsFactory.API.Configs;
using MyVdsFactory.API.Configs.ColumnWriters;
using MyVdsFactory.API.Services;
using MyVdsFactory.Application;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAuthenticationConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddSettingsConfig(builder.Configuration);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddTransient<IFileServices, FileServices>();

builder.Services.AddCors(options =>
    options.AddPolicy("myclients", builder =>
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

Logger log = new LoggerConfiguration()
    .WriteTo.PostgreSQL(
        builder.Configuration.GetConnectionString("PostgreSql"),
        "Logs",
        needAutoCreateTable:true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"Message", new RenderedMessageColumnWriter()},
            {"MessageTemplate",new MessageTemplateColumnWriter()},
            {"Level", new LevelColumnWriter()},
            {"TimeStamp", new TimestampColumnWriter()},
            {"Exception", new ExceptionColumnWriter()},
            {"LogEvent", new LogEventSerializedColumnWriter()},
            {"Username", new UsernameColumnWriter()}
        })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseSerilogRequestLogging();
app.UseHttpLogging();

/*app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
    var response = new { error = exception.Message };
    await context.Response.WriteAsJsonAsync(response);
}));*/

app.UseSwagger();
app.UseSwaggerUI();

app.MigrateDatabase();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("myclients");
app.UseAuthentication();
app.UseAuthorization();

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("Username", username);
    await next();
});
    
app.MapControllers();

app.Run();