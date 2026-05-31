using Npgsql;

namespace TaskFlow.Api.Data;

// Resolve a connection string aceitando tanto a URI do Supabase/Render
// (ex.: postgresql://user:senha@host:5432/postgres) quanto o formato nativo do Npgsql.
public static class DbConnectionHelper
{
    public static string Resolver(IConfiguration config)
    {
        var url = Environment.GetEnvironmentVariable("DATABASE_URL")
                  ?? config.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new InvalidOperationException(
                "Connection string ausente. Defina a variavel de ambiente DATABASE_URL " +
                "ou ConnectionStrings:DefaultConnection no appsettings.");
        }

        // Ja esta no formato Npgsql (key=value)? Usa direto.
        if (url.Contains("Host=", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        // Converte a URI postgres:// para o formato do Npgsql.
        var uri = new Uri(url);
        var userInfo = uri.UserInfo.Split(':', 2);

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
            Database = uri.AbsolutePath.TrimStart('/'),
            SslMode = SslMode.Require,
            TrustServerCertificate = true
        };

        return builder.ConnectionString;
    }
}
