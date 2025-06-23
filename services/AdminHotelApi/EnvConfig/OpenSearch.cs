namespace AdminHotelApi.EnvConfig;

public class OpenSearch
{
    public static string Url => Environment.GetEnvironmentVariable("OPENSEARCH_URL") ?? "https://localhost:9200";
    public static string UserName => Environment.GetEnvironmentVariable("OPENSEARCH_USER") ?? "admin";
    public static string Password => Environment.GetEnvironmentVariable("OPENSEARCH_PASS") ?? "openSearch@123";
    public static string HotelIndex => Environment.GetEnvironmentVariable("OPENSEARCH_HOTEL_INDEX") ?? "hotel";
}
