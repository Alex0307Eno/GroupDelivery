using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;

        public MerchantService(
            IUserRepository userRepo,
            IStoreRepository storeRepo)
        {
            _userRepo = userRepo;
            _storeRepo = storeRepo;
        }
        public async Task<int> CreateStoreAsync(int userId, MerchantInfoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StoreName))
                throw new Exception("店名不可為空");

            var store = new Store
            {
                StoreName = dto.StoreName,
                Phone = dto.StorePhone,
                Address = dto.StoreAddress,
                Latitude = dto.Lat,
                Longitude = dto.Lng,
                OwnerUserId = userId,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            return await _storeRepo.CreateAsync(store);
        }
        public async Task UpgradeToMerchant(
            int userId,
            UpgradeMerchantRequest request)
        {
            var user = _userRepo.GetById(userId);
            if (user == null)
                throw new Exception("User not found");

            if (user.Role == UserRole.Merchant)
                return;

            var store = await _storeRepo.GetFirstByOwnerAsync(userId);

            if (store == null)
            {
                store = new Store
                {
                    OwnerUserId = userId,
                    StoreName = request.StoreName,
                    Phone = request.StorePhone,
                    Address = request.StoreAddress,
                    Status = "Draft",
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };

                await _storeRepo.CreateAsync(store);
            }

            user.Role = UserRole.Merchant;
            _userRepo.Update(user); 
        }
    }

}
