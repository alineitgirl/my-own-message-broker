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

    public async Task SetMessageStatusToSentBySubscriptionsId(List<int> messageIds)
    {
        var messages = await _dbContext.Messages
            .Where(m => messageIds.Contains(m.Id))
            .ToListAsync();

        if (!messages.Any())
        {
            return;
        }

        foreach (var message in messages)
        {
            message.MessageStatus = MessageStatus.Sent;
        }

        await _dbContext.SaveChangesAsync();
    }
}