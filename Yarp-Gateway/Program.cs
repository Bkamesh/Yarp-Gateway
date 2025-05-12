using System.Text;
using Dotnet.Helper.Encryptions;
using Yarp.ReverseProxy.Transforms;
using dotnet.helper.Extensions;

var builder = WebApplication.CreateBuilder(args);

var Services = builder.Services;
var Configuration = builder.Configuration;

Configuration.AddEnvironmentVariables("Yarp_");

Services.CustomCorsOrigin("CrosPolicy",Configuration);

Services.AddPrometheusMonitoring();
Services.AddRedinessAndLivenessCheck();

Services.AddReverseProxy()
  .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(context =>
{
    context.AddRequestTransform(async requestContext =>
    {
        using var reader = new StreamReader(requestContext.HttpContext.Request.Body);
        var body = await reader.ReadToEndAsync();
        if (!string.IsNullOrEmpty(body))
        {
            var originalrequest = AESUtility.AESDecryption(body);
            var bytes = Encoding.UTF8.GetBytes(originalrequest);
            requestContext.HttpContext.Request.Body = new MemoryStream(bytes);
            requestContext.ProxyRequest.Content.Headers.ContentLength = bytes.Length;
        }
    });
});

var app = builder.Build();

app.UseCustomCors("CrosPolicy");

app.UsePrometheusMontoring();

app.UseHealthCheck();

app.UseRouting();

app.MapReverseProxy();

app.Run();




