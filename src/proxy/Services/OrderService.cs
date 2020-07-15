using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Order;
using static Order.OrderService;

public class OrderService : OrderServiceBase
{
  private readonly ILogger<OrderService> _logger;

  public OrderService(ILogger<OrderService> logger)
  {
    _logger = logger;
  }

  private static async Task<HttpResponseMessage> CheckAuth(string microserviceName, string token, string action)
  {
    var payload = new
    {
      microserviceName = microserviceName,
      token = token,
      action = action
    };

    var client = new HttpClient();
    return await client.PostAsync("http://localhost:7000/auth/check", new StringContent(JsonSerializer.Serialize(payload)));
  }
}
