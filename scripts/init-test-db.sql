-- Optional manual seed for local Postgres (EF migrations are the source of truth).
-- Integration tests apply migrations automatically via PostgresWebApplicationFactory.

CREATE TABLE IF NOT EXISTS deployments (
    "Id" SERIAL PRIMARY KEY,
    "Environment" VARCHAR(64) NOT NULL,
    "Status" VARCHAR(32) NOT NULL,
    "DeployedAt" TIMESTAMPTZ NOT NULL
);

INSERT INTO deployments ("Environment", "Status", "DeployedAt")
SELECT 'staging', 'success', TIMESTAMPTZ '2026-01-15 10:00:00+00'
WHERE NOT EXISTS (SELECT 1 FROM deployments WHERE "Environment" = 'staging');

INSERT INTO deployments ("Environment", "Status", "DeployedAt")
SELECT 'production', 'success', TIMESTAMPTZ '2026-01-20 14:30:00+00'
WHERE NOT EXISTS (SELECT 1 FROM deployments WHERE "Environment" = 'production');
