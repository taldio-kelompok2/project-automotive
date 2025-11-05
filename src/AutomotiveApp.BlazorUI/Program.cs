using AutomotiveApp.BlazorUI.Components;
using AutomotiveApp.BlazorUI.Models.Auth.Context;
using AutomotiveApp.BlazorUI.Services.Implementation;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.BlazorUI.Services.Invoices;
using AutomotiveApp.Shared.Config;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http.Features;
using MudBlazor;
using MudBlazor.Services;
using System.Globalization;
using System.Net;
using System.Text;
using AutomotiveApp.BlazorUI.Services.CourseSessions;
using AutomotiveApp.BlazorUI.Services.PaymentMethods;
using AutomotiveApp.Application.PaymentMethods;
using AutomotiveApp.BlazorUI.Services.CourseCategories;


var builder = WebApplication.CreateBuilder(args);


// Auth
builder.Services.AddAuthorizationCore();

// Add services to the container.
builder.Services.AddRazorComponents()
.AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CircuitHandler, BlazorScopeCircuitHandler>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IRentalCartService, RentalCartService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddTransient<AuthMessageHandler>();

builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = new CookieContainer()
}).AddHttpMessageHandler<AuthMessageHandler>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = FileUploadConfig.MaxFileSize;
});

//Mud blazor implementation
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.VisibleStateDuration = 1500;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.PreventDuplicates = true;
});

builder.Services.AddLocalization();


// HttpClient → WebAPI
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrWhiteSpace(apiBaseUrl))
    throw new InvalidOperationException("ApiBaseUrl belum di-set di BlazorUI/appsettings.json");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Service FE yang memanggil API
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<ICourseBookingService, CourseBookingService>();
builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
builder.Services.AddScoped<ICourseSessionService, CourseSessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}


var cultureInfo = new CultureInfo("id-ID");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// cookies proxy api
app.MapPost("/auth/proxy-login", async (HttpContext context, IHttpClientFactory httpFactory) =>
{
    using var sr = new StreamReader(context.Request.Body);
    var body = await sr.ReadToEndAsync();

    var backend = httpFactory.CreateClient("ServerAPI");
    var req = new HttpRequestMessage(HttpMethod.Post, "api/auth/login")
    {
        Content = new StringContent(body, Encoding.UTF8, context.Request.ContentType ?? "application/json")
    };

    var res = await backend.SendAsync(req);

    if (res.Headers.TryGetValues("Set-Cookie", out var cookies))
    {
        foreach (var cookie in cookies)
        {
            context.Response.Headers.Append("Set-Cookie", cookie);
        }
    }

    context.Response.StatusCode = (int)res.StatusCode;
    await res.Content.CopyToAsync(context.Response.Body);
});

app.MapPost("/auth/proxy-logout", (HttpContext context) =>
{
    var options = new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddDays(-1),
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Path = "/"
    };

    context.Response.Cookies.Append("AuthToken", "", options);
    context.Response.Cookies.Append("RefreshToken", "", options);

    return Results.Ok();
});

app.MapPost("/auth/proxy-refresh-token", async (HttpContext context, IHttpClientFactory httpFactory) =>
{
    var backend = httpFactory.CreateClient("ServerAPI");

    var req = new HttpRequestMessage(HttpMethod.Post, "/api/auth/refresh-token");

    // Forward cookies from browser to backend
    var cookieHeader = string.Join("; ", context.Request.Cookies.Select(kvp => $"{kvp.Key}={kvp.Value}"));
    req.Headers.Add("Cookie", cookieHeader);

    var res = await backend.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
    var content = await res.Content.ReadAsStringAsync();

    // Forward Set-Cookie headers back to browser
    if (res.Headers.TryGetValues("Set-Cookie", out var cookies))
        foreach (var cookie in cookies)
            context.Response.Headers.Append("Set-Cookie", cookie);

    context.Response.StatusCode = (int)res.StatusCode;
    context.Response.ContentType = "application/json";

    await context.Response.WriteAsync(content);
});

// app.Run();
await app.RunAsync();