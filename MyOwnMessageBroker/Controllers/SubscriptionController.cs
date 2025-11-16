using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyOwnMessageBroker.Models;
using MyOwnMessageBroker.Services;

namespace MyOwnMessageBroker.Controllers;

[ApiController]
[Route("api/topics/{topicId}/subscriptions")]
public class SubscriptionController : ControllerBase
{
    private readonly TopicService _topicService;
    private readonly SubscriptionService _subscriptionService;
    private readonly MessageService _messageService;

    public SubscriptionController(
        TopicService topicService,
        SubscriptionService subscriptionService,
        MessageService messageService)
    {
        _topicService = topicService;
        _subscriptionService = subscriptionService;
        _messageService = messageService;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateSubscription(
        [FromRoute] int topicId,
        [FromBody] Subscription subscription)
    {
        var topic = await _topicService.GetTopicById(topicId);

        if (topic == null)
            return NotFound("Topic not found.");

        var created = await _subscriptionService.CreateSubscriptionAsync(topicId, subscription.Name);

        return CreatedAtRoute(
            "GetSubscriptionById",
            new {
                topicId = created.TopicId,
                subscriptionId = created.Id
            },
            created);
    }

    [HttpGet("")]
    public async Task<IEnumerable<Subscription>> GetAll(
        [FromRoute] int topicId)
    {
        return await _subscriptionService.GetAllSubscriptions();
    }

    [HttpGet("{subscriptionId}", Name = "GetSubscriptionById")]
    public async Task<IActionResult> GetById(
        [FromRoute] int topicId,
        [FromRoute] int subscriptionId)
    {
        var subscription = await _subscriptionService.GetSubscriptionById(subscriptionId);

        if (subscription == null)
            return NotFound();

        return Ok(subscription);
    }

    [HttpGet("{subscriptionId}/messages")]
    public async Task<IActionResult> GetMessages(
        [FromRoute] int topicId,
        [FromRoute] int subscriptionId)
    {
        var msgs = await _messageService.GetAllMessagesBySubscriptionId(subscriptionId);

        if (msgs == null)
            return NotFound();

        return Ok(msgs);
    }

    [HttpPost("{subscriptionId}/messages")]
    public async Task<IActionResult> ConfirmDelivery(
        [FromRoute] int topicId,
        [FromRoute] int subscriptionId,
        [FromBody] List<int> messageIds)
    {
        await _messageService.SetMessageStatusToSentBySubscriptionsId(messageIds);

        return Ok("Messages status updated.");
    }
}
