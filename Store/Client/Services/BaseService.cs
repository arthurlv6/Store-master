using Microsoft.AspNetCore.Components;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Client.Services
{
    
    public class BaseService
    {
        protected readonly HttpClient httpClient;
        public BaseService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
            
        }
        public async Task<IEnumerable<M>> GetAll<M>(M m,string token=null) where M:BaseModel
        {
            try
            {
                if (!httpClient.DefaultRequestHeaders.Contains("Authorization") && !string.IsNullOrEmpty(token))
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                string url = @"api/"+m.GetType().Name.Remove(m.GetType().Name.IndexOf("Model"), "Model".Length);
                var data = await httpClient.GetStreamAsync(url);
                return await JsonSerializer.DeserializeAsync<IEnumerable<M>>
                    (data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
