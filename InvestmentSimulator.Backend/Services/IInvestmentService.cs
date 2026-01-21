using InvestmentSimulator.Backend.Models.Domain;

namespace InvestmentSimulator.Backend.Services;

public interface IInvestmentService
{
    IEnumerable<InvestmentOption> GetAvailableOptions();
    IEnumerable<ActiveInvestment> GetActiveInvestments(string userId);
    Task RequestInvestmentAsync(string userId, string investmentName);
    Task CompleteInvestmentAsync(ActiveInvestment investment);
}