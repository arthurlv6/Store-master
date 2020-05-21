using AutoMapper;
using Store.Shared;
using Store.Server;
using Store.Server.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Server.Data;

namespace Store.Server.Repos
{
    public class ProductLinkRepo:BaseRepo
    {
        public ProductLinkRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        public ProductLinkModel AddProductLink(ProductLinkModel productLinkModel)
        {
            var productLink = mapper.Map<ProductLink>(productLinkModel);
            productLink.CreatedDate = DateTime.UtcNow;
            var addedEntity = dBContext.ProductLinks.Add(productLink);
            dBContext.SaveChanges();
            return addedEntity.Entity.ToModel<ProductLinkModel>(mapper);
        }
    }
}
