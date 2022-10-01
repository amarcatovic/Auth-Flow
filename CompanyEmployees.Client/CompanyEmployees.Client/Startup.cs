using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CompanyEmployees.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CompanyEmployees.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICompanyHttpClient, CompanyHttpClient>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.SignInScheme = "Cookies";
                opt.Authority = "https://localhost:7240/";
                opt.ClientId = "mvc-client";
                opt.ResponseType = "code id_token";
                opt.SaveTokens = true;
                opt.ClientSecret = "MVCSecret";
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.ClaimActions.DeleteClaim("sid");
                opt.ClaimActions.DeleteClaim("idp");
                opt.ClaimActions.MapUniqueJsonKey("address", "address");
                opt.ClaimActions.MapUniqueJsonKey("role", "role");
                opt.Scope.Add("roles");
                opt.Scope.Add("address");
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role
                };
                opt.Scope.Add("testAPI");
                opt.Scope.Add("openid");
                opt.Scope.Add("profile");
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
