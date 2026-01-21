namespace InvestmentSimulator.Backend.Models.Domain;

public class InvestmentOption
{
    public string Name { get; init; } = string.Empty;
    public decimal RequiredAmount { get; init; }
    public decimal ExpectedReturn { get; init; }
    public TimeSpan Duration { get; init; }
}
