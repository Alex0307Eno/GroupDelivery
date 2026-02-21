using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Application.Abstractions
{
    public interface IGeocodingService
    {
        Task<(double? lat, double? lng)> GetLatLngAsync(string address);
    }
}
