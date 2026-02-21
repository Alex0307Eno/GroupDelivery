using GroupDelivery.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GroupDelivery.Application.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeocodingService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Google:ApiKey"];
        }

        public async Task<(double? lat, double? lng)> GetLatLngAsync(string address)
        {
            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("Google API Key 未設定");
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return (null, null);

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var results = doc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0)
                return (null, null);

            var location = results[0]
                .GetProperty("geometry")
                .GetProperty("location");

            var lat = location.GetProperty("lat").GetDouble();
            var lng = location.GetProperty("lng").GetDouble();

            return (lat, lng);
        }
    }
}
