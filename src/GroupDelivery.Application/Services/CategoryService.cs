using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class CategoryService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreMenuCategoryRepository _categoryRepository;

        public CategoryService(
    IStoreRepository storeRepository,
    IStoreMenuCategoryRepository categoryRepository)
        {
            _storeRepository = storeRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task CreateAsync(int userId, int storeId, string name)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);

            if (store == null)
                throw new Exception("店家不存在");

            if (store.OwnerUserId != userId)
                throw new Exception("無權限");

            var category = new StoreMenuCategory
            {
                StoreId = storeId,
                Name = name,
                CreatedAt = DateTime.Now
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

    }
}
