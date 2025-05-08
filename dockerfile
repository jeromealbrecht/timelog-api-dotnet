# Utilise l'image officielle .NET pour build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copie les fichiers csproj et restaure les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copie le reste du code et build l'application
COPY . ./
RUN dotnet publish -c Release -o out

# Utilise l'image runtime pour exécuter l'app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose le port (Render détecte automatiquement, mais 8080 est la convention)
EXPOSE 8080

# Démarre l'application
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "TimeLog.dll"]