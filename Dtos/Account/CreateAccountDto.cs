using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class CreateAccountDto : AccountBaseDto
    {
        [Required]
        public string? Password { get; set; }
    }
}