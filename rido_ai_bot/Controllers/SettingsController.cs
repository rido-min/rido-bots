using Microsoft.AspNetCore.Mvc;

namespace rido_ai_bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController(IConfiguration config, IWebHostEnvironment  hosting) : ControllerBase
{
    string ObfuscatePwd(string pwd) => pwd.Length > 4 ? pwd.Substring(0, 4) + new string('*', pwd.Length - 4) : pwd;

    public Task<string> Get() => Task.FromResult($"Settings: {Environment.NewLine} " +
        $"HostName={Environment.MachineName} {Environment.NewLine} " +
        $"Environment={hosting.EnvironmentName} {Environment.NewLine} " +
        $"MicrosoftAppType={config.GetValue<string>("MicrosoftAppType")} {Environment.NewLine} " +
        $"MicrosoftAppId={config.GetValue<string>("MicrosoftAppId")} {Environment.NewLine} " +
        $"MicrosoftAppPassword={ObfuscatePwd(config.GetValue<string>("MicrosoftAppPassword")!)}");
}
