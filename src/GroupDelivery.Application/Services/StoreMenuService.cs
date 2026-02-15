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

        public async Task<List<StoreMenuItem>> GetAvailableMenuItemsAsync(int storeId, TimeSpan? atTime)
        {
            // 使用台灣時區（UTC+8）作為未提供時間時的預設值
            var targetTime = atTime ?? DateTime.UtcNow.AddHours(8).TimeOfDay;

            // 先檢查資料時間設定是否完整，避免資料錯誤造成顯示異常
            var allItems = await _menuRepository.GetAllByStoreIdAsync(storeId);
            foreach (var item in allItems)
            {
                ValidateTimeWindow(item.AvailableStartTime, item.AvailableEndTime);
            }

            return await _menuRepository.GetAvailableMenuItemsAsync(storeId, targetTime);
        }

        public async Task<List<StoreMenuCategory>> GetCategoriesAsync(int storeId, bool? isActive)
        {
            return await _menuRepository.GetCategoriesByStoreAsync(storeId, isActive);
        }

        public async Task ToggleActiveAsync(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            if (item == null)
                throw new BusinessException("找不到品項");

            item.IsActive = !item.IsActive;
            await _menuRepository.UpdateAsync(item);
        }

        public async Task BatchCreateAsync(int storeId, List<MenuItemDto> items)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
                throw new BusinessException("店家不存在");

            foreach (var dto in items)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    continue;

                ValidateTimeWindow(dto.AvailableStartTime, dto.AvailableEndTime);

                var menuItem = new StoreMenuItem
                {
                    StoreId = storeId,
                    Name = dto.Name,
                    Price = dto.Price,
                    Description = dto.Description,
                    AvailableStartTime = dto.AvailableStartTime,
                    AvailableEndTime = dto.AvailableEndTime,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                await _menuRepository.AddAsync(menuItem);
            }

            await _menuRepository.SaveChangesAsync();
        }

        public async Task CreateCategoryAsync(int storeId, string name, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessException("分類名稱不可為空");

            var trimmedName = name.Trim();
            var isDuplicate = await _menuRepository.CategoryNameExistsAsync(storeId, trimmedName);
            if (isDuplicate)
                throw new BusinessException("同店家分類名稱不可重複");

            var category = new StoreMenuCategory
            {
                StoreId = storeId,
                Name = trimmedName,
                SortOrder = sortOrder,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _menuRepository.AddCategoryAsync(category);
            await _menuRepository.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int storeId, int categoryId)
        {
            var category = await _menuRepository.GetCategoryByIdAsync(categoryId);
            if (category == null || category.StoreId != storeId)
                throw new BusinessException("找不到分類");

            var hasMenuItems = await _menuRepository.ExistsMenuItemByCategoryAsync(categoryId);
            if (hasMenuItems)
                throw new BusinessException("此分類下尚有菜單品項，無法刪除");

            _menuRepository.RemoveCategory(category);
            await _menuRepository.SaveChangesAsync();
        }

        public async Task ReorderCategoriesAsync(int storeId, List<CategoryReorderRequest> requests)
        {
            if (requests == null || requests.Count == 0)
                throw new BusinessException("排序資料不可為空");

            var categoryIds = requests.Select(x => x.CategoryId).Distinct().ToList();
            if (categoryIds.Count != requests.Count)
                throw new BusinessException("排序資料含有重複分類");

            // 驗證分類是否全部屬於目前商家
            var categories = await _menuRepository.GetCategoriesByIdsAsync(storeId, categoryIds);
            if (categories.Count != requests.Count)
                throw new BusinessException("排序資料包含非本店分類");

            try
            {
                await _menuRepository.ReorderCategoriesAsync(storeId, requests);
            }
            catch (InvalidOperationException)
            {
                throw new BusinessException("排序資料包含非本店分類");
            }
        }

        public async Task ToggleCategoryActiveAsync(int storeId, List<CategoryActiveUpdateRequest> requests)
        {
            if (requests == null || requests.Count == 0)
                throw new BusinessException("啟用停用資料不可為空");

            var categoryIds = requests.Select(x => x.CategoryId).Distinct().ToList();
            if (categoryIds.Count != requests.Count)
                throw new BusinessException("啟用停用資料含有重複分類");

            var categories = await _menuRepository.GetCategoriesByIdsAsync(storeId, categoryIds);
            if (categories.Count != requests.Count)
                throw new BusinessException("啟用停用資料包含非本店分類");

            try
            {
                await _menuRepository.ToggleCategoryActiveAsync(storeId, requests);
            }
            catch (InvalidOperationException)
            {
                throw new BusinessException("啟用停用資料包含非本店分類");
            }
        }

        public async Task TransferCategoryItemsAsync(int storeId, CategoryTransferRequest request)
        {
            if (request == null)
                throw new BusinessException("轉移資料不可為空");

            if (request.SourceCategoryId == request.TargetCategoryId)
                throw new BusinessException("來源分類與目標分類不可相同");

            var categories = await _menuRepository.GetCategoriesByIdsAsync(
                storeId,
                new List<int> { request.SourceCategoryId, request.TargetCategoryId });

            if (categories.Count != 2)
                throw new BusinessException("來源或目標分類不屬於本店");

            await _menuRepository.TransferCategoryItemsAsync(
                storeId,
                request.SourceCategoryId,
                request.TargetCategoryId);
        }

        private void ValidateTimeWindow(TimeSpan? start, TimeSpan? end)
        {
            // 時段規則：兩者皆空代表全天；兩者皆有值代表指定時段
            if (start == null && end == null)
                return;

            if (start == null || end == null)
                throw new BusinessException("供應時段需同時設定開始與結束時間");

            // Start <= End 為同日；Start > End 為跨日，兩者皆合法
        }
    }
}
