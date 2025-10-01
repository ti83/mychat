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
ollamaClient.SelectedModel = "llama3";
builder.Services.AddSingleton(ollamaClient);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

});

var app = builder.Build();

app.UseCors();

RouteGroupBuilder conversations = app.MapGroup("/conversation");
var conversationService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ConversationService>();
conversations.MapGet("/", conversationService.GetAllConversations);
conversations.MapGet("/{id}", conversationService.GetConversation);
conversations.MapGet("/{id}/header", conversationService.GetConversationHeader);
conversations.MapPost("/", conversationService.CreateNewConversation);
conversations.MapPut("/{id}", conversationService.UpdateConversation);
conversations.MapPost("/{id}/message", conversationService.AddMessageToConversation);
conversations
    .MapPost("/ask/{id}", conversationService.AskQuestion)
    .WithRequestTimeout(TimeSpan.FromMinutes(5)); 
conversations.MapPost("/suggest-title", conversationService.SuggestConversationTitleFromPrompt);
conversations.MapDelete("/{id}", conversationService.DeleteConversation);

app.Run();

