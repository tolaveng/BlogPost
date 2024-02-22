using Core.Application.Ioc;
using Core.Infrastructure.Ioc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Auth;
using WebApp.Components.TToast;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<ITToast, TToastProvider>();


// Requires all users to be authenticated - NOT WORK with Blazor razor component
// Solution: add [Authorize] attribute to _Import.razor
// builder.services.AddAuthorization(opt =>
//{
//    opt.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

builder.Services.AddOptions();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddAuthorizationCore();

//builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "LocalNextJs",
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:3000")
                            .WithMethods("GET")
                            .AllowAnyHeader();
                      });
});


// Add Swagger
builder.Services.AddControllers();
// Add controller calls AddApiExplorer (MVC), AddEndpointsApiExplorer (minimal Api)
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//---------------- App -----------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("LocalNextJs");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Run();
