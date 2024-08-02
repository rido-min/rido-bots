using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.TraceExtensions;
using Microsoft.Bot.Connector.Authentication;

namespace rido_ai_bot;

public class InstrumentedBotAdapter : CloudAdapter
{
    public InstrumentedBotAdapter(BotFrameworkAuthentication auth, ILogger<InstrumentedBotAdapter> logger, TelemetryInitializerMiddleware telemetryInitializerMiddleware)
            : base(auth, logger)
    {
        Use(telemetryInitializerMiddleware);
        OnTurnError = async (turnContext, exception) =>
        {
            logger.LogError(exception, $"[OnTurnError] unhandled error : {exception.Message}");

            // Send a message to the user
            await turnContext.SendActivityAsync("The bot encountered an error or bug.");
            await turnContext.SendActivityAsync("To continue to run this bot, please fix the bot source code.");

            // Send a trace activity, which will be displayed in the Bot Framework Emulator
            await turnContext.TraceActivityAsync("OnTurnError Trace", exception.Message, "https://www.botframework.com/schemas/error", "TurnError");
        };
    }
}
