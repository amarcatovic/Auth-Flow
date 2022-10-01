using Auth.Flow.IdentityServer.Configuration;
using Auth.Flow.IdentityServer.Infrastructure;
using Auth.Flow.IdentityServer.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddCors();
builder.Services.AddDbContext<IdentityServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), options => options.MigrationsAssembly("Auth.Flow.IdentityServer")));

services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<IdentityServerDbContext>()
        .AddDefaultTokenProviders();

services.AddIdentityServer(opt =>
    {
        opt.Authentication.CookieLifetime = TimeSpan.FromMinutes(4);
    })
    //.AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
    //.AddInMemoryApiResources(InMemoryConfig.GetApiResources())
    //.AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    //.AddInMemoryClients(InMemoryConfig.GetClients())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddProfileService<CustomProfileService>()
    .AddConfigurationStore(opt =>
     {
         opt.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), options =>
            options.MigrationsAssembly("Auth.Flow.IdentityServer"));
     })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), options =>
            options.MigrationsAssembly("Auth.Flow.IdentityServer"));
    })
    .AddAspNetIdentity<User>()
    .AddDeveloperSigningCredential(); // TODO: use AddSigningCredential method and provide a valid certificate for production.

services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseStaticFiles();
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseIdentityServer(); 

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MigrateDatabase().Run();
