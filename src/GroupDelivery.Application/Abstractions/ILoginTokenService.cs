using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface ILoginTokenService
    {
        string GenerateToken(string email);
        bool TryValidateToken(string token, out string email);
    }

}
