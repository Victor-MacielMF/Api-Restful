# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura dependências
COPY Api/*.csproj ./Api/
COPY Api.Tests/*.csproj ./Api.Tests/
RUN dotnet restore ./Api/Api.csproj

# Copia o restante do código
COPY . .

# Publica a aplicação (build para produção)
RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos publicados da etapa anterior
COPY --from=build /app/publish .

# Variável de ambiente para ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando para rodar a aplicação
ENTRYPOINT ["dotnet", "Api.dll"]