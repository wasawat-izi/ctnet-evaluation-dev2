using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Accounts
{
    public class AppUserResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public GetAccountDto? Data { get; set; }
    }
}