using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IStoreClosedDateRepository
    {
        Task<bool> ExistsAsync(int storeId, DateTime closedDate);
        Task AddAsync(StoreClosedDate entity);
        Task DeleteAsync(int storeClosedDateId);
        Task<List<StoreClosedDate>> GetByStoreIdAsync(int storeId);


    }

}
