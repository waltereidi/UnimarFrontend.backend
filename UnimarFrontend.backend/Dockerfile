# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UnimarFrontend.backend/UnimarFrontend.backend.csproj", "UnimarFrontend.backend/"]
RUN dotnet restore "./UnimarFrontend.backend/UnimarFrontend.backend.csproj"
COPY . .
WORKDIR "/src/UnimarFrontend.backend"
RUN dotnet build "./UnimarFrontend.backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UnimarFrontend.backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Executa o update do EF Core
RUN dotnet ef database update \
    --project UnimarFrontend.backend/UnimarFrontend.backend.csproj \
    --startup-project UnimarFrontend.backend/UnimarFrontend.backend.csproj

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UnimarFrontend.backend.dll"]