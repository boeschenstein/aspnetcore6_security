public static class CustomWindowsUserMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomWindowsUser(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<CustomWindowsUserMiddleware>();
    }

}