using MangoCarrierSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace MangoCarrierSystem.REST
{
    public class LFLRestService
    {
        readonly string baseUri = "http://localhost:55834/";

        public async Task<ShippingLabelResponse> GetShippingLabel(int orderID)
        {
            var result = new ShippingLabelResponse();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/ShippingLabels/" + orderID);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    result = await response.Content.ReadAsAsync<ShippingLabelResponse>();
                }
                catch (HttpRequestException)
                {
                    // Handle exception.
                }
            }

            return result;
        }
    }
}