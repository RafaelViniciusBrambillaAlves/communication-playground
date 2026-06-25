namespace RabbitMQService.Infrastructure.Settings;

public sealed class RabbitMQSettings
{
    public string Url { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string CreatedQueue { get; set; } = string.Empty;
    public string UpdatedQueue { get; set; } = string.Empty;
    public string DeletedQueue { get; set; } = string.Empty;
}