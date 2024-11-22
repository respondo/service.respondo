namespace Respondo.Api.Configuration;

public static class Extensions
{
    public static string[] GetAllowedHosts(this IConfiguration configuration)
    {
        var allowedHostsString = configuration.GetSection("AllowedHosts").Get<string>();
        return allowedHostsString?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    }
}