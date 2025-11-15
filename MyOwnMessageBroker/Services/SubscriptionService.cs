using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Data;
using MyOwnMessageBroker.Models;

namespace MyOwnMessageBroker.Services;

public class SubscriptionService
{
    private readonly AppDbContext _dbContext;
    private readonly MessageService _messageService;

    public SubscriptionService(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task CreateASubscriptionToATopic(int id, Message message)
    {
        var isExistsSubscriptionByTopicId = await _dbContext.Subscriptions.AnyAsync(s => s.TopicId == id);

        if (isExistsSubscriptionByTopicId)
        {
            var subs = _dbContext.Subscriptions.Where(s => s.Id == id);

            foreach (var sub in subs)
            {
                var msg = new Message
                {
                    TopicMessage = message.TopicMessage,
                    SubscriptionId = sub.Id,
                    ExpiresAfter = message.ExpiresAfter,
                    MessageStatus = message.MessageStatus
                };
                await _messageService.AddMessage(message);
            }
        }
    }
}