// Source: https://stackoverflow.com/questions/55337045/where-to-set-custom-claimsprincipal-for-all-httprequests
public class CustomWindowsUserMiddleware
{
    private readonly RequestDelegate _next;

    public CustomWindowsUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // DI per request: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5.0#per-request-middleware-dependencies
    public async Task InvokeAsync(HttpContext httpContext)
    {
        Console.WriteLine($"============== CustomWindowsUserMiddleware: User: {httpContext.User}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User type: {httpContext.User.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.IsInRole(\"docker-users\"): {httpContext.User.IsInRole("docker-users")}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.IsInRole(\"dummy_2\"): {httpContext.User.IsInRole("dummy_2")}");

        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity: {httpContext.User.Identity}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity type: {httpContext.User.Identity?.GetType()}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.Name: {httpContext.User.Identity?.Name}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.IsAuthenticated: {httpContext.User.Identity?.IsAuthenticated}");
        Console.WriteLine($"CustomWindowsUserMiddleware: User.Identity.AuthenticationType: {httpContext.User.Identity?.AuthenticationType}");

        // todo: test with client certificate:
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate: {httpContext.Connection.ClientCertificate}");
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate.Subject: {httpContext.Connection.ClientCertificate?.Subject}");
        Console.WriteLine($"CustomWindowsUserMiddleware: Connection.ClientCertificate type: {httpContext.Connection.ClientCertificate?.GetType()}");

        if (httpContext.User is System.Security.Principal.WindowsPrincipal && httpContext.User.Identity is System.Security.Principal.WindowsIdentity)
        {
            List<string> roles = new List<string>();
            roles.Add("dummy_1");
            if (httpContext.User.IsInRole("docker-users"))
            {
                roles.Add("docker-users"); // example: reuse an exiting AD role

                roles.Add("dummy_2"); // add a new custom role which does not exist in AD -> check Authenticate attribute in HomeController
                //roles.Add("dummy_3"); // add a new custom role which does not exist in AD -> check Authenticate attribute in HomeController
            }
            var principal = new MyCustomPrincipal(httpContext.User.Identity, roles.ToArray(), "my_custom_id", "test_firstname", "test_lastname", "MyOrganization");

            //throw new AuthenticationException($"test");

            Console.WriteLine($"CustomWindowsUserMiddleware: setting new principal: {principal}");
            httpContext.User = principal;
        }
        else
        {
            Console.WriteLine($"CustomWindowsUserMiddleware: !!! wrong type (no support for 'Windows Authentication'), ignoring for now !!!");
        }

        await _next(httpContext);
    }
}
