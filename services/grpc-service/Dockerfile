FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore GrpcService.slnx

RUN dotnet publish src/GrpcService.Api/GrpcService.Api.csproj \
    -c Release \
    -o /publish \
    --no-restore

###################################

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GrpcService.Api.dll"]