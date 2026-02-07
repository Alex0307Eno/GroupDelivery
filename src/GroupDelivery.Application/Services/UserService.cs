using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;
using System;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserProfileDto> GetProfileAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
                return null;

            return new UserProfileDto
            {
                DisplayName = user.DisplayName,
                PictureUrl = user.PictureUrl,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                City = user.City,
                Bio = user.Bio,
                FoodPreference = user.FoodPreference,
                NotifyOptIn = user.NotifyOptIn
            };
        }


        public async Task UpdateProfileAsync(int userId, UpdateProfileRequest req)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.Phone = req.Phone;
            user.Nickname = req.Nickname;
            user.Bio = req.Bio;
            user.City = req.City;
            user.FoodPreference = req.FoodPreference;
            user.NotifyOptIn = req.NotifyOptIn;

            await _userRepo.UpdateAsync(user);
        }
    }
}
