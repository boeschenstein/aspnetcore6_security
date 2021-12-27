using System.Security.Authentication;

// Source: https://stackoverflow.com/questions/55337045/where-to-set-custom-claimsprincipal-for-all-httprequests
public class CustomWindowsUserMiddlewareForDebugOnly
{
    private readonly RequestDelegate _next;

    public CustomWindowsUserMiddlewareForDebugOnly(RequestDelegate next)
    {
        _next = next;
    }

    // DI per request: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5.0#per-request-middleware-dependencies
    public async Task InvokeAsync(HttpContext httpContext)
    {
        Console.WriteLine($"============== CustomWindowsUserMiddlewareForDebugOnly: User: {httpContext.User}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User type: {httpContext.User.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.IsInRole(\"docker-users\"): {httpContext.User.IsInRole("docker-users")}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.IsInRole(\"dummy_2\"): {httpContext.User.IsInRole("dummy_2")}");

        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.Identity: {httpContext.User.Identity}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.Identity type: {httpContext.User.Identity?.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.Identity.Name: {httpContext.User.Identity?.Name}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.Identity.IsAuthenticated: {httpContext.User.Identity?.IsAuthenticated}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: User.Identity.AuthenticationType: {httpContext.User.Identity?.AuthenticationType}");

        // todo: test with client certificate:
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: Connection.ClientCertificate: {httpContext.Connection.ClientCertificate}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: Connection.ClientCertificate.Subject: {httpContext.Connection.ClientCertificate?.Subject}");
        Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: Connection.ClientCertificate type: {httpContext.Connection.ClientCertificate?.GetType()}");

        if (httpContext.User is System.Security.Principal.WindowsPrincipal && httpContext.User.Identity is System.Security.Principal.WindowsIdentity)
        {
            List<string> roles = new List<string>();
            roles.Add("dummy_1");
            if (httpContext.User.IsInRole("docker-users")) {
                roles.Add("docker-users"); // example: reuse an exiting AD role

                roles.Add("dummy_2"); // add a new custom role which does not exist in AD -> check Authenticate attribute in HomeController
            }
            var principal = new MyCustomPrincipal(httpContext.User.Identity, roles.ToArray(), "my_custom_id", "test_firstname", "test_lastname", "MyOrganization");

            //throw new AuthenticationException($"test");

            Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: setting new principal: {principal}");
            httpContext.User = principal;
        }
        else
        {
            Console.WriteLine($"CustomWindowsUserMiddlewareForDebugOnly: !!! wrong type (no support for 'Windows Authentication'), ignoring for now !!!");
        }

        await _next(httpContext);
    }
}
