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
        #region 取得指定使用者資料
        public User GetById(int userId)
        {
            return _db.Users.Find(userId);
        }
        #endregion
        #region 根據電子郵件取得使用者資料，若不存在則建立新使用者
        public void Update(User user)
        {
            _db.Update(user);
            _db.SaveChanges();
        }
        #endregion

        #region 非同步版本的取得指定使用者資料
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
        #endregion

        #region 根據電子郵件取得使用者資料，若不存在則建立新使用者（非同步版本）
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
        #endregion

        #region 根據 LINE User ID 取得使用者資料，若不存在則建立新使用者（非同步版本）
        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
        #endregion
    }
}
