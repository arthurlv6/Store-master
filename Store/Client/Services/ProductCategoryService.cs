using Store.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Client.Services
{
    
    public class ProductCategoryService : BaseService
    {
        private readonly HttpClient _httpClient;
        public ProductCategoryService(HttpClient httpClient):base(httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
