public static class CustomWindowsUserMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomWindowsUserForDevelopment(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<CustomWindowsUserMiddlewareForDebugOnly>();
    }

}
