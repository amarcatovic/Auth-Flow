using Auth.Flow.IdentityServer.Infrastructure.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Auth.Flow.IdentityServer.Configuration
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public CustomProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var claims = new List<Claim>();

            if (user.IsOwner)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            if (user.IsActive)
            {
                claims.Add(new Claim("active", "true"));
            }
            else
            {
                claims.Add(new Claim("active", "false"));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            context.IsActive = (user != null) && user.IsActive;
        }
    }
}
