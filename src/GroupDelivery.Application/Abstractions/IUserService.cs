using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IUserService
    {
        Task<UserProfileDto> GetProfileAsync(int userId);

        Task UpdateProfileAsync(int userId, UpdateProfileRequest req);
    }
}
