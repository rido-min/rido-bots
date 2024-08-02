using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace rido_webapp_zero;

public class SKBot(IChatCompletionService chat) : ActivityHandler
{
    ChatHistory history = new();

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        history.AddUserMessage(turnContext.Activity.Text);
        ChatMessageContent result = await chat.GetChatMessageContentAsync(history, cancellationToken: cancellationToken);
        history.AddMessage(result.Role, result.Content!);
        await turnContext.SendActivityAsync(MessageFactory.Text(result.Content), cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        var welcomeText = $"Hello and welcome! SKBot Version {ThisAssembly.NuGetPackageVersion}";
        history.AddSystemMessage("You are an experienced Teams app developer");
        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }
    }
}
