using InvestmentSimulator.Backend.Hubs;
using InvestmentSimulator.Backend.Models.Domain;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace InvestmentSimulator.Backend.Services;

public class InvestmentService : IInvestmentService
{
    private readonly IUserService _userService;
    private readonly Channel<InvestmentEvent> _channel;
    private readonly IHubContext<InvestmentHub> _hubContext;
    private readonly ConcurrentDictionary<string, ActiveInvestment> _activeInvestments = new();

    private readonly List<InvestmentOption> _options = new()
    {
        new() { Name = "Short Term", RequiredAmount = 10, ExpectedReturn = 20, Duration = TimeSpan.FromSeconds(10) },
        new() { Name = "Mid Term", RequiredAmount = 100, ExpectedReturn = 250, Duration = TimeSpan.FromSeconds(30) },
        new() { Name = "Long Term", RequiredAmount = 1000, ExpectedReturn = 3000, Duration = TimeSpan.FromMinutes(1) }
    };

    public InvestmentService(
        IUserService userService,
        Channel<InvestmentEvent> channel,
        IHubContext<InvestmentHub> hubContext)
    {
        _userService = userService;
        _channel = channel;
        _hubContext = hubContext;
    }

    public IEnumerable<InvestmentOption> GetAvailableOptions() => _options;

    public IEnumerable<ActiveInvestment> GetActiveInvestments(string userId)
        => _activeInvestments.Values.Where(i => i.UserId == userId);

    public async Task RequestInvestmentAsync(string userId, string investmentName)
    {
        var option = _options.FirstOrDefault(o => o.Name.Equals(investmentName, StringComparison.OrdinalIgnoreCase));
        if (option == null)
            throw new ArgumentException("Invalid investment option");

        if (_activeInvestments.Values.Any(i => i.UserId == userId && i.InvestmentName == option.Name))
            throw new InvalidOperationException("You already have an active investment of this type");

        if (!_userService.TryDeductBalance(userId, option.RequiredAmount))
            throw new InvalidOperationException("Insufficient funds");

        var investment = new ActiveInvestment
        {
            UserId = userId,
            InvestmentName = option.Name,
            AmountInvested = option.RequiredAmount,
            ExpectedReturn = option.ExpectedReturn,
            StartedAtUtc = DateTime.UtcNow,
            EndsAtUtc = DateTime.UtcNow.Add(option.Duration)
        };

        if (_activeInvestments.TryAdd(investment.Id, investment))
        {
            await _channel.Writer.WriteAsync(new InvestmentEvent(
                userId,
                investment.InvestmentName
            ));

            var user = _userService.GetUser(userId);
            await _hubContext.Clients.Group(userId).SendAsync("BalanceUpdated", user?.Balance);
            await _hubContext.Clients.Group(userId).SendAsync("InvestmentCreated", investment);
        }
    }

    public async Task CompleteInvestmentAsync(ActiveInvestment investment)
    {
        if (_activeInvestments.TryRemove(investment.Id, out _))
        {
            var newBalance = _userService.AddBalance(investment.UserId, investment.ExpectedReturn);

            await _hubContext.Clients.Group(investment.UserId).SendAsync("InvestmentCompleted", new
            {
                investmentId = investment.Id,
                returnAmount = investment.ExpectedReturn
            });

            await _hubContext.Clients.Group(investment.UserId).SendAsync("BalanceUpdated", newBalance);
        }
    }
}
