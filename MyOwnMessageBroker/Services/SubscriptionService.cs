using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Data;
using MyOwnMessageBroker.Models;

namespace MyOwnMessageBroker.Services;

public class SubscriptionService
{
    private readonly AppDbContext _dbContext;
    private readonly MessageService _messageService;

    public SubscriptionService(AppDbContext context, MessageService messageService)
    {
        _dbContext = context;
        _messageService = messageService;
    }

    public async Task PublishMessageToATopic(int topicId, Message message)
    {
        var subs = await _dbContext.Subscriptions
            .Where(s => s.TopicId == topicId)
            .ToListAsync();

        foreach (var sub in subs)
        {
            var msg = new Message
            {
                TopicMessage = message.TopicMessage,
                SubscriptionId = sub.Id,
                ExpiresAfter = message.ExpiresAfter,
                MessageStatus = message.MessageStatus
            };

            await _messageService.AddMessage(msg);
        }
    }

    public async Task<Subscription> CreateSubscriptionAsync(int topicId, string name)
    {
        var sub = new Subscription
        {
            TopicId = topicId,
            Name = name
        };

        _dbContext.Subscriptions.Add(sub);
        await _dbContext.SaveChangesAsync();

        return sub;
    }

    public async Task<IEnumerable<Subscription>> GetAllSubscriptions()
    {
        var subscriptions = await _dbContext.Subscriptions.ToListAsync();

        return subscriptions;
    }

    public async Task<Subscription?> GetSubscriptionById(int id)
    {
        return await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id);
    }

}