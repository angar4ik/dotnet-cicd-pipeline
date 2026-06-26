# Lesson 6: Deployment (simulated + Azure)

## Goal

Understand the **last mile** of CI/CD: getting the container running in a target environment and proving it works.

## Deployment models

| Model | This project |
|-------|--------------|
| Simulated | Default — manifest artifact, no cloud cost |
| Azure App Service (containers) | Optional — matches many .NET shops |
| Kubernetes / AKS | Stretch goal |

## Simulated deploy (start here)

Already wired in `cd.yml` → job `deploy-simulated`.

**What it teaches:**

1. Deployment record (who, when, what image)
2. Artifact retention for audit
3. Post-deploy smoke test step (curl `/health`)

**Exercise:** After a CD run, download `deployment-manifest.json` and explain each field in an interview.

## Azure App Service setup

### 1. Create resources

```bash
# Login
az login

# Resource group
az group create --name rg-release-pipeline-lab --location eastus

# App Service plan (Linux, container)
az appservice plan create \
  --name plan-release-pipeline \
  --resource-group rg-release-pipeline-lab \
  --is-linux \
  --sku B1

# Web app
az webapp create \
  --name release-pipeline-YOURNAME \
  --resource-group rg-release-pipeline-lab \
  --plan plan-release-pipeline \
  --deployment-container-image-name ghcr.io/YOUR_USER/release-pipeline-api:latest
```

### 2. Configure container registry access

If GHCR package is private, add registry credentials in App Service → Deployment Center.

For public packages, App Service can pull directly.

### 3. GitHub secrets

Repo → Settings → Secrets and variables → Actions:

| Secret | Value |
|--------|-------|
| `AZURE_WEBAPP_NAME` | `release-pipeline-YOURNAME` |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Download from App Service → Get publish profile |
| `AZURE_WEBAPP_URL` | `https://release-pipeline-YOURNAME.azurewebsites.net` |

### 4. GitHub environment

Settings → Environments → Create `production`:

- Optional: required reviewers (simulates approval gate)
- Attach secrets scoped to production

### 5. Deploy

Actions → CD → Run workflow → `deploy_target: azure`

### 6. Verify

```bash
curl https://release-pipeline-YOURNAME.azurewebsites.net/health
curl https://release-pipeline-YOURNAME.azurewebsites.net/api/release-info
```

## Release process checklist (production pattern)

Use this mental model — it maps to Azure DevOps release pipelines too:

```
[ ] CI green on commit SHA
[ ] Image pushed to registry with SHA tag
[ ] Change ticket / approval (if required)
[ ] Deploy to staging
[ ] Smoke tests pass
[ ] Deploy to production
[ ] Post-deploy monitoring (5–15 min)
[ ] Rollback plan documented
```

## Interview story #2: release process change

Template for your "eliminated weekly production issues" story:

> **Situation:** Production releases caused repeat incidents — manual steps, no test gate before deploy.
>
> **Task:** Improve release reliability without slowing the team.
>
> **Action:** Introduced automated test gate in pipeline, standardized deploy checklist, added post-deploy smoke test on `/health`, coordinated with QA on release window.
>
> **Result:** Reduced repeat production issues from weekly to near-zero over 3 months.

Map your real Accounting Hub / financial workflow experience to this structure.

## Check yourself

- What's the difference between continuous **delivery** and **deployment**?
- When would you use deployment slots vs feature flags?
- How do you roll back a bad container deploy?

## Stretch goals

- Add Azure DevOps pipeline equivalent (`azure-pipelines.yml`)
- Deploy to Azure Container Apps instead of App Service
- Add Application Insights for deploy correlation

## Next step

→ [Lesson 7: Interview stories](07-interview-stories.md)
