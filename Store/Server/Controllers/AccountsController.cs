using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Server.Models;
using Store.Server.Repos;
using Store.Shared;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Store.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly UserFreshTokenRepo _userFreshTokenRepo;
        private readonly WechatRepos _wechatRepos;
        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            UserFreshTokenRepo userFreshTokenRepo,
            WechatRepos wechatRepos)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userFreshTokenRepo = userFreshTokenRepo;
            _wechatRepos = wechatRepos;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                var userToken = BuildToken(model);
                _userFreshTokenRepo.Add(new UserInfo() { Email = model.Email }, userToken.RefreshToken);
                return userToken;
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("GetTokenByCode")]
        public async Task<ActionResult<UserToken>> GetTokenByCode([FromBody] String code)
        {
            try
            {
                // step one call wechat link to get access token
                var token = await _wechatRepos.GetWechatUserId(code);
                if (!token.IsSuccess) return BadRequest(((WechatError)token).errcode + "---" + ((WechatError)token).errmsg);
                var wechat = (WechatAccessToken)token;

                var unionid = wechat.unionid;
                // step two get user id by using access token
                var result = await _wechatRepos.GetWechatIdByAccessTokenAndOpenId(wechat.access_token, wechat.openid);
                if (!result.IsSuccess) return BadRequest(((WechatError)result).errcode + "---" + ((WechatError)result).errmsg);
                var wechatUser = ((WechatUserInfo)result);
                unionid = wechatUser.unionid;

                if (string.IsNullOrEmpty(unionid))
                    return BadRequest("unionid value is empty");

                // step three get return token and refresh token
                var userName = unionid + "@wechat.com";
                var userInfo = new UserInfo
                {
                    Email = userName,
                    Nickname = wechatUser.nickname,
                    Sex = wechatUser.sex.ToString(),
                    City = wechatUser.city,
                    Country = wechatUser.country,
                    Headimgurl = wechatUser.headimgurl
                };
                var newJwtToken = BuildToken(userInfo);
                if (_userFreshTokenRepo.IsExist(userInfo.Email))
                {
                    _userFreshTokenRepo.Update(userInfo, newJwtToken.RefreshToken);
                    return Ok(newJwtToken);
                }
                else
                {
                    var user = new ApplicationUser { UserName = userName, Email = userName, };
                    var createResult = await _userManager.CreateAsync(user, "Tomhack!1");
                    if (createResult.Succeeded)
                    {
                        _userFreshTokenRepo.Add(userInfo, newJwtToken.RefreshToken);
                        return Ok(newJwtToken);
                    }
                    else
                    {
                        return BadRequest("api Username or password invalid");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetNewFreshToken")]
        public async Task<ActionResult<UserToken>> GetNewToken([FromBody] UserGetRefreshTokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                ModelState.AddModelError(string.Empty, "Can't be null");
                return BadRequest(ModelState);
            }
            var principal = GetPrincipalFromExpiredToken(tokenModel.Token);
            var username = principal.Identity.Name;
            var UserFreshModel = _userFreshTokenRepo.Get(username);

            if (UserFreshModel.RefreshToken != tokenModel.RefreshToken)
                return BadRequest("Invalid refresh token");
            var userInfo = new UserInfo
            {
                Email = username,
                Nickname = UserFreshModel.Nickname,
                Sex = UserFreshModel.Sex,
                City = UserFreshModel.City,
                Country = UserFreshModel.Country,
                Headimgurl = UserFreshModel.Headimgurl
            };
            var newJwtToken = BuildToken(userInfo);

            _userFreshTokenRepo.Update(userInfo, newJwtToken.RefreshToken);
            return newJwtToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LKM3LKM344NKSJDFN4KJ345N43KJN4KJFNKDJFSNDKFJN4KJKJN4")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim("Nickname", userInfo.Nickname),
                new Claim("Sex", userInfo.Sex),
                new Claim("City", userInfo.City),
                new Claim("Country", userInfo.Country),
                new Claim("Headimgurl", userInfo.Headimgurl),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("LKM3LKM344NKSJDFN4KJ345N43KJN4KJFNKDJFSNDKFJN4KJKJN4"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Expiration time
            var expiration = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
               );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken(),
                Expiration = expiration
            };
        }
        private string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);

        }
    }
}
