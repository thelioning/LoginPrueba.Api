# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar solución y proyecto
COPY LoginPrueba.Api.sln ./
COPY LoginPrueba.Api/*.csproj ./LoginPrueba.Api/

# Restaurar dependencias
RUN dotnet restore LoginPrueba.Api.sln

# Copiar todo el código
COPY LoginPrueba.Api/ ./LoginPrueba.Api/

# Publicar
RUN dotnet publish LoginPrueba.Api/LoginPrueba.Api.csproj -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "LoginPrueba.Api.dll"]







