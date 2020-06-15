using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Server.Data;
using Store.Server.Models;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Store.Server.Repos
{
    public class UserOrderRepo : BaseRepo
    {
        public UserOrderRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        public async Task<Tuple<IEnumerable<M>, double>> GetPageData<T, M>(int page = 1, int size = 20, string keyword = "") where T : BaseEntity where M : BaseModel
        {
            var queryable = dBContext.Set<UserOrder>().AsQueryable();
            Expression<Func<UserOrder, bool>> nameExpected = d => true;
            if (!string.IsNullOrEmpty(keyword))
            {
                nameExpected = user => user.Name.Contains(keyword);
            }
            
            double count = await queryable.Where(nameExpected).CountAsync();
            double pagesQuantity = Math.Ceiling(count / size);
            var pagination = new PaginationModel() { Page = page, QuantityPerPage = size };
            return new Tuple<IEnumerable<M>, double>(await queryable.Where(nameExpected).Paginate(pagination).Select(d => d.ToModel<M>(mapper)).ToListAsync(), pagesQuantity);
        }
        public async Task<UserOrderModel> GetOneAsync(int id)
        {
            var requirement = await dBContext.UserOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
            if (requirement != null)
                return requirement.ToModel<UserOrderModel>(mapper);
            else
                return null;
        }
        public async Task UpdateRequirementAsync(UserOrderModel  userOrderModel)
        {
            try
            {
                var entity = dBContext.UserOrders.First(d => d.Id == userOrderModel.Id);
                mapper.Map(userOrderModel, entity);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
