# Lesson 4: GitHub Actions CI

## Goal

Automate **build + test on every push and PR** — the first half of "continuous integration."

## Concept: CI vs CD

| CI (Continuous Integration) | CD (Continuous Delivery/Deployment) |
|---------------------------|-------------------------------------|
| Merge frequently | Ship frequently |
| Automated build + test | Automated deploy to environments |
| Fast feedback on PRs | Requires CI green + approvals |

This lesson covers `.github/workflows/ci.yml`.

## Workflow triggers

```yaml
on:
  push:
    branches: [main]
  pull_request:
    branches: [main]
```

Runs on:
- Every PR targeting `main` (validates before merge)
- Every push to `main` (validates after merge)

## Job structure

```
checkout → setup-dotnet → restore → build → test → upload artifacts
```

### Key actions

| Action | Purpose |
|--------|---------|
| `actions/checkout@v4` | Clone repo into runner |
| `actions/setup-dotnet@v4` | Install .NET 8 SDK |
| `actions/upload-artifact@v4` | Save test results for debugging |

## Enable on your fork

1. Push repo to GitHub (see root README).
2. Go to **Actions** tab — workflows are enabled by default.
3. Open a PR with a small change — watch CI run.

## Branch protection (recommended)

Settings → Branches → Add rule for `main`:

- ✅ Require status checks to pass before merging
- ✅ Require branches to be up to date
- Select: **Build & Test** (from CI workflow)

Now PRs cannot merge with failing tests — a real quality gate.

## Debugging failed CI

1. Click the failed workflow run.
2. Expand the failed step (usually Test).
3. Read xUnit output — same as local `dotnet test`.
4. Fix locally, push again.

## Interview story #1: pipeline failure you diagnosed

Practice narrating:

> "A PR failed CI on `dotnet test`. The integration test for `/health` returned 503 because we added middleware that blocked the health route. I excluded `/health` from auth middleware and added a regression test. Pipeline green in 20 minutes, deploy unblocked."

## Check yourself

- What runs on `ubuntu-latest` vs `windows-latest` for .NET?
- How do you cache NuGet packages between runs? (`actions/setup-dotnet` cache option)
- What's the cost of a 15-minute CI pipeline on team velocity?

## Stretch goals

- Add NuGet caching: `cache: true` in setup-dotnet
- Matrix build: test on `ubuntu-latest` and `windows-latest`
- Add `concurrency` group to cancel outdated PR runs

## Next step

→ [Lesson 5: GitHub Actions CD](05-github-actions-cd.md)
