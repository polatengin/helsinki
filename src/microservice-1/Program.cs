using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Order;

namespace microservice_1
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var random = new Random();

      var handler = new HttpClientHandler();
      handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

      Console.WriteLine("Caller initiated...");
      Console.WriteLine();

      using(var client = new HttpClient())
      {
        var response = await client.GetStringAsync("http://localhost:7000/auth/policy");

        Console.WriteLine($"You have following roles : {response}");
        Console.WriteLine();
      }

      while (true)
      {
        Console.WriteLine("Press <any> key to send a random gRPC message to proxy...");
        Console.ReadLine();

        using (var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions { HttpHandler = handler }))
        {
          var client = new OrderService.OrderServiceClient(channel);

          var headers = new Metadata();
          headers.Add("MicroserviceName", nameof(microservice_1));
          headers.Add("Token", Guid.NewGuid().ToString("N"));

          if (random.Next() * 10 > 5)
          {
            var request = new MakeOrderRequest { Id = random.Next(1000, 1000000), Price = random.NextDouble() * random.Next(1000, 10000), Product = Guid.NewGuid().ToString("N") };

            var response = await client.MakeOrderAsync(request, headers);

            Console.WriteLine($"Making Order Response: {response.Success}");
          }
          else
          {
            var request = new DeleteOrderRequest { Id = random.Next(1000, 1000000) };

            var response = await client.DeleteOrderAsync(request, headers);

            Console.WriteLine($"Deleting Order Response: {response.Success}");
          }
        }
      }
    }
  }
}
