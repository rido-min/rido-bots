using Microsoft.AspNetCore.Mvc;

namespace rido_ai_bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController(IConfiguration config) : ControllerBase
{
    public Task<string> Get() => Task.FromResult($"Settings: {Environment.NewLine} MicrosoftAppType={config.GetValue<string>("MicrosoftAppType")} {Environment.NewLine} MicrosoftAppId={config.GetValue<string>("MicrosoftAppId")} {Environment.NewLine}MicrosoftAppPassword={config.GetValue<string>("MicrosoftAppPassword")!.Substring(0,5)}");
}
