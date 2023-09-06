using Azure.Identity;
using KeyVaultManagerClient.Data;
using KeyVaultManagerClient.Extensions;
using Microsoft.Extensions.Azure;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<SecretService>();
builder.Services.AddAzureClients(config =>
{
    config.ConfigureDefaults(builder.Configuration.GetSection("AzureDefaults"));
    config.UseCredential(new DefaultAzureCredential());

    config.AddSecretClient(new Uri(builder.Configuration.GetValue<string>("KeyVault:Url")))
                .ConfigureOptions(options => options.Retry.MaxRetries = 10);

}); 
builder.Services.AddLazyCache();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.LoadSecretsFromVault();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
