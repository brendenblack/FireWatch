using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "fw-react",
                    ClientName = "FIREwatch React client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:3000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:3000/signout-callback-oidc" },
                    AllowedCorsOrigins =     { "http://localhost:3000" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                new Client
                {
                    ClientId = "fw-react2",
                    Enabled = true,
                    ClientName = "FIREwatch React client #2",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowPlainTextPkce = false,
                    RequireClientSecret = false,
                    AllowAccessTokensViaBrowser = true,
                    AllowRememberConsent = true,
                    RedirectUris =           { "http://localhost:3000/signin-oidc", "http://localhost:3000/silentj" },
                    PostLogoutRedirectUris = { "http://localhost:3000/" },
                    AllowedCorsOrigins =     { "http://localhost:3000" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                },
                //new Client
                //{
                //    AllowOfflineAccess = true,
                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //    IdentityServerConstants.StandardScopes.Profile,
                //    "api1"
                //    },
                //    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                //    ClientId = "postman",
                //    ClientSecrets = { new Secret("opensecret") },
                //    ClientName = "Postman test client",
                //    AllowedGrantTypes = GrantTypes.Code,
                //    AllowAccessTokensViaBrowser = true
                //}
            };
    }
}
