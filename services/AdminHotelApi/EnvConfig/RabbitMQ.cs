namespace AdminHotelApi.EnvConfig;

public class RabbitMQ
{
    public static string Host => Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
    public static int Port => int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ_PORT"), out var port) ? port : 5673;
    public static string UserName => Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
    public static string Password => Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
    public static string HotelQueue => Environment.GetEnvironmentVariable("RABBITMQ_HOTEL_QUEUE") ?? "hotel-queue";
}
