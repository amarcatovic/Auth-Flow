using System.Diagnostics;
using System.Security.Claims;
using Auth.Flow.IdentityServer.Infrastructure;
using Auth.Flow.IdentityServer.Infrastructure.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Flow.IdentityServer.Configuration
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();

                        if (!context.Clients.Any())
                        {
                            foreach (var client in InMemoryConfig.GetClients())
                            {
                                context.Clients.Add(client.ToEntity());
                            }
                            context.SaveChanges();
                        }

                        if (!context.IdentityResources.Any())
                        {
                            foreach (var resource in InMemoryConfig.GetIdentityResources())
                            {
                                context.IdentityResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }

                        if (!context.ApiScopes.Any())
                        {
                            foreach (var apiScope in InMemoryConfig.GetApiScopes())
                            {
                                context.ApiScopes.Add(apiScope.ToEntity());
                            }

                            context.SaveChanges();
                        }

                        if (!context.ApiResources.Any())
                        {
                            foreach (var resource in InMemoryConfig.GetApiResources())
                            {
                                context.ApiResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw;
                    }
                }

                using (var context = scope.ServiceProvider.GetRequiredService<IdentityServerDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                        if (userManager.FindByEmailAsync("amar@test.com").Result is null)
                        {
                            var newUser = new User
                            {
                                FirstName = "Amar",
                                LastName = "Ćatović",
                                Email = "amar@test.com",
                                EmailAddress = "amar@test.com",
                                UserName = "amar@test.com",
                                PhoneNumber = "123456789",
                                MobileNumber = "123456789",
                                EmailConfirmed = true,
                                IsActive = true
                            };

                            var result = userManager.CreateAsync(newUser, "Pass123$").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            result = userManager.AddClaimsAsync(newUser, new Claim[]{
                                new Claim(JwtClaimTypes.Role, "Admin"),
                                new Claim("some-random-claim-key", "some-random-claim-value"),
                            }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                        if (userManager.FindByEmailAsync("user@test.com").Result is null)
                        {
                            var newUser = new User
                            {
                                FirstName = "User",
                                LastName = "Test",
                                Email = "user@test.com",
                                EmailAddress = "user@test.com",
                                UserName = "user@test.com",
                                PhoneNumber = "123456789",
                                MobileNumber = "123456789",
                                EmailConfirmed = true,
                                IsActive = true
                            };

                            var result = userManager.CreateAsync(newUser, "Pass123$").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                            result = userManager.AddClaimsAsync(newUser, new Claim[]{
                                new Claim("some-random-claim-key", "some-random-claim-value"),
                            }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
