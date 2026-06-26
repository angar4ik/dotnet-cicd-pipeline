# Lesson 1: .NET app and release endpoints

## Goal

Understand the **artifact** your pipeline delivers. CI/CD engineers don't just write YAML — they own what gets built, versioned, and deployed.

## Why this matters

In interviews, you should explain:

- What the service does
- How you know a deploy succeeded (`/health`, `/api/release-info`)
- What metadata you inject at build/deploy time (`GIT_SHA`, `DEPLOYED_AT`)

## Project structure

```
src/ReleasePipeline.Api/
├── Program.cs              # Minimal API + operational endpoints
├── appsettings.json
└── ReleasePipeline.Api.csproj
```

## Key endpoints

### `GET /health`

Liveness check used by load balancers, Kubernetes, and post-deploy smoke tests.

```bash
curl http://localhost:5080/health
# {"status":"healthy","timestamp":"..."}
```

### `GET /api/release-info`

Proves **which version** is running in an environment — essential when debugging "works in staging, broken in prod."

```bash
curl http://localhost:5080/api/release-info
```

Returns: service name, assembly version, environment, git SHA, deploy timestamp.

## Run locally

```bash
dotnet run --project src/ReleasePipeline.Api
```

Swagger UI (Development only): http://localhost:5080/swagger

## CI/CD connection

| Concern | How this app supports it |
|---------|---------------------------|
| Build | `dotnet build` compiles to DLL |
| Test | Integration tests call `/health` and `/api/release-info` |
| Container | `dotnet publish` output copied into Docker image |
| Deploy verify | Smoke test hits `/health` after deploy |

## Check yourself

- Why separate **liveness** (`/health`) from **version info** (`/api/release-info`)?
- What would you add for a financial system? (auth, audit logging, correlation IDs)
- How does `GIT_SHA` get into the container? (Hint: Docker build-arg → env var — see lesson 3)

## Stretch goals

- Add `AssemblyInformationalVersion` with git SHA at build time via MSBuild
- Add OpenTelemetry traces for request latency
- Add a `/ready` endpoint that checks downstream dependencies

## Next step

→ [Lesson 2: Automated testing as quality gates](02-automated-testing.md)
