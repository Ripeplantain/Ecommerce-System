FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Ecommerce.ApiGateway/Ecommerce.ApiGateway.csproj", "src/Ecommerce.ApiGateway/"]
RUN dotnet restore "src/Ecommerce.ApiGateway/Ecommerce.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/Ecommerce.ApiGateway"
