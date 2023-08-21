using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TE.BE.City.Domain.Interfaces;

namespace TE.BE.City.Domain
{
    public class GoogleMapsWebWebProvider : IGoogleMapsWebProvider
    {
        private string apiKey;
        private IConfiguration _config;
        private string baseAddress;
        private string uri = "/maps/api/geocode/json?latlng={0},{1}&key={2}";

        public GoogleMapsWebWebProvider(IConfiguration config)
        {
            _config = config;
            baseAddress = _config["ExternalLink:geoLocationUrl"];
            apiKey = _config["GoogleMapsKey"];
        }

        public async Task<string> GetAddress(string latitude, string longitude)
        {
            string address = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);

                var path = String.Format(uri, latitude, longitude, apiKey);
                
                var response = await client.GetAsync(path);
                
                if (response.IsSuccessStatusCode)
                {
                    var text = await response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<AddressEntity>(text);

                    var result = root.results.FirstOrDefault(c => c.formatted_address.Contains("Rua") 
                                                         || c.formatted_address.Contains("Av.")
                                                         || c.formatted_address.Contains("Avenida")
                                                         || c.formatted_address.Contains("Estrada")
                                                         || c.formatted_address.Contains("Travessa")
                                                         || c.formatted_address.Contains("Via")
                                                         || c.formatted_address.Contains("Viela"));
                    if (result != null)
                    {
                        var array = result.formatted_address.Split(',').Take(2);
                        address = String.Concat(array);
                    } ;
                }
                return address;
            }
        }
    }

    internal class AddressEntity
    {
        [JsonProperty("results")]
        public List<AddressComponentEntity> results { get; set; }
    }

    internal class AddressComponentEntity
    {
        [JsonProperty("formatted_address")]
        public string  formatted_address { get; set; }
    }
}
