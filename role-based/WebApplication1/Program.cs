using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// config Kestrel to require certificate (only for debug)
// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0#configure-your-server-to-require-certificates
//builder.WebHost.ConfigureKestrel(o =>
//{
//    // todo: how to create a client certificate?
//    //o.ConfigureHttpsDefaults(o => o.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

//builder.Services.AddRazorPages(); // todo: for what is this?

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseCustomWindowsUser(); // middleware is getting ClaimsPrincipal + ClaimsIdentity instead of WindowsPrincipal+WindowsIdentity!

app.UseAuthentication();

app.UseCustomWindowsUser(); // after app.UseAuthentication() ! middleware is getting WindowsPrincipal+WindowsIdentity (on the second execution)

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
