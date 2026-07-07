import { test, expect } from '@playwright/test';

test.describe('Release Pipeline Dashboard', () => {
  test('page loads and shows the header', async ({ page }) => {
    await page.goto('/');

    await expect(page.locator('h1')).toHaveText('🔄 Release Pipeline');
  });

  test('health check shows healthy status', async ({ page }) => {
    await page.goto('/');

    const healthBadge = page.locator('section.card:has(h2:text("Health")) .badge');

    await expect(healthBadge).toBeVisible({ timeout: 10_000 });
    await expect(healthBadge).toHaveText('healthy');
  });

  test('release info card shows service name and version', async ({ page }) => {
    await page.goto('/');

    const infoCard = page.locator('section.card:has(h2:text("Release Info"))');

    await expect(infoCard.locator('.info-value:text("ReleasePipeline.Api")')).toBeVisible({ timeout: 10_000 });
    await expect(infoCard.locator('.info-label:text("Service")')).toBeVisible();
    await expect(infoCard.locator('.info-label:text("Version")')).toBeVisible();
    await expect(infoCard.locator('.info-label:text("Git SHA")')).toBeVisible();
    await expect(infoCard.locator('.info-label:text("Environment")')).toBeVisible();
  });

  test('deployments card is present (empty or with data)', async ({ page }) => {
    await page.goto('/');

    const deploymentsCard = page.locator('section.card:has(h2:text("Deployments"))');

    await expect(deploymentsCard).toBeVisible({ timeout: 10_000 });
  });
});
