const AUTH_STORAGE_KEY = "talentsync.auth";

interface StoredAuth {
  token: string;
  refreshToken?: string;
}

export function getToken(): string | null {
  try {
    const auth = localStorage.getItem(AUTH_STORAGE_KEY);

    if (!auth) {
      return null;
    }

    const parsed: StoredAuth = JSON.parse(auth);

    return parsed.token ?? null;
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

export function clearAuth(): void {
  localStorage.removeItem(AUTH_STORAGE_KEY);
}