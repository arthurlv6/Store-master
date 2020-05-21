using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Store.Client.Components;
using Store.Client.Services;
using Store.Shared;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Store.Client.Pages
{
    [Authorize]
    public class ProductsBase: ShopComponentBase
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
            tokenResult.TryGetToken(out var tokenReference, redirect: true);
            token = tokenReference.Value;
            await LoadProducts();
            ProductCategoryModels = await Categoryservice.GetAll(new ProductCategoryModel(),token);
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
                PageDataModel = await service.GetProductsAsync(page,quantityPerPage, nameFilter,categoryId, token);
            }
            catch (System.Exception ex)
            {
                GlobalMsg.SetMessage(ex.Message, MessageLevel.Error);
            }
        }

        async Task DeletePerson(int personId)
        {
            //await js.DisplayMessage("my title", "My message", SweetAlertMessageType.success);
            //var personSelected = people.First(x => x.Id == personId);
            if (await iJSRuntime.Confirm("Confirm", $"Do you want to delete?", SweetAlertMessageType.question))
            {
//                await http.DeleteAsync($"api/people/{personId}");
  //              await LoadPeople();
            }
        }
        protected void ExportAsCSV()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                {
                    using (var csv = new CsvHelper.CsvWriter(writer))
                    {
                        csv.WriteRecords(PageDataModel.Data);
                    }

                    var arr = memoryStream.ToArray();
                    iJSRuntime.SaveAs("people.csv", arr);
                }
            }
        }
    }
}
