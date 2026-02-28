using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
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

        #region 建立商家第一間商店
        public async Task<int> CreateStoreAsync(int userId, MerchantInfoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.StoreName))
                throw new Exception("店名不可為空");

            var store = new Store
            {
                StoreName = dto.StoreName,
                Landline = dto.Landline,
                Mobile = dto.Mobile,
                Address = dto.StoreAddress,


                OwnerUserId = userId,


                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            return await _storeRepo.CreateAsync(store);
        }
        #endregion

        #region 升級為商家
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
                    Landline = request.Landline,
                    Mobile = request.Mobile,
                    Address = request.StoreAddress,


                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                await _storeRepo.CreateAsync(store);
            }

            user.Role = UserRole.Merchant;
            _userRepo.Update(user);
        }
        #endregion
    }
}
