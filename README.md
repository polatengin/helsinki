# Helsinki Project

This project shows how to create gRPC Server and gRPC Client in .Net Core 5

Also, in this project you can find how to add middleman between microservices and check permission first.

So, middleman will act as a semi-transparent proxy, pass incoming requests to "other" microservice if the "caller" microservice has permission to do such request.

To run the solution, open 4 of your favorite terminals (mine is Windows Terminal) and run each .Net Core project in them;

To run Microservice 1

```bash
cd src/microservice-1

dotnet run
```

To run Auth Provider

```bash
cd src/auth-provider

dotnet run
```

To run middleman

```bash
cd src/proxy

dotnet run
```

To run Microservice 2

```bash
cd src/microservice-2

dotnet run
```
