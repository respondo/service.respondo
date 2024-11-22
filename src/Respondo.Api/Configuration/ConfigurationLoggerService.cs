namespace Respondo.Api.Configuration;

public class ConfigurationLoggerService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigurationLoggerService> _logger;

    public ConfigurationLoggerService(IConfiguration configuration, ILogger<ConfigurationLoggerService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var environment = _configuration["ASPNETCORE_ENVIRONMENT"];
        var allowedHosts = _configuration.GetAllowedOrigins();

        _logger.LogInformation("Environment: {Environment}", environment);
        _logger.LogInformation("Allowed Hosts: {AllowedHosts}", string.Join(", ", allowedHosts));
        
        return Task.CompletedTask;
    }
}