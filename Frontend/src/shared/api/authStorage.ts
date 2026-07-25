import type { AuthUser } from "../../app/types";
const AUTH_STORAGE_KEY = "talentsync.auth";

interface StoredAuth {
  token: string;
  refreshToken?: string;
}

export function getAuth(): AuthUser | null {
  try {
    const auth = localStorage.getItem(AUTH_STORAGE_KEY);

    if (!auth) {
      return null;
    }

    const parsed: unknown = JSON.parse(auth);

    if (
      typeof parsed === "object" &&
      parsed !== null &&
      "token" in parsed &&
      typeof (parsed as AuthUser).token === "string"
    ) {
      return parsed as AuthUser;
    }

    clearAuth();
    return null;
  } catch {
    clearAuth();
    return null;
  }
}

export function saveAuth(user: AuthUser): void {
  localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(user));
}

export function getToken(): string | null {
  return getAuth()?.token ?? null;
}

export function clearAuth(): void {
  localStorage.removeItem(AUTH_STORAGE_KEY);
}