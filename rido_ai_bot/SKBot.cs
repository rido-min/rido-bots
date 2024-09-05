using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace rido_ai_bot;

public class SKBot(IChatCompletionService chat, IWebHostEnvironment env) : ActivityHandler
{
    ChatHistory history = new();
    static int counter = 0;
    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        if (env.IsProduction())
        {
            history.AddUserMessage(turnContext.Activity.Text);
            ChatMessageContent result = await chat.GetChatMessageContentAsync(history, cancellationToken: cancellationToken);
            history.AddMessage(result.Role, result.Content!);
            await turnContext.SendActivityAsync(MessageFactory.Text(result.Content), cancellationToken);
        }
        else
        {
            var replyText = $"Echo [{counter++}]: {turnContext.Activity.Text} {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
        }
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        var welcomeText = $"Hello and welcome! SKBot Version {ThisAssembly.NuGetPackageVersion}";

        if (env.IsProduction())
        {
            history.AddSystemMessage("You are an experienced Bot Framework developer");
            welcomeText += " AI enabled";
        }

        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }
    }
}
