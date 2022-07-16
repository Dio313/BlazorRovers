using System.ComponentModel.DataAnnotations;

namespace Common.AuthViewModels;

public class LoginVm
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    public string Password { get; set; }
}