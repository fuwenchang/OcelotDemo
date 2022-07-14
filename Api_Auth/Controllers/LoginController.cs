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
        /// �û���¼
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>Ĭ���û���/���룺admin/admin</remarks>
        /// <returns></returns>
        [HttpPost("login")]
        public string LoginAsync([Required] LoginInput input)
        {
            // ��ȡ���ܺ������
            var encryptPasswod = MD5Encryption.Encrypt(input.Password);

            // �ж��û����������Ƿ���ȷ ����ȫ�ֹ�����
            var user = GetUser(input);

            // ��ȡ����Ȩ��
            var dataScopes = JsonUtil.ToJson(input.Socpes);
            var scopes = JsonUtil.ToJson(input.Socpes);
            

            // ����Token����
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

            // ����Swagger�Զ���¼
            HttpContext.SigninToSwagger(accessToken);

            // ����ˢ��Token����
            var refreshToken =
                JWTEncryption.GenerateRefreshToken(accessToken, App.GetOptions<RefreshTokenSettingOptions>().ExpiredTime);

            // ����ˢ��Token����
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
                Name = "����",
                AdminType = AdminType.Admin               
            }) ;

            users.Add(new SysUser()
            {
                Id = 1,
                TenantId = 123456,
                Account = "lisi",
                Name = "����",
                AdminType = AdminType.None
            });

            return users?.Find(a => a.Account == input?.Account);
        }
    }
}