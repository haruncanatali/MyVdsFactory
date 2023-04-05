using MyVdsFactory.API.SchedulerServices;
using Quartz;

namespace MyVdsFactory.API.Configs;

public static class SchedulerConfig
{
    public static IServiceCollection AddSchedulerConfig(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();
            var jobKey = new JobKey("SchedulerHoroscopeCommentariesFromHtml");
            q.AddJob<HoroscopeBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SchedulerHoroscopeCommentariesFromHtml-trigger")
                .WithCronSchedule("0 0 8 ? * *")
            );
        });
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionScopedJobFactory();
            var jobKey = new JobKey("SchedulerEarthquakesFromHtml");
            q.AddJob<EarthquakeBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("SchedulerEarthquakesFromHtml-trigger")
                .WithCronSchedule("0 * * ? * *")
            );
        });
        
        services.AddTransient<HoroscopeBackgroundService>();
        services.AddTransient<EarthquakeBackgroundService>();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
}