using MediatR;
using MyVdsFactory.Application.HoroscopeCommentaries.Commands.AddRangeHoroscopeCommentaryWithHtml;
using Quartz;

namespace MyVdsFactory.API.SchedulerServices;

public class HoroscopeBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HoroscopeBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await GetHoroscopeCommentariesFromHtml();
    }
    
    private async Task GetHoroscopeCommentariesFromHtml()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        mediator.Send(new AddRangeHoroscopeCommentaryWithHtmlCommand(){ });
    }
}