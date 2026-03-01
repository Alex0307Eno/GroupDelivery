using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroupDelivery.Application.Abstractions;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class LoginTokenUsageRepository:ILoginTokenUsageRepository
    {
        private readonly GroupDeliveryDbContext _db;
        public LoginTokenUsageRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public async Task<bool> IsUsedAsync(string nonce)
        {
            return await _db.LoginTokenUsages
                .AnyAsync(x => x.Nonce == nonce && x.Used);
        }

        public async Task MarkUsedAsync(string nonce, DateTime expire)
        {
            var entity = new LoginTokenUsage
            {
                Nonce = nonce,
                ExpireAt = expire,
                Used = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.LoginTokenUsages.Add(entity);
            await _db.SaveChangesAsync();
        }
    }
}
