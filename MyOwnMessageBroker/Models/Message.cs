using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MyOwnMessageBroker.Enums;

namespace MyOwnMessageBroker.Models;

public class Message
{
    public int Id { get; set; }

    [Required] public DateTime ExpiresAfter { get; set; } = DateTime.UtcNow.AddDays(1);

    [Required] public string? TopicMessage { get; set; } = string.Empty;

    [Required] public MessageStatus MessageStatus { get; set; } = MessageStatus.New;

    [Required]
    public int SubscriptionId { get; set; }
    
}