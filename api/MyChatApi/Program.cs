using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChatApi;
using OllamaSharp;
using OllamaSharp.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ConversationDb>(opt => opt.UseInMemoryDatabase("ConversationList"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ConversationService>();
var ollamaClient = new OllamaApiClient(new Uri("http://localhost:11434"));
builder.Services.AddSingleton(ollamaClient);

var app = builder.Build();


RouteGroupBuilder conversations = app.MapGroup("/conversation");
var conversationService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ConversationService>();
conversations.MapGet("/", conversationService.GetAllConversations);
conversations.MapGet("/{id}", conversationService.GetConversation);
conversations.MapPost("/", conversationService.CreateNewConversation);
conversations.MapPut("/{id}", conversationService.UpdateConversation);
conversations.MapPost("/{id}/message", conversationService.AddMessageToConversation);
conversations.MapPost("/ask/{id}", conversationService.AskQuestion);
conversations.MapPost("/suggest-title", conversationService.SuggestConversationTitleFromPrompt);

app.Run();

