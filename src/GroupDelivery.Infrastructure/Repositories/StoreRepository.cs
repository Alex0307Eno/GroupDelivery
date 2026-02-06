using GroupDelivery.Domain;
using GroupDelivery.Infrastructure.Data;
using GroupDelivery.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Infrastructure.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GroupDeliveryDbContext _db;

        public StoreRepository(GroupDeliveryDbContext db)
        {
            _db = db;
        }

        public void Add(Store store)
        {
            _db.Stores.Add(store);
            _db.SaveChanges();
        }

        public Store GetByOwner(int ownerUserId)
        {
            return _db.Stores.FirstOrDefault(x => x.OwnerUserId == ownerUserId);
        }
        public List<Store> GetByOwnerUserId(int ownerUserId)
        {
            return _db.Stores
                .Where(s => s.OwnerUserId == ownerUserId)
                .ToList();
        }

    }

}
