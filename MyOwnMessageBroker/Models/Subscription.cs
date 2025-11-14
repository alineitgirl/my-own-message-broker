using System.ComponentModel.DataAnnotations;

namespace MyOwnMessageBroker.Models;

public class Subscription
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public int TopicId { get; set; }
}