using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace GroupDelivery.Application.Services
{
    // 此服務類別用於處理開團相關業務邏輯
    public class GroupOrderService: IGroupOrderService
    {
        private readonly IGroupOrderRepository _groupOrderRepository;
        private readonly IUserRepository _userRepo;
        private readonly IStoreRepository _storeRepo;
        private readonly IStoreMenuRepository _storeMenuRepo;
        private readonly IStoreMenuService _menuService;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryRuleRepository _deliveryRuleRepository;

        public GroupOrderService(IGroupOrderRepository groupOrderRepository, IUserRepository userRepository, IStoreRepository storeRepository,IStoreMenuRepository storeMenuRepository, IStoreMenuService menuService, IOrderRepository orderRepository, IDeliveryRuleRepository deliveryRuleRepository)
        {
            _groupOrderRepository = groupOrderRepository; ;
            _userRepo = userRepository;
            _storeRepo = storeRepository;
            _storeMenuRepo = storeMenuRepository;
            _menuService = menuService;
            _orderRepository = orderRepository;
            _deliveryRuleRepository = deliveryRuleRepository;
        }
        public async Task<int> CreateAsync(CreateUserGroupRequest request, int userId)
        {
            var store = await _storeRepo.GetByIdAsync(request.StorePublicId);
            if (store == null)
                throw new Exception("店家不存在");

            // ⬇ 新增時間限制
            var minDeadline = DateTime.Now.AddMinutes(30);
            if (request.Deadline <= minDeadline)
                throw new Exception("截止時間必須至少晚於現在 30 分鐘");

            var rules = await _deliveryRuleRepository
                .GetByStoreIdAsync(request.StorePublicId);

            if (rules == null || !rules.Any())
                throw new Exception("未設定外送門檻");

            var rule = rules.First();

            var entity = new GroupOrder
            {
                StoreId = request.StorePublicId,
                CreatorUserId = userId,
                OwnerUserId = store.OwnerUserId,

                TargetAmount = rule.MinimumOrderAmount,
                CurrentAmount = 0,

                Deadline = request.Deadline,
                Status = GroupOrderStatus.Open,
                Remark = request.Remark,
                CreatedAt = DateTime.Now
            };

            await _groupOrderRepository.AddAsync(entity);

            return entity.GroupOrderId;
        }
        #region 取得指定團單的詳細資料，供團單詳情頁顯示
        public async Task<GroupDetailDto> GetGroupDetailAsync(Guid publicId)
        {
            var group = await _groupOrderRepository.GetDetailAsync(publicId);
            if (group == null)
                return null;
            var hasMenu = await _storeMenuRepo.AnyActiveByStoreIdAsync(group.StoreId);
            return new GroupDetailDto
            {
                GroupOrderPublicId = group.GroupOrderPublicId,
                TargetAmount = group.TargetAmount,
                CurrentAmount = group.CurrentAmount,
                Deadline = group.Deadline,
                JoinCount = group.GroupOrderItems?.Count ?? 0,
                HasMenu = hasMenu,

                Store = group.Store == null ? null : new StoreDto
                {
                    Name = group.Store.StoreName,
                    Landline = group.Store.Landline,
                    Mobile = group.Store.Mobile,
                    Address = group.Store.Address
                }
            };
        }
        #endregion

        #region 建立一筆新的揪團，僅允許商家開團
        public async Task CreateGroupAsync(int userId, CreateGroupRequest request)
        {
            var store = await _storeRepo.GetByIdAsync(request.StoreId);

            if (store == null)
                throw new Exception("店家不存在");

            var isMerchant = await _userRepo.IsMerchantAsync(userId);

            if (isMerchant)
            {
                if (store.OwnerUserId != userId)
                    throw new Exception("無權限操作此店家");
            }

            var deadline = request.Deadline;
            if (deadline <= DateTime.Now)
                throw new Exception("截止時間必須晚於現在");

            var now = DateTime.Now;

            if (!IsStoreOpenNow(store, now))
                throw new Exception("店家目前已打烊，無法開團");

            var group = new GroupOrder
            {
                GroupOrderPublicId = Guid.NewGuid(),
                StoreId = request.StoreId,
                CreatorUserId = userId,
                OwnerUserId = store.OwnerUserId, 
                TargetAmount = request.TargetAmount,
                CurrentAmount = 0,               
                Deadline = deadline,
                Status = GroupOrderStatus.Open,
                CreatedAt = DateTime.Now,
                Remark = request.Remark
            };

            await _groupOrderRepository.AddAsync(group);
        }
        #endregion

        #region 將已超過截止時間的揪團標記為過期狀態
        public async Task ExpireOverdueGroupsAsync()
        {
            var now = DateTime.UtcNow;

            var overdueGroups = await _groupOrderRepository
                .GetActiveOverdueAsync(now);

            foreach (var group in overdueGroups)
            {
                group.Status = GroupOrderStatus.Expired;
            }

        }
        #endregion
        #region 取得我開的團列表
        public async Task<List<GroupOrder>> GetMyGroupOrdersAsync(int userId)
        {
            return await _groupOrderRepository.GetByCreatorAsync(userId);
        }
        #endregion
        #region 加入團單，新增訂單明細並更新團金額，判斷是否成團
        public async Task JoinGroupAsync(int userId, int groupOrderId, decimal amount)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupOrderId);

            if (group == null)
                throw new Exception("團不存在");

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("此團無法加入");

            if (group.Deadline <= DateTime.Now)
                throw new Exception("已截止");

            // 新增訂單明細
            var item = new GroupOrderItem
            {
                GroupOrderId = groupOrderId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            await _groupOrderRepository.AddItemAsync(item);

            // 更新團金額
            group.CurrentAmount += amount;

            // 判斷是否成團
            if (group.CurrentAmount >= group.TargetAmount)
            {
                group.Status = GroupOrderStatus.Success;
            }

            await _groupOrderRepository.UpdateAsync(group);
        }
        #endregion
        #region 取得指定商店的所有開團中團單
        public async Task<List<GroupOrder>> GetOpenGroupsByStoreAsync(int storeId)
        {
            var now = DateTime.Now;

            return await _groupOrderRepository
                .GetOpenByStoreAsync(storeId, now);
        }
        #endregion
        public async Task<GroupDetailDto> GetDetailAsync(int groupId)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupId);

            return new GroupDetailDto
            {
                GroupOrderPublicId = group.GroupOrderPublicId,
                Deadline = group.Deadline,
                CurrentAmount = group.CurrentAmount,
                TargetAmount = group.TargetAmount
            };
        }
        public async Task JoinGroupAsync(int userId, int groupId)
        {
            var group = await _groupOrderRepository.GetByIdAsync(groupId);

            if (group.Status != GroupOrderStatus.Open)
                throw new Exception("團已結束");

            group.CurrentAmount += 100; // 暫時固定金額

            await _groupOrderRepository.UpdateAsync(group);
        }

        // 新增：首頁附近開團列表
        public async Task<List<GroupSummaryDto>> GetOpenGroupsAsync(double? lat, double? lng)
        {
            var groups = await _groupOrderRepository.GetOpenGroupsWithStoreAsync();

            var result = groups.Select(x =>
            {
                var dto = new GroupSummaryDto();

                dto.GroupOrderPublicId = x.GroupOrderPublicId;
                dto.TargetAmount = x.TargetAmount;
                dto.CurrentAmount = x.CurrentAmount;
                dto.Deadline = x.Deadline;
                dto.Remark = x.Remark;

                dto.Store = new StoreSummaryDto
                {
                    PublicId = x.Store.StorePublicId,
                    StoreName = x.Store.StoreName,
                    CoverImageUrl = x.Store.CoverImageUrl,
                    Latitude = x.Store.Latitude,
                    Longitude = x.Store.Longitude
                };

                return dto;
            }).ToList();

            // 有定位才算距離
            if (lat.HasValue && lng.HasValue)
            {
                foreach (var g in result)
                {
                    if (g.Store.Latitude.HasValue && g.Store.Longitude.HasValue)
                    {
                        g.Distance = CalculateDistance(
                            lat.Value,
                            lng.Value,
                            g.Store.Latitude.Value,
                            g.Store.Longitude.Value);
                    }
                    else
                    {
                        g.Distance = 9999;
                    }
                }

                result = result
                    .OrderBy(x => x.Distance)
                    .ToList();
            }

            return result;
        }
        public async Task<GroupMenuDto> GetMenuAsync(Guid groupOrderPublicId)
        {
            var group = await _groupOrderRepository.GetByPublicIdAsync(groupOrderPublicId);

            if (group == null)
                return null;

            var store = await _storeRepo.GetByIdAsync(group.StoreId);

            if (store == null)
                return null;

            var menuItems = await _menuService.GetMenuAsync(store.StoreId);

            var grouped = menuItems
                .GroupBy(m => new
                {
                    CategoryId = m.CategoryId ?? 0,
                    CategoryName = m.Category != null ? m.Category.Name : "其他"
                })
                .Select(g => new GroupMenuCategoryDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    Items = g.Select(m => new GroupMenuItemDto
                    {
                        StoreMenuItemPublicId = m.StoreMenuItemPublicId,
                        Name = m.Name,
                        Price = m.Price,
                        Description = m.Description,
                        ImageUrl = m.ImageUrl,

                        OptionGroups = m.OptionGroups != null
                            ? m.OptionGroups.Select(og => new GroupMenuOptionGroupDto
                            {
                                GroupName = og.GroupName,
                                Options = og.Options != null
                                    ? og.Options.Select(o => new GroupMenuOptionDto
                                    {
                                        StoreMenuItemOptionId = o.StoreMenuItemOptionId,
                                        OptionName = o.OptionName,
                                        PriceAdjust = o.PriceAdjust
                                    }).ToList()
                                    : new List<GroupMenuOptionDto>()
                            }).ToList()
                            : new List<GroupMenuOptionGroupDto>()
                    }).ToList()
                })
                .OrderBy(c => c.CategoryId)
                .ToList();

            return new GroupMenuDto
            {
                GroupOrderId = group.GroupOrderId,
                StoreId = store.StoreId,
                StoreName = store.StoreName,
                StoreAddress = store.Address,
                Categories = grouped
            };
        }
        public async Task<GroupOrder> GetByIdAsync(int id)
        {
            return await _groupOrderRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(GroupOrder groupOrder)
        {
            await _groupOrderRepository.UpdateAsync(groupOrder);
        }
        #region Business Logic
        // 設定取餐方式，僅允許訂單擁有者修改
        public async Task SetTakeModeAsync(int userId, int orderId, TakeMode takeMode)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("訂單不存在");

            if (order.UserId != userId)
                throw new Exception("無權限操作此訂單");

            order.TakeMode = takeMode;

            await _orderRepository.UpdateAsync(order);
        }

        // 手動關團，僅允許團主操作
        public async Task CloseGroupAsync(int userId, Guid groupPublicId)
        {
            var groupOrder = await _groupOrderRepository.GetByPublicIdAsync(groupPublicId);

            if (groupOrder == null)
                throw new Exception("揪團不存在");

            if (groupOrder.OwnerUserId != userId)
                throw new Exception("無權限操作此揪團");

            if (groupOrder.Status != GroupOrderStatus.Open)
                throw new Exception("揪團已結束");

            // 判斷是否成團
            if (groupOrder.CurrentAmount >= groupOrder.TargetAmount)
            {
                groupOrder.Status = GroupOrderStatus.Success;
            }
            else
            {
                groupOrder.Status = GroupOrderStatus.Open;
            }

            groupOrder.Deadline = DateTime.Now;

            await _groupOrderRepository.UpdateAsync(groupOrder);
        }
        #endregion

        private bool IsStoreOpenNow(Store store, DateTime now)
        {
            if (store.IsPausedToday)
                return false;

            var today = (int)now.DayOfWeek;

            if (!string.IsNullOrEmpty(store.ClosedDays))
            {
                var closed = store.ClosedDays
                    .Split(',')
                    .Select(x => int.Parse(x.Trim()))
                    .ToList();

                if (closed.Contains(today))
                    return false;
            }

            var nowTime = now.TimeOfDay;

            if (IsWithinTimeRange(store.OpenTime, store.CloseTime, nowTime))
                return true;

            if (store.OpenTime2.HasValue && store.CloseTime2.HasValue)
            {
                if (IsWithinTimeRange(
                    store.OpenTime2.Value,
                    store.CloseTime2.Value,
                    nowTime))
                    return true;
            }

            return false;
        }

        private bool IsWithinTimeRange(TimeSpan open, TimeSpan close, TimeSpan now)
        {
            if (open <= close)
                return now >= open && now <= close;
            else
                return now >= open || now <= close;
        }
        #region  Haversine 距離公式，回傳公里
        private double CalculateDistance(
            double lat1,
            double lon1,
            double lat2,
            double lon2)
        {
            double R = 6371; // 地球半徑，公里

            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) *
                Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = R * c;

            return d;
        }
        #endregion
    }
}
