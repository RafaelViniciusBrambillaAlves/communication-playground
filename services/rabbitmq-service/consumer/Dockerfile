# services/rabbitmq-consumer/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore RabbitMQService.slnx

RUN dotnet publish src/RabbitMQService.Worker/RabbitMQService.Worker.csproj \
    -c Release \
    -o /publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /publish .

ENTRYPOINT ["dotnet", "RabbitMQService.Worker.dll"]