using backend.Dtos.Accounts;
using System.Collections.Generic;

namespace backend.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public NewAccountDto? Data { get; set; }
    }
}