using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface ILoginTokenUsageRepository
    {
        Task<bool> IsUsedAsync(string nonce);
        Task MarkUsedAsync(string nonce, DateTime expire);
    }
}
