# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the .NET 6 runtime image for the base
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET 6 SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["BookShop.Web/BookShop.Web.csproj", "BookShop.Web/"]
RUN dotnet restore "BookShop.Web/BookShop.Web.csproj"
COPY . .
WORKDIR "/src/BookShop.Web"
RUN dotnet build "BookShop.Web.csproj" -c Release -o /app/build

# Publish the app in release mode
FROM build AS publish
RUN dotnet publish "BookShop.Web.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookShop.Web.dll"]