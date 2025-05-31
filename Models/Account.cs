using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    [Table("Accounts")]
    public class Account : IdentityUser
    {
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}