using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
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

  public override Task<MakeOrderResponse> MakeOrder(MakeOrderRequest request, ServerCallContext context)
  {
    var headers = context.RequestHeaders;

    var microserviceName = headers.FirstOrDefault(e => e.Key == "microservicename").Value;
    var token = headers.FirstOrDefault(e => e.Key == "token").Value;

    var payload = new {
      microserviceName = microserviceName,
      token = token
    };

    Console.WriteLine($"Received... {microserviceName} is making an order with {token} token");

    return Task.FromResult(new MakeOrderResponse { Success = true });
  }
}
