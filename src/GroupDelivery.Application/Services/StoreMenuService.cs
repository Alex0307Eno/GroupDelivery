using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Exceptions;
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
            return await _menuRepository.GetByStoreIdAsync(storeId);
        }

        public async Task ToggleActiveAsync(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            if (item == null)
                throw new BusinessException("找不到品項");

            item.IsActive = !item.IsActive;
            await _menuRepository.UpdateAsync(item);
        }

        public async Task BatchCreateAsync(int userId, int storeId, List<MenuItemDto> items)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                throw new BusinessException("店家不存在");

            if (store.OwnerUserId != userId)
                throw new BusinessException("無權限操作此店家");

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
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                await _menuRepository.AddAsync(menuItem);
            }

            await _menuRepository.SaveChangesAsync();
        }

        public async Task ReorderCategoriesAsync(int userId, List<CategoryReorderRequest> request)
        {
            if (request == null || request.Count == 0)
                throw new BusinessException("排序資料不可為空");

            var categories = await _menuRepository.GetCategoriesByIdsAsync(request.Select(x => x.CategoryId).Distinct().ToList());
            if (categories.Count != request.Select(x => x.CategoryId).Distinct().Count())
                throw new BusinessException("分類資料不完整");

            var ownedStore = await _storeRepository.GetFirstByOwnerAsync(userId);
            if (ownedStore == null)
                throw new BusinessException("找不到商家店鋪");

            if (categories.Any(x => x.StoreId != ownedStore.StoreId))
                throw new BusinessException("分類不屬於目前商家");

            await _menuRepository.ReorderCategoriesAsync(request);
        }

        public async Task BatchSetCategoryActiveAsync(int userId, List<CategoryActiveRequest> request)
        {
            if (request == null || request.Count == 0)
                throw new BusinessException("啟用停用資料不可為空");

            var categories = await _menuRepository.GetCategoriesByIdsAsync(request.Select(x => x.CategoryId).Distinct().ToList());
            if (categories.Count != request.Select(x => x.CategoryId).Distinct().Count())
                throw new BusinessException("分類資料不完整");

            var ownedStore = await _storeRepository.GetFirstByOwnerAsync(userId);
            if (ownedStore == null)
                throw new BusinessException("找不到商家店鋪");

            if (categories.Any(x => x.StoreId != ownedStore.StoreId))
                throw new BusinessException("分類不屬於目前商家");

            await _menuRepository.BatchUpdateCategoryActiveAsync(request);
        }

        public async Task<List<MenuItemAvailableDto>> GetAvailableItemsAsync(int userId, string time)
        {
            var ownedStore = await _storeRepository.GetFirstByOwnerAsync(userId);
            if (ownedStore == null)
                throw new BusinessException("找不到商家店鋪");

            TimeSpan queryTime;
            if (string.IsNullOrWhiteSpace(time))
            {
                var taipeiTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Taipei Standard Time");
                queryTime = taipeiTime.TimeOfDay;
            }
            else if (!TimeSpan.TryParse(time, out queryTime))
            {
                throw new BusinessException("time 格式錯誤，請使用 HH:mm");
            }

            var items = await _menuRepository.GetAvailableItemsAsync(ownedStore.StoreId, queryTime);
            return items.Select(x => new MenuItemAvailableDto
            {
                StoreMenuItemId = x.StoreMenuItemId,
                StoreId = x.StoreId,
                CategoryId = x.CategoryId,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                IsActive = x.IsActive,
                AvailableStartTime = x.AvailableStartTime,
                AvailableEndTime = x.AvailableEndTime
            }).ToList();
        }

        public async Task TransferCategoryAsync(int userId, CategoryTransferRequest request)
        {
            if (request.SourceCategoryId == request.TargetCategoryId)
                throw new BusinessException("來源分類與目標分類不可相同");

            var categories = await _menuRepository.GetCategoriesByIdsAsync(new List<int> { request.SourceCategoryId, request.TargetCategoryId });
            if (categories.Count != 2)
                throw new BusinessException("分類不存在");

            var ownedStore = await _storeRepository.GetFirstByOwnerAsync(userId);
            if (ownedStore == null)
                throw new BusinessException("找不到商家店鋪");

            if (categories.Any(x => x.StoreId != ownedStore.StoreId))
                throw new BusinessException("分類不屬於目前商家");

            await _menuRepository.TransferCategoryAsync(request.SourceCategoryId, request.TargetCategoryId);
        }
    }
}
