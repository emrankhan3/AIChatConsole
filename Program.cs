using Microsoft.SemanticKernel;
//using Microsoft.SemanticKernel.Orchestration;
//using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Planning;
using System.ComponentModel;
using Newtonsoft.Json;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using MattElandsBlog.plugins.EmailPlugins.EmailPlugin;
using MattElandsBlog.plugins.DBPlugins.SqlPlugin;


var builder = Kernel.CreateBuilder();


//builder.Services.AddLogging(c => c.SetMinimumLevel(LogLevel.Trace).AddDebug());

builder.Plugins.AddFromType<UserSQLPlugin>();
Kernel kernel = builder.Build();






////////////////////////////////////////////////////////////
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};
IChatCompletionService chatcompletionservice = kernel.GetRequiredService<IChatCompletionService>();

//var chatFunction = kernel.CreateFunctionFromPrompt(prompt,
//    executionSettings: new OpenAIPromptExecutionSettings { Maxtokens = 800, Temperature = 0.7, Top = 0.5 });

// Create chat history

ChatHistory chatMessages = new ChatHistory();
chatMessages.AddSystemMessage("""
    You are a friendly assistant who likes to follow the rules. You will complete required steps
    and request approval before taking any consequential actions. If the user doesn't provide
    enough information for you to complete a task, you will keep asking question untill you have enough information.
    You can use the get_schema function for the database schema, and make query if needed and use execute_query function for result. 
    never make a query that included a table name that not exists in the database
    """);

// Get the response from the AI
//var result = await chatCompletionService.GetChatMessageContentAsync(
//    chatMessages,
//    executionSettings: openAIPromptExecutionSettings,
//    kernel: kernel
//);
var API_KEY = "sk-proj-.........8Y";
while (true)
{
    // Get user input
    System.Console.Write("User > ");
    chatMessages.AddUserMessage(Console.ReadLine()!);

    OpenAIChatCompletionService chatCompletionService = new("gpt-3.5-turbo", API_KEY);
    // Get the chat completions
    //OpenAIPromptExecutionSettings openAIPromptExecutionSettings1 = new()
    //{
    //    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
    //};

    var result = await chatCompletionService.GetChatMessageContentAsync(
        chatMessages,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Stream the results
    Console.WriteLine("AI> "+result.Content);
    string fullMessage = result.ToString();
    //await foreach (var content in result)
    //{
    //    if (content.Role.HasValue)
    //    {
    //        System.Console.Write("Assistant > ");
    //    }
    //    System.Console.Write(content.Content);
    //    fullMessage += content.Content;
    //}
    //System.Console.WriteLine();

    // Add the message from the agent to the chat history
    chatMessages.AddAssistantMessage(fullMessage);
}
