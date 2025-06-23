namespace AdminHotelApi.EnvConfig;

public class PostgresSql
{
    public static string ConnectionString => Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING") ?? "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=postgres";
}
