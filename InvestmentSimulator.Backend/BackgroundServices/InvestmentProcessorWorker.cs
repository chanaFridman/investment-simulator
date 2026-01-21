using InvestmentSimulator.Backend.Models.Domain;
using InvestmentSimulator.Backend.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace InvestmentSimulator.Backend.BackgroundServices;

public class InvestmentProcessorWorker : BackgroundService
{
    private readonly Channel<InvestmentEvent> _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InvestmentProcessorWorker> _logger;

    public InvestmentProcessorWorker(
        Channel<InvestmentEvent> channel,
        IServiceProvider serviceProvider,
        ILogger<InvestmentProcessorWorker> logger)
    {
        _channel = channel;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var evt in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            _ = HandleInvestmentAsync(evt, stoppingToken);
        }
    }

    private async Task HandleInvestmentAsync(InvestmentEvent evt, CancellationToken token)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var investmentService = scope.ServiceProvider.GetRequiredService<IInvestmentService>();

            var activeInvestments = investmentService.GetActiveInvestments(evt.UserId);
            var investment = activeInvestments.FirstOrDefault(i => i.InvestmentName == evt.InvestmentName);

            if (investment == null)
                return;

            var delay = investment.EndsAtUtc - DateTime.UtcNow;
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, token);

            await investmentService.CompleteInvestmentAsync(investment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing investment for user {UserId}", evt.UserId);
        }
    }
}
