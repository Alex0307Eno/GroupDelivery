using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public UserRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> GetOrCreateByEmail(string email)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
                return user;

            user = new User
            {
                Email = email,
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
