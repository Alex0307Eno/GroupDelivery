using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repo;

        public StoreService(IStoreRepository repo)
        {
            _repo = repo;
        }

        public int CreateStore(int userId, StoreInitRequest request)
        {
            var store = new Store
            {
                OwnerUserId = userId,
                StoreName = request.StoreName,
                Phone = request.Phone,
                Address = request.Address,
                Status = "Draft",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _repo.Add(store);

            return store.StoreId;
        }
    }

}
