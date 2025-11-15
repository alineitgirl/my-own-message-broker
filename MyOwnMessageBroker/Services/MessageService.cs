using Microsoft.AspNetCore.Mvc;
using MyOwnMessageBroker.Data;
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
}