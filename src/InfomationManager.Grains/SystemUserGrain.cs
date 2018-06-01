using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfomationManager.Abstractions;
using InfomationManager.Models;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace InfomationManager.Grains
{
    public class SystemUserGrain : Grain<SystemUser>, ISystemUserGrain
    {
        private readonly ApplicationDbContext _context;
        public SystemUserGrain(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Add(SystemUser user)
        {
            _context.Add(user);
            return _context.SaveChangesAsync();
        }

        public Task<List<SystemUser>> Get()
        {
            return _context.Users.ToListAsync();
        }

        public Task<SystemUser> GetById(Guid Id)
        {
            return _context.Users.FindAsync(Id);
        }

        public Task Update(SystemUser user)
        {
            _context.Users.Update(user);
            return _context.SaveChangesAsync();
        }
    }
}
