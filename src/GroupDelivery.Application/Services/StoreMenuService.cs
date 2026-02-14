using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class StoreMenuService : IStoreMenuService
    {
        private readonly IStoreMenuRepository _menuRepository;
        private readonly IStoreRepository _storeRepository;

        public StoreMenuService(IStoreMenuRepository menuRepository, IStoreRepository storeRepository)
        {
            _menuRepository = menuRepository;
            _storeRepository = storeRepository;
        }

        public async Task CreateMenuItemAsync(int userId, int storeId, string name, decimal price, string description)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            var item = new StoreMenuItem
            {
                StoreId = storeId,
                Name = name,
                Price = price,
                Description = description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _menuRepository.AddAsync(item);
        }

        public async Task<List<StoreMenuItem>> GetMenuAsync(int storeId)
        {
            return await _menuRepository.GetByStoreIdAsync(storeId);
        }
        public async Task ToggleActiveAsync(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);

            if (item == null)
                throw new Exception("找不到品項");

            item.IsActive = !item.IsActive;

            await _menuRepository.UpdateAsync(item);
        }
        public async Task BatchCreateAsync(
    int userId,
    int storeId,
    List<MenuItemDto> items)
        {
            // 驗證店家是否存在
            var store = await _storeRepository.GetByIdAsync(storeId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            foreach (var dto in items)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    continue;

                var menuItem = new StoreMenuItem
                {
                    StoreId = storeId,
                    Name = dto.Name,
                    Price = dto.Price,
                    Description = dto.Description,
                    CreatedAt = DateTime.Now
                };

                await _menuRepository.AddAsync(menuItem);
            }

            await _menuRepository.SaveChangesAsync();
        }

    }
}
