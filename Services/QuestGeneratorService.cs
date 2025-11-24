using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using im_bored.Models;
using System.Text;
using im_bored.Data;

namespace im_bored.Services;


/// <summary>
/// Provides functionality for creating AI-generated quest descriptions.
/// </summary>
public class QuestGeneratorService
{
    private readonly Kernel _kernel;
    private ChatHistory _history;


    public QuestGeneratorService(Kernel kernel)
    {
        _kernel = kernel;
        _history = new ChatHistory();
    }


    /// <summary>
    /// Retrieves the most recent response from the AI chat.
    /// </summary>
    /// <returns>A string representing the AI response.</returns>
    private async Task<string> RetrieveAIResponse()
    {

        var chatService = _kernel.Services.GetRequiredService<IChatCompletionService>();
        var response = await chatService.GetChatMessageContentsAsync(chatHistory: _history, kernel: _kernel);

        // Parse the response into a string
        var combinedText = new StringBuilder();
        foreach (var chunk in response)
        {
            combinedText.AppendLine(chunk.Content);
        }

        return combinedText.ToString();
    }


    /// <summary>
    /// Generates a curated quest description for a given activity.
    /// </summary>
    /// <param name="activity">The activity used to generate the description.</param>
    /// <param name="currentUser">The current user that is generating the description.</param>
    /// <returns>Returns the curated quest description as a string.</returns>
    public async Task<string> GenerateQuestDescriptionAsync(Activity activity, ApplicationUser currentUser)
    {
        // Prompt the AI to generate a description for the activity
        var prompt = $@"
        Your objective is to create a concise, clear, and fun activity
        description to expand on the given activity to give the user
        an idea of how to follow through with completing activity.
        
        Activity details:
        - Title = {activity.Title}
        - Type = {activity.Type}
        - Participants = {activity.Participants}
        - Price = {activity.Price}
        - Duration = {activity.ActivityDuration}
        - Kid Friendly? = {activity.kidFriendly}

        User details:
        - Postal code (Use only if valid, otherwise ignore) = [{currentUser.PostalCode}]
        
        Use the postal code to create location specific instructions if possible.
        Do not ask follow-up questions.";
        
        _history.AddUserMessage(prompt);
        var response = await RetrieveAIResponse();
        _history.Clear();
        return response;
    }

} // end of class