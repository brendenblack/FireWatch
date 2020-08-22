using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firewatch.Infrastructure.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new List<IdentityResource>
           {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
           };

        //public static IEnumerable<IdentityServer4.Models.ApiScope> ApiScopes =>
        //    new List<ApiScope>
        //    {
        //        new ApiScope("api1", "FIREWatch API")
        //    };


        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("api", "FIREWatch API")
            };
        }

        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "react-app",
                AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },
                RequirePkce = true,
                AllowOfflineAccess = true,
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = { "http://localhost:3000/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:3000/signout-callback-oidc" },
                AllowedCorsOrigins = { "http://localhost:3000" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "api"
                }
            }
        };
    }
}
