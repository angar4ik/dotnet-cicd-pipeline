# Lesson 2: Automated testing as quality gates

## Goal

Learn how **tests block bad releases** — the core job of CI pipelines.

## Concept: quality gates

A quality gate is a rule: *if this check fails, the pipeline stops and nothing deploys.*

Common gates:

| Gate | Tool in this project |
|------|----------------------|
| Unit / integration tests | `dotnet test` + xUnit |
| Build must succeed | `dotnet build` |
| (Stretch) Code coverage minimum | coverlet collector |
| (Stretch) Security scan | `dotnet list package --vulnerable` |

## Test project

```
tests/ReleasePipeline.Api.Tests/
├── HealthEndpointTests.cs
├── ReleaseInfoEndpointTests.cs
└── ReleasePipeline.Api.Tests.csproj
```

We use `WebApplicationFactory<Program>` — this spins up the real API in-memory and hits HTTP endpoints. That's an **integration test**, which is more valuable than testing controllers in isolation for pipeline demos.

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
