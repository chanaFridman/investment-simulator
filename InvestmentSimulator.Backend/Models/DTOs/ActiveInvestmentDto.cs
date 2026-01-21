namespace InvestmentSimulator.Backend.Models.DTOs;

public record ActiveInvestmentDto(
    string Id,
    string UserId,
    string InvestmentName,
    decimal AmountInvested,
    decimal ExpectedReturn,
    DateTime StartedAtUtc,
    DateTime EndsAtUtc
);