# Lesson 3: Docker containerization

## Goal

Package the .NET app into an **immutable artifact** (container image) that runs the same everywhere: your laptop, CI, staging, production.

## Concept: why containers for CI/CD

| Without containers | With containers |
|--------------------|-----------------|
| "Works on my machine" | Same image SHA in every environment |
| Manual server setup | `docker pull` + run |
| Deploy = copy files | Deploy = roll out new image tag |

## Dockerfile walkthrough

Our `Dockerfile` uses a **multi-stage build**:

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  SDK stage  │ ──► │   publish   │ ──► │   runtime   │
│ dotnet restore│   │ dotnet publish│   │ aspnet:8.0  │
│ dotnet build  │   │  (Release)    │   │ + DLL only  │
└─────────────┘     └─────────────┘     └─────────────┘
```

**Stage 1 (`build`):** SDK image has compilers — large but needed to compile.

**Stage 2 (`runtime`):** ASP.NET runtime only — smaller attack surface, faster pulls.

### Layer caching trick

We copy `.csproj` files and `dotnet restore` **before** copying all source code. Docker caches layers — when only `.cs` files change, restore is skipped.

### Build arguments

```dockerfile
ARG GIT_SHA=unknown
ENV GIT_SHA=$GIT_SHA
```

CI passes `GIT_SHA=${{ github.sha }}` so `/api/release-info` shows the commit in deployed containers.

## Build and run

```bash
# Build image
docker build -t release-pipeline-api:local .

# Run container
docker run -p 8080:8080 -e GIT_SHA=local-dev release-pipeline-api:local

curl http://localhost:8080/health
```

## Docker Compose (local dev)

```bash
docker compose up --build
```

`docker-compose.yml` maps port 8080 and sets `GIT_SHA=local-docker`.

## Health check

```dockerfile
HEALTHCHECK CMD curl -f http://localhost:8080/health || exit 1
```

Orchestrators (Docker Compose, Kubernetes, Azure Container Apps) use this to know when the container is ready.

## Check yourself

- Why not use the SDK image in production?
- What's the difference between `EXPOSE` and `-p 8080:8080`?
- How would you scan images for CVEs? (Trivy, GHCR vulnerability alerts)

## Stretch goals

- Add a `.dockerignore` to exclude `bin/`, `obj/`, `TestResults/`
- Use `docker scout` or Trivy in CI
- Pin base images by digest instead of tag

## Next step

→ [Lesson 4: GitHub Actions CI](04-github-actions-ci.md)
