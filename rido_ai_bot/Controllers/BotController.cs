using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

namespace rido_ai_bot.Controllers;

[Route("api/messages")]
[ApiController]
public class BotController(IBotFrameworkHttpAdapter adapter, IBot bot) : ControllerBase
{
    [HttpPost, HttpGet]
    public Task PostAsync() => adapter.ProcessAsync(Request, Response, bot);
}
