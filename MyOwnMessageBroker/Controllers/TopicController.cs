using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyOwnMessageBroker.Models;
using MyOwnMessageBroker.Services;

namespace MyOwnMessageBroker.Controllers;

[ApiController]
[Route("api/topics")]
public class TopicController : ControllerBase
{
    private readonly TopicService _topicService;
    private readonly SubscriptionService _subscriptionService;

    public TopicController(TopicService topicService, SubscriptionService subscriptionService)
    {
        _topicService = topicService;
        _subscriptionService = subscriptionService;
    }
    
    [HttpPost(Name = "CreateANewTopic")]
    public async Task<IActionResult> CreateTopicEndpoint([FromBody]Topic topic)
    {
        await _topicService.AddTopic(topic);

        return CreatedAtRoute(
            "GetTopicById",
            new { Id = topic.Id},
            topic
            );
    }

    [HttpGet("{id}", Name = "GetTopicById")]
    public async Task<IActionResult> GetByIdEndpoint(int id)
    {
        var topic = await _topicService.GetTopicById(id);

        if (topic == null)
            return NotFound();

        return Ok(topic);

    }

    [HttpGet(Name = "GetAllTopics")]
    public async Task<IActionResult> GetAllTopicsEndpoint()
    {
        var topics = await _topicService.GetAllTopics();

        if (topics is null)
        {
            return NotFound("No topics provided.");
        }
        
        return Ok(topics);
    }

    [HttpPost("{id}/messages", Name = "PublishMessageToATopic")]
    public async Task<IActionResult> PublishMessageToATopicEndpoint([FromRoute]int id, [FromBody]Message message)
    {
        var isExistsTopic = _topicService.GetTopicById(id).Result;

        if (isExistsTopic == null)
        {
            return NotFound("Topic not found.");
        }

        var subs = _subscriptionService.PublishMessageToATopic(id, message);

        if (subs.IsCompletedSuccessfully)
        {
            return Ok("Message has been added.");
        }

        return BadRequest("Something went wrong. Please try again later");
    }
    
    
}