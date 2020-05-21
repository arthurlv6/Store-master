using Microsoft.AspNetCore.Components;
using System.IO;
using System.Threading.Tasks;
using Store.Shared;
using Store.Client.Services;
using BlazorInputFile;
using System.Text;
using System;

namespace Store.Client.Components
{
    public class ProductLineBase : ShopComponentBase
    {
        [Parameter]
        public ProductModel Item { get; set; }
        [Inject]
        protected ProductLinkService ProductLinkService { get; set; }
        protected override void OnInitialized()
        {
            foreach (var item in Item.ProductLinks)
            {
                if(!item.Address.Contains("http"))
                item.Address = "https://neartonztesting.oss-ap-southeast-2.aliyuncs.com/" + item.Address;
            }
        }
       protected void ShowDetail(ProductModel productModel)
        {
            productModel.IsShowDetail = !productModel.IsShowDetail;
        }
        protected IFileListEntry[] selectedFiles;

        protected void HandleSelection(IFileListEntry[] files)
        {
            selectedFiles = files;
        }

        protected async Task LoadFile(IFileListEntry file)
        {
            file.OnDataRead += (sender, eventArgs) => InvokeAsync(StateHasChanged);


            var model = new UploadProductLinkModel() 
            { 
                ProductId=Item.Id,
                Image=file.Data,
                ImageName=file.Name
            };

            string imageUrl = await ProductLinkService.PostProductLinkAsync(model);
            if (imageUrl != null)
            {
                Item.ProductLinks.Insert(0,new ProductLinkModel() { ProductId = model.ProductId, Address = imageUrl });
                GlobalMsg.SetMessage("File " + file.Name + " uploaded.", MessageLevel.Error);
            }
            else
            {
                GlobalMsg.SetMessage("Failed to connect API.", MessageLevel.Error);
            }
        }
        protected void Remove(int id)
        {

        }
    }
}
