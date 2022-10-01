using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Auth.Flow.IdentityServer.Configuration
{
    public static class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
          new List<IdentityResource>
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResource("roles", "User role(s)", new List<string> { "role" })
          };

        public static List<TestUser> GetUsers() =>
          new List<TestUser>
          {
              new TestUser
              {
                  SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                  Username = "Amar",
                  Password = "amaramar",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Amar"),
                      new Claim("family_name", "Ćatović"),
                      new Claim("address", "Kralja Tvrtka 12"),
                      new Claim("role", "Admin")
                  }
              },
              new TestUser
              {
                  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                  Username = "Tito",
                  Password = "amaramar",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Josip"),
                      new Claim("family_name", "Broz"),
                      new Claim("address", "Long Avenue 289"),
                      new Claim("role", "User")
                  }
              }
          };

        public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
           new Client
           {
                ClientId = "auth-flow",
                ClientSecrets = new [] { new Secret("secret".Sha512()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "testAPI" }
            },
            new Client
            {
                ClientName = "MVC Client",
                ClientId = "mvc-client",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                RequirePkce = false,
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId, 
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    "roles",
                    "testAPI" 
                },
                ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" }
            }
        };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> { new ApiScope("testAPI", "Testing API") };

        public static IEnumerable<ApiResource> GetApiResources() =>
        new List<ApiResource>
        {
            new ApiResource("testAPI", "Testing API")
            {
                Scopes = { "testAPI" }
            }
        };
    }
}
