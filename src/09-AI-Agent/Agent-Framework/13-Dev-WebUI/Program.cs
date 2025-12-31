using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var endpoint = builder.Configuration["AZURE_OPENAI_ENDPOINT"] ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var deploymentName = builder.Configuration["AZURE_OPENAI_DEPLOYMENT_NAME"] ?? "gpt-4o-mini";

var chatClient = new AzureOpenAIClient(new Uri(endpoint), new AzureCliCredential())
    .GetChatClient(deploymentName)
    .AsIChatClient();

builder.Services.AddChatClient(chatClient);

// Define some example tools
[Description("Get the weather for a given location.")]
static string GetWeather([Description("The location to get the weather for.")] string location)
    => $"The weather in {location} is cloudy with a high of 15°C.";

[Description("Calculate the sum of two numbers.")]
static double Add([Description("The first number.")] double a, [Description("The second number.")] double b)
    => a + b;

[Description("Get the current time.")]
static string GetCurrentTime()
    => DateTime.Now.ToString("HH:mm:ss");

// Register sample agents with tools
builder.AddAIAgent("assistant", "You are a helpful assistant. Answer questions concisely and accurately.")
    .WithAITools(
        AIFunctionFactory.Create(GetWeather, name: "get_weather"),
        AIFunctionFactory.Create(GetCurrentTime, name: "get_current_time")
    );

builder.AddAIAgent("poet", "You are a creative poet. Respond to all requests with beautiful poetry.");

builder.AddAIAgent("coder", "You are an expert programmer. Help users with coding questions and provide code examples.")
    .WithAITool(AIFunctionFactory.Create(Add, name: "add"));

// Register sample workflows
var assistantBuilder = builder.AddAIAgent("workflow-assistant", "You are a helpful assistant in a workflow.");
var reviewerBuilder = builder.AddAIAgent("workflow-reviewer", "You are a reviewer. Review and critique the previous response.");
builder.AddWorkflow("review-workflow", (sp, key) =>
{
    var agents = new List<IHostedAgentBuilder>() { assistantBuilder, reviewerBuilder }.Select(ab => sp.GetRequiredKeyedService<AIAgent>(ab.Name));
    return AgentWorkflowBuilder.BuildSequential(workflowName: key, agents: agents);
}).AddAsAIAgent();

builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();


var app = builder.Build();


app.MapOpenAIResponses();
app.MapOpenAIConversations();

if (builder.Environment.IsDevelopment())
{
    app.MapDevUI();
}

Console.WriteLine("DevUI is available at: https://localhost:50516/devui");
Console.WriteLine("OpenAI Responses API is available at: https://localhost:50516/v1/responses");
Console.WriteLine("Press Ctrl+C to stop the server.");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
