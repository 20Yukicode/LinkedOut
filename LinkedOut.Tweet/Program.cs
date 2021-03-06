using LinkedOut.Common.Config;
using LinkedOut.Common.Feign.Extension;
using LinkedOut.Common.Feign.Middleware;
using LinkedOut.Common.Helper;
using LinkedOut.Tweet.Config;
using SummerBoot.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasicModuleService()
    .AddModuleService()
    .AddSingleton(new AppSettingHelper(builder.Configuration));

builder.Services.AddSummerBoot();
builder.Services.AddSummerBootFeign(item =>
{
    item.AddNacos(builder.Configuration);
});

var app = builder.Build();

app.UseBasicModuleMiddleWare("Tweet");

app.Run();
