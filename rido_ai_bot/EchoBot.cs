using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace rido_webapp_zero;

public class EchoBot : ActivityHandler
{
    static int counter = 0;
    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        var replyText = $"Echo [{counter++}]: {turnContext.Activity.Text} {turnContext.Activity.Text}";


        await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);

        if (turnContext.Activity.Text.ToLowerInvariant().Contains("weather"))
        {
            var cardAttachment = CreateAdaptiveCardAttachment(Path.Combine(".", "cards", "LargeWeatherCard.json"));
            await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
        }

    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        var welcomeText = $"Hello and welcome! EchoBot Version {ThisAssembly.NuGetPackageVersion}";
        foreach (var member in membersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            }
        }
    }

    private static Attachment CreateAdaptiveCardAttachment(string filePath)
    {
        var adaptiveCardJson = File.ReadAllText(filePath);
        var adaptiveCardAttachment = new Attachment()
        {
            ContentType = "application/vnd.microsoft.card.adaptive",
            Content = JsonConvert.DeserializeObject(adaptiveCardJson),
        };
        return adaptiveCardAttachment;
    }
}
