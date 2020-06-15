using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Store.Client.Components;
using Store.Client.Services;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Client.Pages
{
    public class UserOrdersBase: ShopComponentBase
    {
        [Inject]
        IAccessTokenProvider AuthenticationService { get; set; }
        [Inject]
        NavigationManager Navigation { get; set; }
        [Inject]
        ProductService service { get; set; }
        [Inject]
        ProductCategoryService Categoryservice { get; set; }
        [Inject]
        IJSRuntime iJSRuntime { get; set; }

        public PageDataModel<ProductModel> PageDataModel { get; set; }
        public IEnumerable<ProductCategoryModel> ProductCategoryModels { get; set; }
        protected int currentPage = 1;
        protected string nameFilter = string.Empty;
        protected string categoryId = "0";
        private string token = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var tokenResult = await AuthenticationService.RequestAccessToken();
            tokenResult.TryGetToken(out var tokenReference);
            token = tokenReference.Value;
            await LoadProducts();
            ProductCategoryModels = await Categoryservice.GetAll(new ProductCategoryModel(), token);
        }
        protected async Task Change(ChangeEventArgs e, SearchItem searchItem)
        {
            if (searchItem == SearchItem.Name)
            {
                nameFilter = e.Value.ToString();
            }
            if (searchItem == SearchItem.Category)
            {
                categoryId = e.Value.ToString();
            }
            currentPage = 1;
            await LoadProducts();
        }

        protected async Task SelectedPage(int page)
        {
            currentPage = page;
            await LoadProducts(page);
        }

        protected async Task Filter()
        {
            currentPage = 1;
            await LoadProducts();
        }

        protected async Task Clear()
        {
            nameFilter = string.Empty;
            currentPage = 1;
            await LoadProducts();
        }

        async Task LoadProducts(int page = 1, int quantityPerPage = 10)
        {
            try
            {
                PageDataModel = await service.GetProductsAsync(page, quantityPerPage, nameFilter, categoryId, token);
            }
            catch (System.Exception ex)
            {
                GlobalMsg.SetMessage(ex.Message, MessageLevel.Error);
            }
        }
    }
}
