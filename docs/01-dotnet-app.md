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

### `GET /ready`

Readiness check — verifies downstream dependencies before accepting traffic. Returns `503` when a configured dependency is unreachable.

```bash
curl http://localhost:5080/ready
# {"status":"ready","dependencies":[],"timestamp":"..."}
```

### `GET /api/release-info`

Proves **which version** is running in an environment — essential when debugging "works in staging, broken in prod."

```bash
curl http://localhost:5080/api/release-info
```

Returns: service name, assembly version, environment, git SHA, deploy timestamp.

### `GET /api/deployments`

Returns deployment records from Postgres when `ConnectionStrings:Default` is configured.

```bash
curl http://localhost:5080/api/deployments
# [{"id":1,"environment":"staging","status":"success","deployedAt":"..."}, ...]
```

When no connection string is set, this endpoint is not registered — the API still serves `/health` and `/api/release-info`.

## Run locally

**API only:**
```bash
dotnet run --project src/ReleasePipeline.Api
```

**With Angular UI:**
```bash
# Terminal 1: start API
ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/ReleasePipeline.Api

# Terminal 2: build Angular UI and start dev server
cd src/ReleasePipeline.UI
npm ci && npm run build
npm start
```

Then open **http://localhost:4200** (Angular dev server, proxies API calls) or
**http://localhost:5080** (API serves built UI from `wwwroot/`).

Swagger UI (Development only): http://localhost:5080/swagger
Dashboard UI (served by API): http://localhost:5080/

## CI/CD connection

| Concern | How this app supports it |
|---------|---------------------------|
| Build | `dotnet build` compiles to DLL; `npm run build` compiles Angular |
| Test | xUnit integration tests + Playwright E2E browser tests |
| Container | Multi-stage Dockerfile: Angular (node:22) + .NET SDK → runtime image |
| Deploy verify | Smoke test hits `/health` after deploy |

## Check yourself

- Why separate **liveness** (`/health`) from **version info** (`/api/release-info`)?
- What would you add for a financial system? (auth, audit logging, correlation IDs)
- How does `GIT_SHA` get into the container? (Hint: Docker build-arg → env var — see lesson 3)

## Stretch goals

These are implemented in the repo — read this section to understand **why** each one matters in production CI/CD.

### `AssemblyInformationalVersion` with git SHA (MSBuild)

**Problem:** `AssemblyVersion` (e.g. `1.0.0`) changes only when you bump the package. At deploy time you need to know *exactly* which commit is running — especially when debugging "staging works, prod doesn't."

**Solution:** MSBuild stamps the assembly at compile time:

| Source | Mechanism |
|--------|-----------|
| Local `dotnet build` | `git rev-parse --short HEAD` via an MSBuild target |
| CI | `/p:GitSha=${{ github.sha }}` in `.github/workflows/ci.yml` |
| Docker | `ARG GIT_SHA` → `/p:GitSha=${GIT_SHA}` in `Dockerfile` |

The informational version becomes `1.0.0+abc1234` (semver + build metadata). `/api/release-info` exposes both `version` and `informationalVersion`. The git SHA is also embedded as `AssemblyMetadata` so it survives even if the `GIT_SHA` env var is missing.

```bash
curl http://localhost:5080/api/release-info
# "informationalVersion": "1.0.0+a1b2c3d", "gitSha": "a1b2c3d"
```

**Interview line:** "We bake the commit SHA into the DLL at build time so release metadata is tied to the artifact, not just the runtime environment."

### OpenTelemetry traces for request latency

**Problem:** `/health` tells you the process is alive, not whether requests are slow or failing deep in the stack.

**Solution:** ASP.NET Core instrumentation creates a trace span per HTTP request (method, route, status, duration). Outgoing `HttpClient` calls (e.g. readiness probes) get child spans.

Configuration in `appsettings.json`:

```json
"OpenTelemetry": {
  "ServiceName": "ReleasePipeline.Api",
  "Exporter": "Console"
}
```

- **Console** (default): spans print to stdout — useful locally and in `docker logs`.
- **Otlp**: set `"Exporter": "Otlp"` and `"OtlpEndpoint": "http://your-collector:4317"` for Jaeger, Grafana Tempo, Azure Monitor, etc.

`/health` is excluded from tracing to avoid noise from kubelet probes every few seconds.

**Interview line:** "We instrument at the framework level so every endpoint gets latency and status without manual timers in each handler."

### `/ready` — readiness vs liveness

**Problem:** Kubernetes (and load balancers) distinguish:

| Probe | Question | This app |
|-------|----------|----------|
| **Liveness** (`/health`) | Is the process stuck/crashed? | Always `200` if the app runs |
| **Readiness** (`/ready`) | Can this instance accept traffic? | Checks downstream dependencies |

**Solution:** `Readiness:Dependencies` in config lists HTTP endpoints to probe before reporting ready:

```json
"Readiness": {
  "Dependencies": [
  {
    "Name": "payments-api",
    "Url": "https://payments.internal/health",
    "TimeoutSeconds": 3
  }
  ]
}
```

- Empty list → `200` with `"status": "ready"` (no external deps).
- Any dependency fails → `503` with per-dependency latency and error details.

```bash
curl http://localhost:5080/ready
# {"status":"ready","dependencies":[],"timestamp":"..."}
```

Wire this in Kubernetes as `readinessProbe` (not `livenessProbe`) so unhealthy instances are removed from the service pool without being restarted.

**Interview line:** "Liveness restarts the pod; readiness stops sending traffic. We never put dependency checks on liveness — a downstream outage would cause restart loops."

## Next step

→ [Lesson 2: Automated testing as quality gates](02-automated-testing.md)
