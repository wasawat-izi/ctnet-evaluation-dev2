using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Dtos.Accounts;

namespace backend.Dtos.Auth
{
    public class AuthResultDto
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public NewAccountDto? Data { get; set; }
    }
}