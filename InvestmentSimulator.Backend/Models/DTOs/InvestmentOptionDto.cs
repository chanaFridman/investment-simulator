namespace InvestmentSimulator.Backend.Models.DTOs;

public record InvestmentOptionDto(
    string Name,
    decimal RequiredAmount,
    decimal ExpectedReturn,
    double DurationSeconds
);
