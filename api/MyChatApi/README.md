# MyChat API

A .NET 9 minimal API that provides a backend service for chat conversations using the Ollama AI model. This API allows for creating, managing, and conducting AI-powered conversations.

## Features

- **Conversation Management**
  - Create, read, update, and delete conversations
  - Maintain conversation history
  - Automatic conversation title suggestion based on content
  - In-memory database storage

- **AI Integration**
  - Powered by Ollama AI (llama3 model)
  - Stream-based responses for real-time chat interaction
  - Contextual conversation handling

## Tech Stack

- .NET 9
- Entity Framework Core (In-Memory Database)
- OllamaSharp for AI integration
- Minimal API architecture

## Prerequisites

- .NET 9 SDK
- Ollama running locally on port 11434
- llama3 model installed in Ollama

## API Endpoints

### Conversations

- `GET /conversation` - Get all conversations
- `GET /conversation/{id}` - Get a specific conversation
- `GET /conversation/{id}/header` - Get conversation header information
- `POST /conversation` - Create a new conversation
- `PUT /conversation/{id}` - Update an existing conversation
- `PUT /conversation/{id}/header` - Update conversation header
- `DELETE /conversation/{id}` - Delete a conversation

### Messages

- `POST /conversation/{id}/message` - Add a message to a conversation
- `POST /conversation/ask/{id}` - Send a message and get AI response
- `POST /conversation/suggest-title` - Get an AI-generated title suggestion

## Data Models

### ConversationDto
```json
{
  "id": int,
  "title": string,
  "messages": MessageDto[]
}
```

### MessageDto
```json
{
  "id": int,
  "text": string,
  "source": string,
  "timestamp": datetime
}
```

## CORS

Since this is not intended for real production use the API has CORS enabled and allows:
- Any origin
- Any header
- Any method

## Getting Started

1. Ensure Ollama is running locally with the llama3 model installed
2. Run the API project
3. The API will be available at https://localhost:7192 by default

## Notes

- The API uses an in-memory database, so data will be lost when the application restarts
- Chat responses have a 5-minute timeout configured
- The API supports server-sent events for streaming AI responses