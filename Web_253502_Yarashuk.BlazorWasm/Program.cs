using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using Web_253502_Yarashuk.BlazorWasm;
using Web_253502_Yarashuk.BlazorWasm.Services;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IDataService, DataService>();


builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    //builder.Configuration.Bind("Local", options.ProviderOptions);
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

await builder.Build().RunAsync(); 

