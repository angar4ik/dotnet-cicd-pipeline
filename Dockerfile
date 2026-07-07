# Multi-stage build: compile Angular + .NET, run in smaller runtime image.
# See docs/03-docker-containerization.md for a line-by-line walkthrough.

# --- Stage 1: Build Angular UI ---
FROM node:22-alpine AS ui-build
WORKDIR /ui

COPY src/ReleasePipeline.UI/package.json src/ReleasePipeline.UI/package-lock.json* ./
RUN npm ci

COPY src/ReleasePipeline.UI/ ./
RUN npm run build

# --- Stage 2: Build .NET API ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG GIT_SHA=unknown

COPY ReleasePipeline.sln ./
COPY src/ReleasePipeline.Api/ReleasePipeline.Api.csproj src/ReleasePipeline.Api/
COPY tests/ReleasePipeline.Api.Tests/ReleasePipeline.Api.Tests.csproj tests/ReleasePipeline.Api.Tests/

RUN dotnet restore ReleasePipeline.sln

COPY . .
RUN dotnet publish src/ReleasePipeline.Api/ReleasePipeline.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:GitSha=${GIT_SHA}

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ARG GIT_SHA=unknown
ENV ASPNETCORE_URLS=http://+:8080
ENV GIT_SHA=$GIT_SHA

EXPOSE 8080

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
COPY --from=ui-build /ui/dist/browser /app/wwwroot

HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ReleasePipeline.Api.dll"]
