namespace InvestmentSimulator.Backend.Models.Domain;

public class ActiveInvestment
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string InvestmentName { get; init; } = string.Empty;
    public decimal AmountInvested { get; init; }
    public decimal ExpectedReturn { get; init; }
    public DateTime StartedAtUtc { get; init; }
    public DateTime EndsAtUtc { get; init; }
}
