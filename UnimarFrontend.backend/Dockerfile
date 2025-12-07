# ============================================
# BASE ASP.NET 8
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5432

# ============================================
# INSTALAR POSTGRESQL
# ============================================
RUN apt-get update && \
    apt-get install -y postgresql postgresql-contrib && \
    rm -rf /var/lib/apt/lists/*

# Configurar banco
RUN pg_ctlcluster 15 main start && \
    psql --username=postgres -c "ALTER USER postgres WITH PASSWORD 'postgres';" && \
    psql --username=postgres -c "CREATE DATABASE livrosexpo;" && \
    pg_ctlcluster 15 main stop

# ============================================
# BUILD .NET 8
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["UnimarFrontend.backend/UnimarFrontend.backend.csproj", "UnimarFrontend.backend/"]
RUN dotnet restore "./UnimarFrontend.backend/UnimarFrontend.backend.csproj"

COPY . .
WORKDIR "/src/UnimarFrontend.backend"
RUN dotnet build "./UnimarFrontend.backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ============================================
# PUBLISH
# ============================================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UnimarFrontend.backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ============================================
# FINAL – COM APP + POSTGRES
# ============================================
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Script de inicialização
COPY <<EOF /app/start.sh
#!/bin/bash
# Inicia o PostgreSQL
echo "Inicializando PostgreSQL..."
service postgresql start

echo "PostgreSQL iniciado."

# Inicia a aplicação
echo "Iniciando aplicação .NET..."
exec dotnet UnimarFrontend.backend.dll
EOF

RUN chmod +x /app/start.sh

ENTRYPOINT ["/app/start.sh"]
