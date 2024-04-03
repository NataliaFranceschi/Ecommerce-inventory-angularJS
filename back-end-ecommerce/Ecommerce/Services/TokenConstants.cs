using Microsoft.Extensions.Configuration;

public static class TokenConstants
{
    private static IConfiguration _configuration;

    public static string Secret => _configuration["Secret"];

    internal static void Configure(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}

