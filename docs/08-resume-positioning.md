# Lesson 8: Resume positioning

## Goal

Reframe your experience for **CI/CD / Release Engineer** roles — not generic full-stack or feature dev.

## Lead with outcomes, not features

### Before (feature-focused)

> Built features for Accounting Hub using .NET and Azure. Worked on APIs and UI.

### After (release-focused)

> Release-focused engineer with 3+ years building and supporting Azure-based .NET systems and automated delivery workflows. Experience with GitHub pipelines, Azure DevOps, Docker, automated testing gates, and production release reliability in high-volume financial workflows.

Adjust years and tools to match your real background.

## Resume structure

### 1. Summary (3–4 lines)

Hit these keywords naturally:

- CI/CD, release management, deployment automation
- Azure, .NET, Docker, GitHub Actions / Azure DevOps
- Automated testing, quality gates, smoke tests
- Production reliability, incident reduction
- Financial / regulated environments (if true)

### 2. Skills section

Group intentionally:

**CI/CD & Release:** GitHub Actions, Azure DevOps, Docker, container registries, deployment slots, branch policies

**Platforms:** Azure App Service, Azure (broader), Linux runners

**Development:** C#, .NET 8, xUnit, REST APIs

**Practices:** trunk-based development, PR reviews, change management, on-call / incident response

### 3. Experience bullets — formula

Each bullet: **Action + tool + measurable outcome**

| Weak | Strong |
|------|--------|
| "Used Azure DevOps" | "Maintained Azure DevOps pipelines for 12 microservices; reduced failed deploys by 40%" |
| "Wrote tests" | "Integrated xUnit gates in release pipeline; blocked 15+ regressions pre-production in Q3" |
| "Deployed to Azure" | "Coordinated weekly releases across dev/QA/ops; cut repeat production incidents from weekly to rare" |

Rewrite 3–5 bullets from your current role using this formula.

### 4. Projects section — this repo

```
Release Pipeline Lab (GitHub)                    2026
• Reference CI/CD implementation: GitHub Actions, .NET 8, Docker, GHCR
• Automated build/test gates, container deploy, post-deploy smoke tests
• github.com/YOUR_USER/release-pipeline-lab
```

Link the public repo. Recruiters **will** click it.

## LinkedIn headline options

- `.NET Release Engineer | CI/CD, Azure, Docker | Building reliable delivery pipelines`
- `Software Engineer → CI/CD | Azure DevOps & GitHub Actions | Financial systems`

## What NOT to do

- Don't claim "5 years CI/CD" if most was feature dev — say "3+ years .NET/Azure with increasing ownership of release automation"
- Don't list 40 tools — depth on 5–6 beats breadth on 30
- Don't hide this lab project — it's your proof pillar #2

## Application strategy

| Role title | Emphasize |
|------------|-----------|
| CI/CD Engineer | Pipelines, gates, deploy automation |
| Release Engineer | Coordination, change management, reliability |
| DevOps Engineer | Infra + pipelines (if you have Azure infra experience) |
| Platform Engineer | Internal developer experience, golden paths |

Same stories, different emphasis.

## Checklist before applying

- [ ] Public GitHub repo with green Actions badge
- [ ] README explains pipeline design (this repo ✓)
- [ ] Resume summary is release-focused
- [ ] 4 STAR stories practiced (lesson 7)
- [ ] LinkedIn updated with repo link

## You are ready when

You can whiteboard this in 90 seconds:

```
PR → CI (build, test) → merge → CD (docker, push, deploy) → smoke test → monitor
```

And point to a real URL that does exactly that.

---

Back to [Learning path](00-learning-path.md) | [Project README](../README.md)
