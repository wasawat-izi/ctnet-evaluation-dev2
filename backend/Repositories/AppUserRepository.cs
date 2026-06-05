using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Interfaces;
using backend.Models;

namespace backend.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDBContext _context;

        public AppUserRepository(ApplicationDBContext context)
        {
            _context = context;
        }
    }
}