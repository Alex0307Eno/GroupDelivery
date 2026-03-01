using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface ILoginLogService
    {
        Task RecordAsync(
            string email,
            string provider,
            string ip,
            string userAgent,
            bool success
        );
    }
}
