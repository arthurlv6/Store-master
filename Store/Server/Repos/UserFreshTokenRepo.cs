using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Shared;
using Store.Server;
using Store.Server.Repos;
using System.Collections.Generic;
using System.Linq;
using Store.Server.Data;

namespace Store.Server.Repos
{
    public class UserFreshTokenRepo : BaseRepo
    {
        public UserFreshTokenRepo(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        public void Add(string email,string refreshToken)
        {
            dBContext.UserRefreshTokens.Add(new UserRefreshToken { Name = email, RefreshToken = refreshToken });
            dBContext.SaveChanges();
        }
        public void Update(string email, string refreshToken)
        {
            var userRefreshToken = dBContext.UserRefreshTokens.First(d => d.Name == email);
            userRefreshToken.RefreshToken = refreshToken;
            dBContext.SaveChanges();
        }
        public string Get(string email)
        {
            var userRefreshToken = dBContext.UserRefreshTokens.First(d => d.Name == email);
            return userRefreshToken.RefreshToken;
        }
    }
}
