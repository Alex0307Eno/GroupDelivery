using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _menuRepository.GetByStoreIdWithOptionsAsync(storeId);
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

            // 一次把群組、選項都 Include 進來
            var menuItem = await _menuRepository.GetWithOptionsAsync(dto.StoreMenuItemId);
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

            // 更新基本欄位
            menuItem.Name = dto.Name;
            menuItem.CategoryId = dto.CategoryId;
            menuItem.Price = dto.Price;
            menuItem.Description = dto.Description;
            menuItem.IsActive = dto.IsActive;
            menuItem.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.ImageUrl))
            {
                menuItem.ImageUrl = dto.ImageUrl;
            }

            // 確保導覽屬性不為 null
            if (menuItem.OptionGroups == null)
            {
                menuItem.OptionGroups = new List<StoreMenuItemOptionGroup>();
            }

            // 前端傳回來的群組列表
            var dtoGroups = dto.OptionGroups ?? new List<StoreMenuItemOptionGroupDto>();

            // 先抓目前資料庫裡的群組快照，用來判斷哪些被刪除
            var existingGroups = new List<StoreMenuItemOptionGroup>(menuItem.OptionGroups);

            // 處理每一個前端傳回來的群組
            foreach (var dtoGroup in dtoGroups)
            {
                StoreMenuItemOptionGroup groupEntity = null;

                if (dtoGroup.StoreMenuItemOptionGroupId > 0)
                {
                    // 找到既有群組就更新
                    groupEntity = existingGroups
                        .FirstOrDefault(g => g.StoreMenuItemOptionGroupId == dtoGroup.StoreMenuItemOptionGroupId);
                }

                if (groupEntity == null)
                {
                    // 新增群組
                    groupEntity = new StoreMenuItemOptionGroup();
                    groupEntity.StoreMenuItemId = menuItem.StoreMenuItemId;
                    groupEntity.Options = new List<StoreMenuItemOption>();

                    menuItem.OptionGroups.Add(groupEntity);
                }

                groupEntity.GroupName = dtoGroup.GroupName;

                // 處理選項
                if (groupEntity.Options == null)
                {
                    groupEntity.Options = new List<StoreMenuItemOption>();
                }

                var dtoOptions = dtoGroup.Options ?? new List<StoreMenuItemOptionDto>();
                var existingOptions = new List<StoreMenuItemOption>(groupEntity.Options);

                // 新增或更新選項
                foreach (var dtoOption in dtoOptions)
                {
                    StoreMenuItemOption optionEntity = null;

                    if (dtoOption.StoreMenuItemOptionId > 0)
                    {
                        optionEntity = existingOptions
                            .FirstOrDefault(o => o.StoreMenuItemOptionId == dtoOption.StoreMenuItemOptionId);
                    }

                    if (optionEntity == null)
                    {
                        optionEntity = new StoreMenuItemOption();
                        groupEntity.Options.Add(optionEntity);
                    }

                    optionEntity.OptionName = dtoOption.OptionName;
                    optionEntity.PriceAdjust = dtoOption.PriceAdjust;
                }

                // 刪除頁面上已經拿掉的選項
                foreach (var optionEntity in existingOptions)
                {
                    var existInDto = dtoOptions
                        .Any(o => o.StoreMenuItemOptionId == optionEntity.StoreMenuItemOptionId);

                    if (!existInDto)
                    {
                        groupEntity.Options.Remove(optionEntity);
                    }
                }
            }

            // 刪除頁面上已經移除的群組
            foreach (var groupEntity in existingGroups)
            {
                var existInDto = dtoGroups
                    .Any(g => g.StoreMenuItemOptionGroupId == groupEntity.StoreMenuItemOptionGroupId);

                if (!existInDto)
                {
                    menuItem.OptionGroups.Remove(groupEntity);
                }
            }

            // 交給 Repository 寫回資料庫
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

            var menuItem = await _menuRepository.GetWithOptionsAsync(id);
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
        public async Task<StoreMenuItem> GetByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }
    }
}