using Microsoft.EntityFrameworkCore;
using NBP.Clients;
using NBP.EF;
using NBP.EF.Repositories;
using NBP.HostedService;
using NBP.Kernel;
using NBP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CurrencyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ExchangeService>();
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddSingleton<IClock, Clock>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpClient<ICurrencyExchangeClient, NBPCurrencyExchangeClient>(client =>
{
    client.BaseAddress = builder.Configuration.GetValue<Uri>("NBP:BaseAddress");
});

builder.Services.AddHostedService<AppInitializer>();
builder.Services.AddHostedService<CurrencyExchangeHostedService>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();