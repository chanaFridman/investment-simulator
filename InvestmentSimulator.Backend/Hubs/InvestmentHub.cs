using Microsoft.AspNetCore.SignalR;

namespace InvestmentSimulator.Backend.Hubs;

public class InvestmentHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }
}
