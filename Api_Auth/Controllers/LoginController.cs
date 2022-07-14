using System.ComponentModel.DataAnnotations;

using Furion;
using Furion.DataEncryption;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api_Auth.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>默认用户名/密码：admin/admin</remarks>
        /// <returns></returns>
        [HttpPost("login")]
        public string LoginAsync([Required] LoginInput input)
        {
            // 获取加密后的密码
            var encryptPasswod = MD5Encryption.Encrypt(input.Password);

            // 判断用户名和密码是否正确 忽略全局过滤器
            var user = GetUser(input);

            // 获取数据权限
            var dataScopes = JsonUtil.ToJson(input.Socpes);
            var scopes = JsonUtil.ToJson(input.Socpes);
            

            // 生成Token令牌
            //var accessToken = await _jwtBearerManager.CreateTokenAdmin(user);
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                {ClaimConst.CLAINM_USERID, user.Id},
                {ClaimConst.TENANT_ID, user.TenantId},
                {ClaimConst.CLAINM_ACCOUNT, user.Account},
                {ClaimConst.CLAINM_NAME, user.Name},
                {ClaimConst.CLAINM_SUPERADMIN, user.AdminType},
                {ClaimConst.DATA_SCOPES, dataScopes},
                { ClaimConst.SCOPES ,scopes}
            });

            // 设置Swagger自动登录
            HttpContext.SigninToSwagger(accessToken);

            // 生成刷新Token令牌
            var refreshToken =
                JWTEncryption.GenerateRefreshToken(accessToken, App.GetOptions<RefreshTokenSettingOptions>().ExpiredTime);

            // 设置刷新Token令牌
            HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return accessToken;
        }

        public SysUser GetUser(LoginInput input) 
        {
            List<SysUser> users = new List<SysUser>();
            users.Add(new SysUser()
            {
                Id = 1,
                TenantId = 123456,
                Account = "zhangsan",
                Name = "张三",
                AdminType = AdminType.Admin               
            }) ;

            users.Add(new SysUser()
            {
                Id = 1,
                TenantId = 123456,
                Account = "lisi",
                Name = "李四",
                AdminType = AdminType.None
            });

            return users?.Find(a => a.Account == input?.Account);
        }
    }
}