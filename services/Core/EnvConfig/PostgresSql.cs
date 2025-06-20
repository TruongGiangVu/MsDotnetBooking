namespace Core.EnvConfig;

public class PostgresSql
{
    public static string ConnectionString => Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING") ?? string.Empty;
}
