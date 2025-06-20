using System.Text.Json.Serialization;

namespace Core.Dtos;

/// <summary>
/// wrapper class for push and consume queue with payload
/// </summary>
public class QueueDto<T>
{
    [JsonPropertyOrder(1)]
    public QueueAction Action { get; set; } = QueueAction.None;

    [JsonPropertyOrder(2)]
    public T? Payload { get; set; } = default;

    public QueueDto(QueueAction action = QueueAction.None, T? payload = default)
    {
        Action = action;
        Payload = payload;
    }

    public QueueDto() { }
}
