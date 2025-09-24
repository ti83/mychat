using Microsoft.EntityFrameworkCore;
using MyChatApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ConversationDb>(opt => opt.UseInMemoryDatabase("ConversationList"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ConversationService>();
var app = builder.Build();


RouteGroupBuilder conversations = app.MapGroup("/conversation");
var conversationService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ConversationService>();
conversations.MapGet("/", conversationService.GetAllConversations);
conversations.MapGet("/{id}", conversationService.GetConversation);
conversations.MapPost("/", conversationService.CreateNewConversation);
conversations.MapPut("/{id}", conversationService.UpdateConversation);
conversations.MapPost("/{id}/message", conversationService.AddMessageToConversation);

app.Run();

