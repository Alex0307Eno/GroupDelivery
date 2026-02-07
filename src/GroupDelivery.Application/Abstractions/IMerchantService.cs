using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface IMerchantService
    {
         Task  UpgradeToMerchant(int userId, UpgradeMerchantRequest request);
        Task<int> CreateStoreAsync(int userId, MerchantInfoDto dto);

    }

}
