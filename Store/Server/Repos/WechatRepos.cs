using AutoMapper;
using Store.Server.Data;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Server.Repos
{
    public class WechatRepos : BaseRepo
    {
        public WechatRepos(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        private string AppId = "wxc0d33b6b5e385696";
        private string AppSecret = "94275c3c5d5f72140f2046fa0db1d8a6";
        private string Authorization_code = "authorization_code";

        public async Task<WechatAccessToken> GetWechatUserId(string code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://api.weixin.qq.com/sns/");
                var response = await client.GetStreamAsync(@$"oauth2/access_token?appid={AppId}&secret=SECRET&code={code}&grant_type={Authorization_code}");
                return await JsonSerializer.DeserializeAsync<WechatAccessToken>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            throw new Exception("Failed");
        }
    }
    public class WechatAccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }

        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
        public string unionid { get; set; }
    }
}
