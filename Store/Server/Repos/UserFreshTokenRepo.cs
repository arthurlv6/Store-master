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
        public void Add(UserInfo userInfo, string refreshToken)
        {
            dBContext.UserRefreshTokens.Add(new UserRefreshToken
            {
                Name = userInfo.Email,
                RefreshToken = refreshToken,
                Nickname = userInfo.Nickname,
                Sex = userInfo.Sex,
                City = userInfo.City,
                Country = userInfo.Country,
                Headimgurl = userInfo.Headimgurl,
            });
            dBContext.SaveChanges();
        }
        public void Update(UserInfo userInfo, string refreshToken)
        {
            var userRefreshToken = dBContext.UserRefreshTokens.First(d => d.Name == userInfo.Email);
            userRefreshToken.RefreshToken = refreshToken;
            userRefreshToken.Nickname = userInfo.Nickname;
            userRefreshToken.Sex = userInfo.Sex;
            userRefreshToken.City = userInfo.City;
            userRefreshToken.Country = userInfo.Country;
            userRefreshToken.Headimgurl = userInfo.Headimgurl;
            dBContext.SaveChanges();
        }
        public UserRefreshTokenModel Get(string email)
        {
            return dBContext.UserRefreshTokens.First(d => d.Name == email).ToModel<UserRefreshTokenModel>(mapper);
        }
        public bool IsExist(string email)
        {
            var token = dBContext.UserRefreshTokens.FirstOrDefault(d => d.Name == email);
            if (token == null)
                return false;
            else
                return true;
        }
    }
}
