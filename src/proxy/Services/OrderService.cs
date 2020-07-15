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

  public override async Task<MakeOrderResponse> MakeOrder(MakeOrderRequest request, ServerCallContext context)
  {
    var headers = context.RequestHeaders;

    var microserviceName = headers.FirstOrDefault(e => e.Key == "microservicename").Value;
    var token = headers.FirstOrDefault(e => e.Key == "token").Value;

    Console.WriteLine($"Received... {microserviceName} is asking for permission with {token} token to make an order");

    var response = await CheckAuth(microserviceName, token, "makeOrder");

    if (response.IsSuccessStatusCode)
    {
      var handler = new HttpClientHandler();
      handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

      using (var channel = GrpcChannel.ForAddress("https://localhost:6001", new GrpcChannelOptions { HttpHandler = handler }))
      {
        var grpcClient = new OrderServiceClient(channel);

        return await grpcClient.MakeOrderAsync(request, headers);
      }
    }

    return new MakeOrderResponse { Success = false };
  }
}
