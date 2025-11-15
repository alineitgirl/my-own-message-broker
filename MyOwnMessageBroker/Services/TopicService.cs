using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Data;
using MyOwnMessageBroker.Models;

namespace MyOwnMessageBroker.Services;

public class TopicService
{
    private readonly AppDbContext _dbContext;
    
    public TopicService(AppDbContext context)
    {
        _dbContext = context;
    }
    public async Task AddTopic(Topic topic)
    {
        await _dbContext.Topics.AddAsync(topic);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<Topic>> GetAllTopics()
    {
        var topics = await _dbContext.Topics.ToListAsync();

        return topics;
    }

    public async Task<Topic> GetTopicById(int id)
    {
        var topic = await _dbContext.Topics.FirstOrDefaultAsync(t => t.Id == id);

        if (topic is null)
        {
            return null;
        }

        return topic;
    }
    
}