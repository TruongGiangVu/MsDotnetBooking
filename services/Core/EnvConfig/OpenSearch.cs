namespace Core.EnvConfig;

public class OpenSearch
{
    public static string Url => Environment.GetEnvironmentVariable("OPENSEARCH_URL") ?? "http://localhost:9200";
    public static string UserName => Environment.GetEnvironmentVariable("OPENSEARCH_USER") ?? string.Empty;
    public static string Password => Environment.GetEnvironmentVariable("OPENSEARCH_PASS") ?? string.Empty;
    public static string HotelIndex => Environment.GetEnvironmentVariable("OPENSEARCH_HOTEL_INDEX") ?? string.Empty;
}
