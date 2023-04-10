using MediatR;
using MyVdsFactory.Application.Prayers.Commands.AddPrayerWithHtml;
using Quartz;

namespace MyVdsFactory.API.SchedulerServices;

public class PrayerTimeBackgroundService: IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PrayerTimeBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await GetPrayerTimesFromHtml();
    }
    
    private async Task GetPrayerTimesFromHtml()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        mediator.Send(new AddPrayerWithHtmlCommand());
    }
}