using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    [Table("Accounts")]
    public class Account : IdentityUser
    {
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}