using InvestmentSimulator.Backend.BackgroundServices;
using InvestmentSimulator.Backend.Models.Domain;
using InvestmentSimulator.Backend.Services;
using System.Threading.Channels;

namespace InvestmentSimulator.Backend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IInvestmentService, InvestmentService>();
        services.AddHostedService<InvestmentProcessorWorker>();

        services.AddSingleton(Channel.CreateUnbounded<InvestmentEvent>());

        return services;
    }
}
