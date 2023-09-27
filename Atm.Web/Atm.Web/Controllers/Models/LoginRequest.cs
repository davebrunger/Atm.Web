namespace Atm.Web.Controllers.Models;

public class LoginRequest
{
    [Required]
    [MaxLength(16)]
    [MinLength(16)]
    [RegularExpression("^\\d{16}$")]
    public string AccountNumber { get; init; } = null!;

    [Required]
    [MaxLength(4)]
    [MinLength(4)]
    [RegularExpression("^\\d{4}$")]
    public string Pin { get; init; } = null!;
}