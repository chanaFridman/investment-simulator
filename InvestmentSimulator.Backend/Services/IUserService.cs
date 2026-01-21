using InvestmentSimulator.Backend.Models.Domain;

namespace InvestmentSimulator.Backend.Services;

public interface IUserService
{
    User CreateOrGetUser(string name);
    User? GetUser(string userId);
    bool TryDeductBalance(string userId, decimal amount);
    decimal AddBalance(string userId, decimal amount);
}
