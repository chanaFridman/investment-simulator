using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulator.Backend.Models.Requests;

public record LoginRequest(
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Name must contain only English letters")]
    string Name
);