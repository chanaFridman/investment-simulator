namespace InvestmentSimulator.Backend.Models.DTOs;

public record BalanceUpdatedDto(
    string UserId,
    decimal Balance
);