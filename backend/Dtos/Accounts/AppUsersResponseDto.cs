using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos.Accounts
{
    public class AppUsersResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public List<GetAccountDto>? Data { get; set; }
    }
}