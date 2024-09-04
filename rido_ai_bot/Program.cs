using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Bot.Builder.ApplicationInsights;
using Microsoft.Bot.Builder.Integration.ApplicationInsights.Core;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.SemanticKernel;
using rido_ai_bot;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>options.SerializerSettings.MaxDepth = 128 );

builder.Services
    .AddApplicationInsightsTelemetry()
    .AddSingleton<IBotTelemetryClient, BotTelemetryClient>()
    .AddSingleton<ITelemetryInitializer, OperationCorrelationTelemetryInitializer>()
    .AddSingleton<ITelemetryInitializer, TelemetryBotIdInitializer>()
    .AddSingleton<TelemetryInitializerMiddleware>()
    .AddSingleton<TelemetryLoggerMiddleware>()
    .AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>()
    .AddSingleton<IBotFrameworkHttpAdapter, InstrumentedBotAdapter>()
    .AddAzureOpenAIChatCompletion("gpt-4o", builder.Configuration["AOAI_ENDPOINT"]!.ToString(), builder.Configuration["AOAI_APIKEY"]!.ToString())
    .AddTransient<IBot, SKBot>();

var app = builder.Build();


app.MapGet("/hello", () =>
{
    string msg = $"Hello World! from {Environment.MachineName} with version {ThisAssembly.NuGetPackageVersion}  at {DateTime.UtcNow.ToString("O")}";
    Console.WriteLine(msg);
    Trace.TraceInformation(msg);
    return msg;
});


app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
