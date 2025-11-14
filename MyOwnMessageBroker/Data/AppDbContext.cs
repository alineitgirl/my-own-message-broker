using Microsoft.EntityFrameworkCore;
using MyOwnMessageBroker.Enums;
using MyOwnMessageBroker.Models;

namespace MyOwnMessageBroker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Message> Messages => Set<Message>();
}