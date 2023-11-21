using Core.Application.Ioc;
using Core.Infrastructure.Ioc;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);



// Requires all users to be authenticated - NOT WORK with Blazor razor component
// Solution: add [Authorize] attribute to _Import.razor
// builder.services.AddAuthorization(opt =>
//{
//    opt.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
