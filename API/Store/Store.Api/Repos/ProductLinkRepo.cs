using AutoMapper;
using Store.Shared;
using Store.Api.Data;

namespace Store.Api.Repos
{
    public class ProductLinkRepo:BaseRepo
    {
        public ProductLinkRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        public ProductLinkModel AddProductLink(ProductLinkModel productLinkModel)
        {
            var productLink = mapper.Map<ProductLink>(productLinkModel);
            productLink.Product = dBContext.Products.Find(productLinkModel.ProductId);

            var addedEntity = dBContext.ProductLinks.Add(productLink);
            dBContext.SaveChanges();
            return addedEntity.Entity.ToModel<ProductLinkModel>(mapper);
        }
    }
}
