using System.Threading.Tasks;
using GroupDelivery.Domain;

namespace GroupDelivery.Application.Abstractions
{
    public interface ILoginLogRepository
    {
        Task AddAsync(LoginLog log);
    }
}