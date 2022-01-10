# ASP.NET Core 6 Security: Authentication, Authorization

## Authentication

Authentication is the process of ascertaining who a user is.
Authentication is the process of determining a user's identity.

### Configure Windows Authentication

https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-6.0&tabs=visual-studio

## Authorization

Authorization is the process of determining whether a user has access to a resource. I

Authorization is orthogonal and independent from authentication. However, authorization requires an authentication mechanism. Authentication is the process of ascertaining who a user is. Authentication may create one or more identities for the current user.

## Role-Based

CustomWindowsUserMiddleware handler:

```cs
public class CustomWindowsUserMiddleware
{
    private readonly RequestDelegate _next;

    public CustomWindowsUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // DI per request: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-6.0#per-request-middleware-dependencies
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.User is System.Security.Principal.WindowsPrincipal && httpContext.User.Identity is System.Security.Principal.WindowsIdentity)
        {
            List<string> roles = new List<string>();
            roles.Add("dummy_1");
            if (httpContext.User.IsInRole("docker-users")) // IsInRole needs Windows Authentication in IIS/Kestrel/IIExpress
            {
                roles.Add("docker-users"); // example: reuse an exiting AD role
                roles.Add("dummy_2"); // add a new custom role which does not exist in AD -> check Authenticate attribute in HomeController
            }
            var principal = new MyCustomPrincipal(httpContext.User.Identity, roles.ToArray(), "my_custom_id", "test_name";
            httpContext.User = principal;
        }
        else
        {
            Console.WriteLine($"CustomWindowsUserMiddleware: !!! wrong type (no support for 'Windows Authentication'), ignoring for now !!!");
        }

        await _next(httpContext);
    }
}
```

Custom Principal:

```cs
public class MyCustomPrincipal : GenericPrincipal
{
    public MyCustomPrincipal(IIdentity identity, string[] roles, string myCustomInfo1, string myCustomInfo2)
        : base(identity, roles)
    {
        MyCustomInfo1 = myCustomInfo1;
        MyCustomInfo2 = myCustomInfo2;
    }

    public string MyCustomInfo1 { get; private set; }
    public string MyCustomInfo2 { get; private set; }
}
```

Authorize Attribute examples:

```cs
[Authorize(Roles = "dummy_2")] // access
[Authorize(Roles = "dummy_3")] // no access

// OR: role dummy_2 OR dummy_3 needed
[Authorize(Roles = "dummy_2,dummy_3")] // OR

// AND: if on separate line: role dummy_2 AND dummy_3 needed
[Authorize(Roles = "dummy_2")]
[Authorize(Roles = "dummy_3")]
```

>"Policy based role checks" (Convert Role-based to Policy-based)
- https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0#policy-based-role-checks
- https://andrewlock.net/introduction-to-authorisation-in-asp-net-core/#authorising-based-on-roles

## Claims-Based

## Policy-Based

## IIS (Internet Information Service)

### .NET CLR Version

For .NET Core or .NET 6 choose: `No Managed Code` (App Pool Settings for your app pool)

### Windows Authentication

To Enable "Windows Authentication", open "Authentication" on your website and 
- disable "Anonymous Authentication" and 
- enable "Windows Authentication".

### Require Client Certificate

To force the browser to request a client certificate, add this setting in IIS:

> SSL Settings: Client Certifications: [x] require

Now you get access to the certificate:

```cs
var clientCertificate = await context.HttpContext.Connection.GetClientCertificateAsync(); // async
var clientCertificate = context.HttpContext.Connection.ClientCertificate; // sync
```

## Information

- https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0
