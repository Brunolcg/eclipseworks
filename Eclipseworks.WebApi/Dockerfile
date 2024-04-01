FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Eclipseworks.WebApi/Eclipseworks.WebApi.csproj", "Eclipseworks.WebApi/"]
COPY ["Eclipseworks.Application/Eclipseworks.Application.csproj", "Eclipseworks.Application/"]
COPY ["Eclipseworks.Core.Application/Eclipseworks.Core.Application.csproj", "Eclipseworks.Core.Application/"]
COPY ["Eclipseworks.Core.Domain/Eclipseworks.Core.Domain.csproj", "Eclipseworks.Core.Domain/"]
COPY ["Eclipseworks.Domain/Eclipseworks.Domain.csproj", "Eclipseworks.Domain/"]
COPY ["Eclipseworks.Core.WebApi/Eclipseworks.Core.WebApi.csproj", "Eclipseworks.Core.WebApi/"]
COPY ["Eclipseworks.Infrastructure/Eclipseworks.Infrastructure.csproj", "Eclipseworks.Infrastructure/"]
RUN dotnet restore "Eclipseworks.WebApi/Eclipseworks.WebApi.csproj"
COPY . .
WORKDIR "/src/Eclipseworks.WebApi"
RUN dotnet build "Eclipseworks.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Eclipseworks.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Eclipseworks.WebApi.dll"]
