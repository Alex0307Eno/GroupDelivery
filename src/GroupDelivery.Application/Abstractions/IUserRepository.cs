using GroupDelivery.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetOrCreateByEmail(string email);
        User GetById(int userId);
        void Update(User user);
    }
}
