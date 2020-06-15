using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Shared;
using Store.Server;
using Store.Server.Repos;
using System.Collections.Generic;
using System.Linq;
using Store.Server.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;

namespace Store.Server.Repos
{
    public class ProductRepo:BaseRepo
    {
        public ProductRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        public async Task<Tuple<IEnumerable<M>,double>> GetPageData<T, M>(int page = 1, int size = 20, string keyword = "",string categoryId="0") where T : BaseEntity where M : BaseModel
        {
            var queryable = dBContext.Set<Product>().AsQueryable().Include(d => d.ProductLinks).Include(d=>d.ProductCategory);
            Expression<Func<Product, bool>> nameExpected = d => true ;
            if (!string.IsNullOrEmpty(keyword))
            {
                nameExpected = product => product.Name.Contains(keyword);
            }
            Expression<Func<Product, bool>> categoryExpected = d => true ;
            if (int.TryParse(categoryId, out int categoryid))
            {
                if (categoryid > 0)
                    categoryExpected = product => product.ProductCategoryId==categoryid;
            }
            double count = await queryable.Where(nameExpected).Where(categoryExpected).CountAsync();
            double pagesQuantity = Math.Ceiling(count / size);
            var pagination = new PaginationModel() { Page = page, QuantityPerPage = size };
            return new Tuple<IEnumerable<M>, double>(await queryable.Where(nameExpected).Where(categoryExpected).Paginate(pagination).Select(d => d.ToModel<M>(mapper)).ToListAsync(),pagesQuantity);
        }
        public async Task<ProductModel> GetOneAsync(int id)
        {
            var requirement = await dBContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
            if (requirement != null)
                return requirement.ToModel<ProductModel>(mapper);
            else
                return null;
        }
        public async Task UpdateRequirementAsync(ProductModel productModel)
        {
            try
            {
                Product entity = dBContext.Products.First(d => d.Id == productModel.Id);
                mapper.Map(productModel, entity);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
