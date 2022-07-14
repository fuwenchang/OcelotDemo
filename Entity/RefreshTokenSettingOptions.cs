using Furion.ConfigurableOptions;

namespace Api_Auth
{
    public class RefreshTokenSettingOptions : IConfigurableOptions
    {
        /// <summary>
        /// 令牌过期时间（分钟）
        /// </summary>
        public int ExpiredTime { get; set; } = 43200;
    }
}