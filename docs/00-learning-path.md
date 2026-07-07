# Learning path

Follow these steps in order. Each lesson builds on the previous one. By the end you'll have a portfolio-ready pipeline project and interview stories to match.

## Your three proof pillars

This repo is designed around the three things hiring managers look for when you pivot to CI/CD:

1. **Resume positioning** — lead with release outcomes, not feature dev ([lesson 8](08-resume-positioning.md))
2. **One visible pipeline project** — this repo ([lessons 1–6](01-dotnet-app.md))
3. **Interview stories** — map real work + lab exercises to STAR format ([lesson 7](07-interview-stories.md))

## Recommended schedule

| Week | Focus | Time | Outcome |
|------|--------|------|---------|
| 1 | Lessons 1–2 | 3–4 hrs | Run app locally; understand test gates |
| 2 | Lesson 3 | 2–3 hrs | Build and run Docker image |
| 3 | Lessons 4–5 | 3–4 hrs | Push to GitHub; green CI/CD badges |
| 4 | Lesson 6 + Azure (optional) | 2–4 hrs | Real or simulated deploy |
| 5 | Lessons 7–8 | 2 hrs | Interview prep + resume update |

## How to use each lesson

Every doc follows the same structure:

1. **Goal** — what CI/CD skill you're proving
2. **Concept** — short theory (why it matters in production)
3. **Walkthrough** — commands and file references in this repo
4. **Check yourself** — questions interviewers ask
5. **Stretch goals** — optional upgrades when you're ready

## Before you start

Install:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 22+](https://nodejs.org/)
- [Git](https://git-scm.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (or Docker Engine on Linux)
- A [GitHub](https://github.com) account

Verify:

```bash
dotnet --version    # 8.x
node --version      # >=22
docker --version
git --version
```

## Next step

→ [Lesson 1: .NET app and release endpoints](01-dotnet-app.md)
