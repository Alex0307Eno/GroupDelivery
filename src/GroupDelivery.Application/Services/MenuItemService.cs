using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IStoreRepository _storeRepository;

        public MenuItemService(
            IMenuItemRepository menuItemRepository,
            IStoreRepository storeRepository)
        {
            _menuItemRepository = menuItemRepository;
            _storeRepository = storeRepository;
        }

        public async Task<List<MenuItem>> GetActiveAsync(int ownerUserId, int? categoryId)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            return await _menuItemRepository.GetActiveByStoreAsync(store.StoreId, categoryId);
        }

        public async Task<List<MenuItem>> GetDeletedAsync(int ownerUserId)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            return await _menuItemRepository.GetDeletedByStoreAsync(store.StoreId);
        }

        public async Task<MenuItem> GetByIdAsync(int ownerUserId, int id)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            return await _menuItemRepository.GetByIdAsync(id, store.StoreId);
        }

        public async Task CreateAsync(int ownerUserId, MenuItem item)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            await ValidateMenuItemAsync(item, store.StoreId);

            item.StoreId = store.StoreId;
            item.IsDeleted = false;
            item.DeletedAt = null;
            item.CreatedAt = DateTime.Now;

            await _menuItemRepository.AddAsync(item);
        }

        public async Task UpdateAsync(int ownerUserId, MenuItem item)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            var current = await _menuItemRepository.GetByIdAsync(item.Id, store.StoreId);

            if (current == null || current.IsDeleted)
            {
                throw new InvalidOperationException("找不到可更新的菜單資料。");
            }

            await ValidateMenuItemAsync(item, store.StoreId);

            current.Name = item.Name;
            current.Description = item.Description;
            current.CategoryId = item.CategoryId;
            current.OriginalPrice = item.OriginalPrice;
            current.IsOnSale = item.IsOnSale;
            current.SalePrice = item.SalePrice;
            current.ImageUrl = item.ImageUrl;

            await _menuItemRepository.UpdateAsync(current);
        }

        public async Task DeleteAsync(int ownerUserId, int id)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            var item = await _menuItemRepository.GetByIdAsync(id, store.StoreId);

            if (item == null || item.IsDeleted)
            {
                return;
            }

            // 採用軟刪除以保留未來稽核與還原能力。
            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now;
            await _menuItemRepository.UpdateAsync(item);
        }

        public async Task RestoreAsync(int ownerUserId, int id)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            var item = await _menuItemRepository.GetByIdAsync(id, store.StoreId);

            if (item == null || !item.IsDeleted)
            {
                return;
            }

            // 還原時僅調整刪除狀態，不改動其他商業資料。
            item.IsDeleted = false;
            item.DeletedAt = null;
            await _menuItemRepository.UpdateAsync(item);
        }

        public async Task<List<Category>> GetCategoriesAsync(int ownerUserId)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);
            return await _menuItemRepository.GetCategoriesByStoreAsync(store.StoreId);
        }

        public async Task CreateCategoryAsync(int ownerUserId, string name)
        {
            var store = await GetCurrentStoreAsync(ownerUserId);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("分類名稱不可為空白。");
            }

            var existing = await _menuItemRepository.GetCategoryByNameAsync(store.StoreId, name.Trim());
            if (existing != null)
            {
                throw new InvalidOperationException("分類名稱已存在。");
            }

            var category = new Category
            {
                StoreId = store.StoreId,
                Name = name.Trim(),
                CreatedAt = DateTime.Now
            };

            await _menuItemRepository.AddCategoryAsync(category);
        }

        private async Task<Store> GetCurrentStoreAsync(int ownerUserId)
        {
            var store = await _storeRepository.GetFirstByOwnerAsync(ownerUserId);
            if (store == null)
            {
                throw new InvalidOperationException("目前帳號尚未建立商家資料。");
            }

            return store;
        }

        private async Task ValidateMenuItemAsync(MenuItem item, int storeId)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new InvalidOperationException("菜單名稱不可為空白。");
            }

            if (item.OriginalPrice <= 0)
            {
                throw new InvalidOperationException("OriginalPrice 必須大於 0。");
            }

            if (!item.IsOnSale && item.SalePrice.HasValue)
            {
                throw new InvalidOperationException("非特價狀態時 SalePrice 必須為空值。");
            }

            if (item.IsOnSale)
            {
                if (!item.SalePrice.HasValue)
                {
                    throw new InvalidOperationException("特價狀態時 SalePrice 不可為空值。");
                }

                if (item.SalePrice.Value <= 0 || item.SalePrice.Value >= item.OriginalPrice)
                {
                    throw new InvalidOperationException("SalePrice 必須大於 0 且小於 OriginalPrice。");
                }
            }

            var category = await _menuItemRepository.GetCategoryByIdAsync(item.CategoryId, storeId);
            if (category == null)
            {
                throw new InvalidOperationException("分類不存在或不屬於目前商家。");
            }
        }
    }
}
