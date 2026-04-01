using System.IO;
using DotNetEnv;

namespace Data.Extensions;

public static class DotEnvExtensions
{
    private static bool _isLoaded;
    private static void LoadEnv()
    {
        if (_isLoaded) return;

        var basePath = AppContext.BaseDirectory;
        var envPath = Path.Combine(basePath, ".env");

        if (!File.Exists(envPath))
            throw new FileNotFoundException($".env not found: {envPath}");

        Env.Load(envPath);
        _isLoaded = true;
    }

    public static string GetConnectionString()
    {
        LoadEnv();

        return Environment.GetEnvironmentVariable("CONNECTION_STRING")
               ?? throw new InvalidOperationException("CONNECTION_STRING not found in .env");
    }
}