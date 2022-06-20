using LinkedOut.Common.Config;
using LinkedOut.Common.Feign.Middleware;
using LinkedOut.Common.Helper;
using LinkedOut.Recruitment.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasicModuleService()
    .AddModuleService()
    .AddSingleton(new AppSettingHelper(builder.Configuration));


var app = builder.Build();

app.UseMiddleware<FeignMiddleware>();
app.UseBasicModuleMiddleWare("Recruitment");

app.Run();