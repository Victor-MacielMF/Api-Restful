# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY Api/*.csproj ./Api/
COPY Api.Tests/*.csproj ./Api.Tests/

# Restaura as dependências
RUN dotnet restore ./Api/Api.csproj

# Copia todo o conteúdo da solução
COPY . .

# Publica a aplicação em modo Release
RUN dotnet publish ./Api/Api.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia os arquivos da publicação do build anterior
COPY --from=build /app/publish .

# Define a porta que será exposta
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de entrada
ENTRYPOINT ["dotnet", "Api.dll"]