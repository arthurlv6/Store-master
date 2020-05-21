using AutoMapper;
using Store.Api.Data;
using Store.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Api.Repos
{
    public class WechatRepos : BaseRepo
    {
        public WechatRepos(ApplicationDbContext _dBContext, IMapper _mapper) : base(_dBContext, _mapper)
        {
        }
        private string AppId = "wxc0d33b6b5e385696";
        private string AppSecret = "94275c3c5d5f72140f2046fa0db1d8a6";
        private string Authorization_code = "authorization_code";

        public async Task<WechatBase> GetWechatUserId(string code)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://api.weixin.qq.com/sns/");
                var response = await client.GetAsync(@$"oauth2/access_token?appid={AppId}&secret={AppSecret}&code={code}&grant_type={Authorization_code}");
                return await HandleResult(response);
            }
        }
        public async Task<WechatBase> GetWechatIdByAccessTokenAndOpenId(string accessToken,string openId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(@"https://api.weixin.qq.com/sns/");
                var response = await client.GetAsync(@$"userinfo?access_token={accessToken}&openid={openId}");
                return await HandleResult(response,WechatCall.UserProfile);
            }
        }
        private async Task<WechatBase> HandleResult(HttpResponseMessage response, WechatCall wechatCall=WechatCall.Token)
        {
            if (response.IsSuccessStatusCode)
            {
                var returnString = await response.Content.ReadAsStringAsync();
                if (returnString.Contains("errcode"))
                {
                    return JsonSerializer.Deserialize<WechatError>(returnString);
                }
                else
                {
                    if (wechatCall == WechatCall.Token)
                    {
                        var expected = JsonSerializer.Deserialize<WechatAccessToken>(returnString);
                        expected.IsSuccess = true;
                        return expected;
                    }
                    else
                    {
                        var expected = JsonSerializer.Deserialize<WechatUserInfo>(returnString);
                        expected.IsSuccess = true;
                        return expected;
                    }
                }
            }
            else
            {
                return new WechatError { errcode = int.Parse(response.StatusCode.ToString()), errmsg = "Wechate SuccessStatusCode return failed." };
            }
        }
    }
    enum WechatCall
    {
        Token,
        UserProfile
    }
    public class WechatBase
    {
        public bool IsSuccess { get; set; } = false;
    }
    public class WechatUserInfo: WechatBase
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public IList<string> privilege { get; set; }
        public string unionid { get; set; }
    }

    public class WechatAccessToken:WechatBase
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }

        public string unionid { get; set; }
    }
    public class WechatError: WechatBase
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
}
