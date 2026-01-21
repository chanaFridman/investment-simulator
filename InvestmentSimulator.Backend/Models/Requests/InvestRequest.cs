using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulator.Backend.Models.Requests;

public record InvestRequest(
    [Required(ErrorMessage = "UserId is required")]
    string UserId,
    
    [Required(ErrorMessage = "InvestmentName is required")]
    [StringLength(100, MinimumLength = 1)]
    string InvestmentName
);
