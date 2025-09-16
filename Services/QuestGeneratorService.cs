using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace im_bored.Services;

public class QuestGeneratorService
{
    private readonly Kernel _kernel;
    private ChatHistory _history;

    // Uses DI to obtain Kernel singleton
    public QuestGeneratorService(Kernel kernel)
    {
        _kernel = kernel;
        _history = new ChatHistory();

    }

    public async Task GenerateQuestAsync(string activityTitle, string userInput)
    {
        // Retrieve the chat completion service
        var chatService = _kernel.Services.GetRequiredService<IChatCompletionService>();

        // Prepare the prompt
        var prompt = $@"
You are a quest master. Create a fun and engaging quest description:
- Activity: {activityTitle}
- User input / goal: {userInput}

Make it feel like a small challenge or adventure that the user can complete.
";
        _history.AddUserMessage(prompt);

        // Call the AI model
        var result = await chatService.GetChatMessageContentsAsync(chatHistory:_history, kernel: _kernel);

        // Write the reply to console
        Console.WriteLine("Assistant > ");
        foreach (var chunk in result)
        {
            Console.Write(chunk.Content);
        }
    }

} // end of class