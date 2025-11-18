# My Own Message Broker

A lightweight, self-built message broker implementation using ASP.NET Core 9.0 and Entity Framework Core. This project demonstrates the core concepts of publish-subscribe messaging patterns with a REST API interface.

## 📋 Overview

This message broker system consists of two main components:

1. **MyOwnMessageBroker** - REST API service that manages topics, subscriptions, and message publishing
2. **Subscriber** - Worker service that consumes messages from the broker

The system uses SQLite for data persistence and follows a publish-subscribe (pub-sub) architecture pattern.

## 🏗️ Architecture

### Core Components

- **Topics** - Message channels that producers publish to
- **Subscriptions** - Links between topics and consumers
- **Messages** - Payloads with metadata including expiration and status tracking
- **Message Status** - Tracks message lifecycle (New, Requested, Sent, etc.)

### Technology Stack

- **.NET 9.0**
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0** - ORM for SQLite
- **SQLite** - Lightweight database
- **Docker** - Containerization

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- Docker & Docker Compose (optional)
- Git

### Installation

1. Clone the repository:
```bash
git clone https://github.com/alineitgirl/my-own-message-broker.git
cd my-own-message-broker
```

2. Navigate to the project directory:
```bash
cd MyOwnMessageBroker
```

3. Restore dependencies:
```bash
dotnet restore
```

4. Apply database migrations:
```bash
dotnet ef database update
```

### Running Locally

#### Start the Message Broker API

```bash
cd MyOwnMessageBroker
dotnet run
```

The API will be available at `https://localhost:7005`

#### Start the Subscriber Worker

In another terminal:
```bash
cd Subscriber
dotnet run
```

This will build and start the message broker.

## 📡 API Endpoints

### Topics

#### Create a Topic
```
POST /api/topics
Content-Type: application/json

{
  "name": "orders"
}
```

#### Get All Topics
```
GET /api/topics
```

#### Get Topic by ID
```
GET /api/topics/{id}
```

### Messages

#### Publish Message to Topic
```
POST /api/topics/{id}/messages
Content-Type: application/json

{
  "topicMessage": "Your message content",
}
```

### Subscriptions

- **GET /api/topics/{topicId}/subscriptions** - List all subscriptions
- **POST /api/topics/{topicId}subscriptions** - Create new subscription
- **GET /api/topics/{topicId}/subscriptions/{subscriptionId}** - Get subscription by ID

## 📊 Data Models

### Topic
```csharp
public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Message
```csharp
public class Message
{
    public int Id { get; set; }
    public DateTime ExpiresAfter { get; set; }
    public string TopicMessage { get; set; }
    public MessageStatus MessageStatus { get; set; }
    public int SubscriptionId { get; set; }
}
```

### MessageStatus Enum
- **New** - Message just created
- **Requested** - Message has been sent to consumer
- **Sent** - Message delivery has been confirmed

### Subscription
- Links topics with consumer endpoints
- Manages message delivery to subscribers

## ⚙️ Configuration

Configuration is managed through `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MessageBrokerDb.db;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Database

The project uses SQLite with a local database file `MessageBrokerDb.db`. The database schema is managed through Entity Framework Core migrations.

## 📁 Project Structure

```
MyOwnMessageBroker/
├── MyOwnMessageBroker/           # Main API project
│   ├── Controllers/              # API endpoints
│   │   ├── TopicController.cs
│   │   └── SubscriptionController.cs
│   ├── Models/                   # Domain models
│   │   ├── Topic.cs
│   │   ├── Message.cs
│   │   └── Subscription.cs
│   ├── Services/                 # Business logic
│   │   ├── TopicService.cs
│   │   ├── MessageService.cs
│   │   └── SubscriptionService.cs
│   ├── Data/                     # Database context
│   │   └── AppDbContext.cs
│   ├── Enums/                    # Enumerations
│   │   └── MessageStatus.cs
│   ├── Migrations/               # EF Core migrations
│   ├── Program.cs                # Application configuration
│   ├── appsettings.json          # Configuration
│   └── Dockerfile                # Docker configuration
│
├── Subscriber/                   # Subscriber worker project
│   ├── Program.cs
│   ├── SubscriberWorker.cs       # Background worker service
│   ├── Dtos/
│   │   └── MessageReadDto.cs
│   └── Subscriber.csproj
│
├── compose.yaml                  # Docker Compose configuration
└── README.md                      # This file
```

## 🔄 Workflow Example

1. **Create a Topic**
   ```bash
   POST /api/topics
   { "name": "order-events" }
   ```

2. **Create a Subscription**
   - Link the topic to a subscriber endpoint

3. **Publish a Message**
   ```bash
   POST /api/topics/1/messages
   {
     "topicMessage": "Order #12345 created",
   }
   ```

4. **Subscribe to Messages**
   - The Subscriber worker polls for new messages
   - Processes messages and updates status

## 📝 Database Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName -p MyOwnMessageBroker
```

Apply migrations:
```bash
dotnet ef database update -p MyOwnMessageBroker
```

## 🔐 Security Considerations

- Enable HTTPS in production
- Implement authentication/authorization
- Add rate limiting
- Validate all inputs
- Use connection string secrets management

## 📄 License

This project is provided as-is for educational and development purposes.

**Last Updated:** November 2025
