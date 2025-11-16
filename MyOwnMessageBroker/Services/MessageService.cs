using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Data;
using MyOwnMessageBroker.Enums;
using MyOwnMessageBroker.Models;

namespace MyOwnMessageBroker.Services;

public class MessageService
{
    private readonly AppDbContext _dbContext;

    public MessageService(AppDbContext context)
    {
        _dbContext = context;
    }

    public async Task AddMessage(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IQueryable<Message>> GetAllMessagesBySubscriptionId(int id)
    {
        var messages = _dbContext.Messages.Where(m => m.SubscriptionId == id);

        if (messages is not null)
        {
            return messages;
        }

        return null;
    }

    public async Task SetMessageStatusToSentBySubscriptionsId(List<int> subscriptionsIds)
    {
        foreach (var message in subscriptionsIds.Select(id => _dbContext.Messages
                         .Where(m => m.SubscriptionId == id))
                     .SelectMany(messages => messages))
        {
            message.MessageStatus = MessageStatus.Sent;
            _dbContext.Update(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}