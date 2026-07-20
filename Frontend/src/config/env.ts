import { z } from 'zod';

const envSchema = z.object({
  VITE_API_URL: z.url(),
  VITE_APP_NAME: z.string().min(1),
  VITE_SIGNALR_URL: z.url(),
});

const result = envSchema.safeParse(import.meta.env);

if (!result.success) {
  alert(JSON.stringify(result.error.flatten(), null, 2));
  throw new Error(JSON.stringify(result.error.flatten(), null, 2));
}

export const env = result.data;