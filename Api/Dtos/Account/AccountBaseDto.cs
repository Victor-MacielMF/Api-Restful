using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class AccountBaseDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        
        public string? Email { get; set; }
    }
}