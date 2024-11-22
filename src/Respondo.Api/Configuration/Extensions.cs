namespace Respondo.Api.Configuration;

public static class Extensions
{
    public static string[] GetAllowedOrigins(this IConfiguration configuration)
    {
        var allowedHostsString = configuration.GetSection("AllowedOrigins").Get<string>();
        return allowedHostsString?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    }
}