using System.ComponentModel.DataAnnotations;

namespace MyOwnMessageBroker.Models;

public class Topic
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
}