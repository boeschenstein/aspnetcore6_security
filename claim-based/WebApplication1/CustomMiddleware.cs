// Source: https://stackoverflow.com/questions/55337045/where-to-set-custom-claimsprincipal-for-all-httprequests
using System.Security.Claims;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // DI per request: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5.0#per-request-middleware-dependencies
    public async Task InvokeAsync(HttpContext httpContext)
    {
        Console.WriteLine($"============== CustomWindowsUserMiddleware: User: {httpContext.User}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User type: {httpContext.User.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.IsInRole(\"docker-users\"): {httpContext.User.IsInRole("docker-users")}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.IsInRole(\"EmployeeNumber\"): {httpContext.User.IsInRole("EmployeeNumber")}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.HasClaim(x => x.Type.Contains(\"EmployeeNumber\")): {httpContext.User.HasClaim(x => x.Type.Contains("EmployeeNumber"))}");

        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity: {httpContext.User.Identity}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity type: {httpContext.User.Identity?.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.Name: {httpContext.User.Identity?.Name}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.IsAuthenticated: {httpContext.User.Identity?.IsAuthenticated}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.AuthenticationType: {httpContext.User.Identity?.AuthenticationType}");

        // todo: test with client certificate:
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate: {httpContext.Connection.ClientCertificate}");
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate.Subject: {httpContext.Connection.ClientCertificate?.Subject}");
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate type: {httpContext.Connection.ClientCertificate?.GetType()}");

        var сlaimsIdentity = httpContext.User.Identity as ClaimsIdentity;
        сlaimsIdentity?.AddClaim(new Claim("EmployeeNumber", "12345"));

        await _next(httpContext);
    }
}
