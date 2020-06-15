using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Client.Services
{
    public class UserOrdersService : BaseService
    {
        private readonly HttpClient _httpClient;

        public UserOrdersService(HttpClient httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PageDataModel<ProductModel>> GetProductsAsync(int page, int size, string keyword, string categoryId, string token)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization") && !string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string url = $"api/Product?page={page}&size={size}&keyword={keyword}&categoryId={categoryId}";
            var httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStreamAsync();
                var deserializedData = await JsonSerializer.DeserializeAsync<List<ProductModel>>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return new PageDataModel<ProductModel>
                {
                    Data = deserializedData,
                    PageQuantity = int.Parse(httpResponseMessage.Headers.GetValues("pagesQuantity").FirstOrDefault() ?? "0")
                };
            }
            else
            {
                throw new Exception(httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
        }
    }
}
