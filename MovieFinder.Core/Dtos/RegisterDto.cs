using System.ComponentModel.DataAnnotations;

namespace MovieFinder.Core.Dtos;

public class RegisterDto
{
    [Required]
    [Display(Name = "Fullname")]
    public string GivenName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}