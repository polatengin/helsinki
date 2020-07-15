using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace identity_provider
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async context =>
        {
          await context.Response.WriteAsync("Hello World!");
        });

        endpoints.MapGet("/auth/policy", async context => {
          var roles = "[{\"role\":\"Developer\",\"action\":\"Read\",\"resource\":\"CategoryTree\"},{\"role\":\"Developer\",\"action\":\"Read\",\"resource\":\"ContentManager\"},{\"role\":\"Developer\",\"action\":\"Read\",\"resource\":\"MediaManager\"},{\"role\":\"Developer\",\"action\":\"Read\",\"resource\":\"Product\"}]";

          await context.Response.WriteAsync(roles);
        });

        endpoints.MapPost("/auth/check", async context =>
        {
          var payload = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(context.Request.Body);

          var token = payload["token"];
          var action = payload["action"];

          Console.WriteLine($"Auth request received for {action} : {token}");
        });
      });
    }
  }
}
