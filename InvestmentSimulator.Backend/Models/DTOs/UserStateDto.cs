namespace InvestmentSimulator.Backend.Models.DTOs;

public record UserStateDto(
    string UserId,
    string Name,
    decimal Balance
);