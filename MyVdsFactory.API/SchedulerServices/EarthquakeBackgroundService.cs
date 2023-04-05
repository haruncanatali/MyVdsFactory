using MediatR;
using MyVdsFactory.Application.Earthquakes.Commands.AddEarthquakeWithHtml;
using Quartz;

namespace MyVdsFactory.API.SchedulerServices;

public class EarthquakeBackgroundService: IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EarthquakeBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await GetEarthquakesFromHtml();
    }
    
    private async Task GetEarthquakesFromHtml()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        mediator.Send(new AddEarthquakeWithHtmlCommand());
    }
}