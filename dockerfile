# Utilise l'image officielle .NET pour build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copie le reste du code et build l'application
COPY . .
RUN dotnet publish -c Release -o /app

# Utilise l'image runtime pour exécuter l'app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Expose le port (Render détecte automatiquement, mais 8080 est la convention)
#EXPOSE 8080

# Démarre l'application
#ENV ASPNETCORE_URLS=http://+:8080

# Si tu veux vraiment utiliser un user non-root, il faut le créer :
# RUN useradd -m appuser && chown -R appuser /app
# USER appuser

ENTRYPOINT ["dotnet", "TimeLog.dll"]