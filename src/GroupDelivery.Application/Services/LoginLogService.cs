using System;
using System.Threading.Tasks;
using GroupDelivery.Application.Abstractions;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Services
{
    public class LoginLogService : ILoginLogService
    {
        private readonly ILoginLogRepository _repository;

        public LoginLogService(ILoginLogRepository repository)
        {
            _repository = repository;
        }

        public async Task RecordAsync(string account, string ip, string userAgent,  string loginType,bool success)
        {
            var log = new LoginLog
            {
                Account = account,
                IpAddress = ip,
                UserAgent = userAgent,
                Success = success,
                LoginType = loginType,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(log);
        }
    }
}