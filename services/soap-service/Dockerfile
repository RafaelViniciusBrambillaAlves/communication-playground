FROM mcr.microsoft.com/dotnet/sdk:10.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore SoapService.csproj

RUN dotnet publish SoapService.csproj \
    -c Release \
    -o /publish \
    --no-restore

###################################

FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /publish .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "SoapService.dll" ]