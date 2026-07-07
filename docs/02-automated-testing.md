# Lesson 2: Automated testing as quality gates

## Goal

Learn how **tests block bad releases** — the core job of CI pipelines.

## Concept: quality gates

A quality gate is a rule: *if this check fails, the pipeline stops and nothing deploys.*

Common gates:

| Gate | Tool in this project |
|------|----------------------|
| Unit / integration tests | `dotnet test` + xUnit |
| E2E browser tests | Playwright (separate workflow) |
| Build must succeed | `dotnet build` |
| (Stretch) Code coverage minimum | coverlet collector |
| (Stretch) Security scan | `dotnet list package --vulnerable` |

## Test project

```
tests/ReleasePipeline.Api.Tests/
├── HealthEndpointTests.cs
├── ReleaseInfoEndpointTests.cs
├── ReadyEndpointTests.cs
├── DeploymentsEndpointTests.cs      # requires Postgres
├── ReadyWithDatabaseTests.cs        # requires Postgres
├── PostgresWebApplicationFactory.cs
└── ReleasePipeline.Api.Tests.csproj
```

We use `WebApplicationFactory<Program>` — this spins up the real API in-memory and hits HTTP endpoints. That's an **integration test**, which is more valuable than testing controllers in isolation for pipeline demos.

Database-backed tests use a **Postgres service container on the GitHub runner** (see `.github/workflows/ci.yml`). Tests apply EF migrations and seed minimal rows automatically. Locally, start Postgres with `docker compose up postgres -d` and set `ConnectionStrings__Default`.

### Playwright E2E tests

```
tests/ReleasePipeline.UI.E2E/
├── tests/dashboard.spec.ts     # Browser smoke tests for the Angular UI
├── playwright.config.ts        # Chromium, port 5080, retries in CI
└── package.json
```

Four smoke tests verify the Angular dashboard renders correctly:
- Header is displayed
- Health badge shows `healthy`
- Release info card shows service name, version, git SHA, environment
- Deployments card is present

The E2E workflow (`.github/workflows/e2e.yml`) builds both Angular and .NET, starts the API,
then runs `npx playwright test` against the full app. Locally:

```bash
cd tests/ReleasePipeline.UI.E2E
npm ci
npx playwright install chromium
npx playwright test
```

## Run tests locally

```bash
dotnet test --configuration Release --verbosity normal
```

Expected output: all tests passed.

## Run with coverage

```bash
dotnet test \
  --configuration Release \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

Coverage files land in `TestResults/` — CI uploads these as artifacts.

## Break it on purpose (learning exercise)

1. Change `HealthEndpointTests` to expect status `"broken"` instead of `"healthy"`.
2. Run `dotnet test` — it fails.
3. Imagine this on a PR: **merge blocked**, no deploy.
4. Revert the change.

This is the story for interview question #3: *automated tests integrated before deployment.*

## CI wiring

See `.github/workflows/ci.yml`:

```yaml
- name: Test with coverage gate
  run: dotnet test ... --configuration Release
```

If tests fail, the job fails → PR checks red → CD does not run on broken code.

## Check yourself

- What's the difference between unit and integration tests here?
- When would you run tests in parallel shards?
- How do flaky tests hurt release confidence?

## Stretch goals

- Add a failing test job that must pass before merge (branch protection on GitHub)
- Enforce minimum 80% line coverage with a script
- Add a GitHub Actions job for `dotnet format --verify-no-changes`

## Next step

→ [Lesson 3: Docker containerization](03-docker-containerization.md)
