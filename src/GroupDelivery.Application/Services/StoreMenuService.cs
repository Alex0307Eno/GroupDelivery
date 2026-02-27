using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
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

        public async Task<List<StoreMenuItem>> GetMenuAsync(int storeId)
        {
            return await _menuRepository.GetByStoreIdAsync(storeId);
        }

        public async Task ToggleActiveAsync(int userId, int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);

            if (item == null)
                throw new Exception("找不到品項");

            var store = await _storeRepository.GetByIdAsync(item.StoreId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            item.IsActive = !item.IsActive;

            _menuRepository.Update(item);

            await _menuRepository.SaveChangesAsync();
        }

        public async Task BatchCreateAsync(int userId, int storeId, List<MenuItemDto> items)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            foreach (var dto in items)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    continue;

                if (dto.CategoryId <= 0)
                    continue;

                var menuItem = new StoreMenuItem
                {
                    StoreId = storeId,
                    CategoryId = dto.CategoryId,
                    Name = dto.Name,
                    Price = dto.Price,
                    ImageUrl = dto.ImageUrl,
                    Description = dto.Description,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    OptionGroups = new List<StoreMenuItemOptionGroup>()
                };

                // 建立選項群組
                if (dto.OptionGroups != null && dto.OptionGroups.Count > 0)
                {
                    foreach (var groupDto in dto.OptionGroups)
                    {
                        if (string.IsNullOrWhiteSpace(groupDto.GroupName))
                            continue;

                        var group = new StoreMenuItemOptionGroup
                        {
                            GroupName = groupDto.GroupName,
                            Options = new List<StoreMenuItemOption>()
                        };

                        // 建立選項
                        if (groupDto.Options != null && groupDto.Options.Count > 0)
                        {
                            foreach (var optDto in groupDto.Options)
                            {
                                if (string.IsNullOrWhiteSpace(optDto.OptionName))
                                    continue;

                                var option = new StoreMenuItemOption
                                {
                                    OptionName = optDto.OptionName,
                                    PriceAdjust = optDto.PriceAdjust
                                };

                                group.Options.Add(option);
                            }
                        }

                        menuItem.OptionGroups.Add(group);
                    }
                }

                await _menuRepository.AddAsync(menuItem);
            }

            await _menuRepository.SaveChangesAsync();
        }
        public async Task UpdateAsync(int userId, MenuItemEditDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");

            if (dto.StoreMenuItemId <= 0)
                throw new Exception("菜單品項 Id 錯誤");

            var menuItem = await _menuRepository.GetByIdAsync(dto.StoreMenuItemId);

            if (menuItem == null)
                throw new Exception("菜單品項不存在");

            var store = await _storeRepository.GetByIdAsync(menuItem.StoreId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("品名不能為空白");

            if (dto.CategoryId <= 0)
                throw new Exception("分類錯誤");

            if (dto.Price < 0)
                throw new Exception("價格不可小於 0");

            menuItem.Name = dto.Name;
            menuItem.CategoryId = dto.CategoryId;
            menuItem.Price = dto.Price;
            menuItem.Description = dto.Description;
            menuItem.IsActive = dto.IsActive;
            menuItem.UpdatedAt = DateTime.Now;

            _menuRepository.Update(menuItem);

            await _menuRepository.SaveChangesAsync();
        }

        public async Task<MenuItemEditDto> GetForEditAsync(int userId, int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            if (item == null)
                return null;

            var store = await _storeRepository.GetByIdAsync(item.StoreId);
            if (store == null)
                return null;

            if (store.OwnerUserId != userId)
                return null;

            var dto = new MenuItemEditDto();
            dto.StoreMenuItemId = item.StoreMenuItemId;
            dto.Name = item.Name;
            dto.CategoryId = item.CategoryId.HasValue ? item.CategoryId.Value : 0;
            dto.Price = item.Price;
            dto.Description = item.Description;
            dto.IsActive = item.IsActive;

            return dto;
        }

        public async Task DeleteAsync(int userId, int id)
        {
            if (id <= 0)
                throw new Exception("菜單品項 Id 錯誤");

            var menuItem = await _menuRepository.GetByIdAsync(id);
            if (menuItem == null)
                throw new Exception("菜單品項不存在");

            var store = await _storeRepository.GetByIdAsync(menuItem.StoreId);
            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限操作此店家");

            _menuRepository.Remove(menuItem);

            await _menuRepository.SaveChangesAsync();
        }
    }
}