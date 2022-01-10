# ASP.NET Core 6 Security: Authentication, Authorization

## Authentication

Authentication is the process of ascertaining who a user is.
Authentication is the process of determining a user's identity.

### Configure Windows Authentication

https://docs.microsoft.com/en-us/aspnet/core/security/authentication/windowsauth?view=aspnetcore-6.0&tabs=visual-studio

## Authorization

Authorization is the process of determining whether a user has access to a resource. I

Authorization is orthogonal and independent from authentication. However, authorization requires an authentication mechanism. Authentication is the process of ascertaining who a user is. Authentication may create one or more identities for the current user.

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

https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0
