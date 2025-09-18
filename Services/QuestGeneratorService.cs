using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using im_bored.Models;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using im_bored.Data;

namespace im_bored.Services;

public class QuestGeneratorService
{
    private readonly Kernel _kernel;
    private ChatHistory _history;
    private UserManager<ApplicationUser> _userManager;

    // Uses DI to obtain Kernel singleton
    public QuestGeneratorService(
        Kernel kernel,
        UserManager<ApplicationUser> userManager)
    {
        _kernel = kernel;
        _history = new ChatHistory();
        _userManager = userManager;

    }

    public async Task<string> GenerateQuestDescriptionAsync(Activity activity, ClaimsPrincipal principal)
    {
        // Retrieve the ApplicationUser given the Claims Principal
        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return "";

        // Retrieve the chat completion service
        var chatService = _kernel.Services.GetRequiredService<IChatCompletionService>();

        // Prepare the prompt
        var prompt = $@"
        Your objective is to create a concise, clear, and fun activity
        description to expand on the given activity to give the user
        an idea of how to follow through with completing activity.
        
        Ensure that your reply is appropriate.
        
        Give a reply specific to this activity only, ignore previous activies.

        Activity details:
        - Title = {activity.Title}
        - Type = {activity.Type}
        - Participants = {activity.Participants}
        - Price = {activity.Price}
        - Duration = {activity.ActivityDuration}
        - Kid Friendly? = {activity.kidFriendly}

        User details:
        - Postal code = {user.PostalCode}
";
        _history.AddUserMessage(prompt);

        // Call the AI model
        var response = await chatService.GetChatMessageContentsAsync(chatHistory: _history, kernel: _kernel);

        // Parse the response into a string
        var combinedText = new StringBuilder();
        Console.WriteLine("Assistant > ");
        foreach (var chunk in response)
        {
            Console.Write(chunk.Content);
            combinedText.AppendLine(chunk.Content);
        }

        return combinedText.ToString();
    }

} // end of class