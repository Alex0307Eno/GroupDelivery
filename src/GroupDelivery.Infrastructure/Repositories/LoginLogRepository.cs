using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class LoginLogRepository : ILoginLogRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public LoginLogRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(LoginLog log)
        {
            _db.LoginLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}