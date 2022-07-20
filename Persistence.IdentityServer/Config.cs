using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Persistence.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        // Defining an API Resource
        public static IEnumerable<ApiResource> Apis =>
                new List<ApiResource>{
            new ApiResource("Order.WebApi", "订单服务"),
            new ApiResource("Product.WebApi", "产品服务")
                };

        // Defining Client
        public static IEnumerable<Client> Clients =>
            new List<Client> {
            new Client
            {
                AllowedGrantTypes=GrantTypes.Hybrid,
                ClientId="client",
                ClientSecrets={new Secret("secret".Sha256())},                
                // where to redirect to after login
                RedirectUris = { "http://localhost:5002/signin-oidc" },
                // where to redirect to after logout
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                //客户端可以访问的范围
                AllowedScopes={
                    "Order.WebApi",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                AllowOfflineAccess = true
            }
            };



        //public static List<TestUser> Users => new List<TestUser>
        //    {
        //        new TestUser
        //        {
        //            SubjectId = "1",
        //            Username = "fan",
        //            Password = "123456",

        //            Claims = new []
        //            {
        //                new Claim("name", "fan"),
        //                new Claim("website", "https://fan.com")
        //            }
        //        }
        //    };
    }

}
