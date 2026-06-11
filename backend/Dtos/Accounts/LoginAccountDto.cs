using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Accounts
{
    public class LoginAccountDto
    {
        [Required]
        public string? Identifier { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}