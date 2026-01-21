namespace InvestmentSimulator.Backend.Models.Domain;

public class User
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public string Name { get; init; } = string.Empty;
    public decimal Balance { get; set; } = 1000;
}
