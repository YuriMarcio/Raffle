using Raffle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raffle.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string userId);
    }
}
