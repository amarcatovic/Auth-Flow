using Auth.Flow.IdentityServer.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddIdentityServer()
    .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
    .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
    .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddInMemoryClients(InMemoryConfig.GetClients())
    .AddDeveloperSigningCredential(); // TODO: use AddSigningCredentials method and provide a valid certificate for production.

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

app.UseIdentityServer(); 

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
