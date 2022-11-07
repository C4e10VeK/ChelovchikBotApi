FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY bin/Release/net6.0 .
ENTRYPOINT ["dotnet", "ChelovchikBotApi.dll"]
