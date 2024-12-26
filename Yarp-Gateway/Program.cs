using System.Runtime.InteropServices.Marshalling;
using System.Text;
using Dotnet.Helper.Encryptions;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.VisualBasic;
using Polly;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
      options.AddPolicy("AllowOrigins", builder => 
      builder.WithOrigins("http://localhost:4200")
      .AllowAnyMethod()
      .AllowCredentials()
      .AllowAnyHeader()));

builder.Services.AddReverseProxy()
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
}); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

//app.UseRouting();

app.UseCors("AllowOrigins");

app.MapReverseProxy();
//app.MapControllers();

app.Run();




