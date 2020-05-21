using Store.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Client.Services
{
    
    public class ProductLinkService : BaseService
    {
        private readonly HttpClient _httpClient;
        public ProductLinkService(HttpClient httpClient):base(httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> PostProductLinkAsync(UploadProductLinkModel model)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                
                content.Add(new StreamContent(model.Image,(int)model.Image.Length),"File",model.ImageName);
                content.Add(new StringContent(model.ProductId.ToString()), "ProductId");
                var response = await _httpClient.PostAsync("api/ProductLink", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
