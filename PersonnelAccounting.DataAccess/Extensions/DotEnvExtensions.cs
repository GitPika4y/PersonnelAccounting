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
            throw new FileNotFoundException($".env не найден по пути: {envPath}.\n Убедитесь, что он есть и содержит строку подключения CONNECTION_STRING к серверу БД");

        Env.Load(envPath);
        _isLoaded = true;
    }

    public static string GetConnectionString()
    {
        LoadEnv();

        return Environment.GetEnvironmentVariable("CONNECTION_STRING")
               ?? throw new InvalidOperationException("Строка подключения CONNECTION_STRING не найдена в .env.");
    }
}