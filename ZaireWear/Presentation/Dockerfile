# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Business/Business.csproj", "Business/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Common/Core.csproj", "Common/"]

RUN dotnet restore "Presentation/Presentation.csproj"

COPY . .
WORKDIR "/src/Presentation"
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Serve static files if they exist
RUN mkdir -p /app/wwwroot
VOLUME ["/app/wwwroot"]

ENTRYPOINT ["dotnet", "Presentation.dll"]
