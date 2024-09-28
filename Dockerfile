FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
FROM base AS final
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "AshamedApp.API.dll"]