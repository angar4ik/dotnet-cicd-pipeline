# Lesson 7: Interview stories (STAR format)

Prepare four stories. Each should be 2–3 minutes spoken. Use **STAR**: Situation, Task, Action, Result.

Link every story to something in this repo when possible.

---

## Story 1: Pipeline failure you diagnosed and fixed

**Repo tie-in:** Break a test on purpose (lesson 2), fix it, reference the CI log.

### Template

| STAR | Your notes |
|------|------------|
| **Situation** | PR blocked, release train waiting |
| **Task** | Find root cause fast without bypassing gates |
| **Action** | Read CI logs → reproduce locally → fix → add regression test |
| **Result** | Pipeline green, deploy resumed, documented in runbook |

### Example (adapt to your experience)

> Our Azure DevOps pipeline failed on integration tests after a config change. I traced the failure to a missing connection string in the staging slot transform, reproduced it locally with the same test suite, fixed the transform, and added a pipeline validation step for required app settings. Deploy completed same day; no production impact.

---

## Story 2: Release process change that reduced repeat production issues

**Repo tie-in:** Quality gates in `ci.yml`, smoke test in `cd.yml`, deployment manifest.

### Template

| STAR | Your notes |
|------|------------|
| **Situation** | Repeat prod incidents after releases |
| **Task** | Make releases predictable |
| **Action** | Automated test gate, deploy checklist, smoke tests, QA sign-off window |
| **Result** | Fewer repeat incidents, faster mean time to recovery |

### Your anchor story

Use your real **"eliminated weekly production issues"** experience here. Quantify if you can:

- "Weekly → monthly → rare"
- "MTTR from 4 hours to 45 minutes"
- "Zero rollback events for 2 quarters"

---

## Story 3: Automated tests integrated before deployment

**Repo tie-in:** `dotnet test` in CI blocks CD; branch protection on GitHub.

### Template

| STAR | Your notes |
|------|------------|
| **Situation** | Bugs reached production because deploy didn't wait for tests |
| **Task** | Enforce test gate without killing velocity |
| **Action** | CI on every PR, required status check, parallel test jobs |
| **Result** | Defects caught pre-merge; deploy confidence up |

### Talking points

- Difference between "tests exist" and "tests gate deploy"
- Flaky test policy (quarantine, don't disable gates)
- Coverage vs meaningful assertions

---

## Story 4: Cross-team release coordination (dev + QA + ops)

**Repo tie-in:** GitHub Environments with approval; deployment manifest as audit trail.

### Template

| STAR | Your notes |
|------|------------|
| **Situation** | Release involved dev, QA, ops — unclear ownership |
| **Task** | Single coordinated release with clear handoffs |
| **Action** | Release calendar, shared checklist, comms channel, rollback owner |
| **Result** | On-time release, no finger-pointing during incident |

### Financial systems angle

Mention if applicable:

- Change windows / blackout periods
- Regulatory audit trail (who deployed what, when)
- Separation of duties (dev can't push to prod alone)

---

## Questions they'll ask you

Practice answers:

1. "Walk me through your CI/CD pipeline from commit to production."
2. "How do you handle a failed deployment?"
3. "CI is slow — what do you optimize first?"
4. "How do you manage secrets in pipelines?"
5. "What's the difference between GitHub Actions and Azure DevOps?"

**Answer #1 using this repo:**

> Developer opens PR → GitHub Actions runs `dotnet build` and `dotnet test` → branch protection requires green CI → merge to main → CD workflow builds Docker image, pushes to GHCR with commit SHA tag → simulated or Azure deploy runs → smoke test hits `/health` → deployment manifest archived.

---

## Next step

→ [Lesson 8: Resume positioning](08-resume-positioning.md)
