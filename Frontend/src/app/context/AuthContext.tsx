import {
  createContext,
  useCallback,
  useMemo,
  useState,
  type ReactNode,
} from "react";
import type { AuthUser } from "../types";

interface AuthContextType {
  user: AuthUser | null;
  login: (userData: AuthUser) => void;
  logout: () => void;
  isAuthenticated: boolean;
  isLoading: boolean;
}

const AUTH_STORAGE_KEY = "talentsync.auth";

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

function getStoredUser(): AuthUser | null {
  try {
    const stored = localStorage.getItem(AUTH_STORAGE_KEY);

    if (!stored) {
      return null;
    }

    const parsed: unknown = JSON.parse(stored);

    if (
      typeof parsed === "object" &&
      parsed !== null &&
      "token" in parsed &&
      typeof (parsed as AuthUser).token === "string"
    ) {
      return parsed as AuthUser;
    }

    localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<AuthUser | null>(getStoredUser);

  const login = useCallback((userData: AuthUser) => {
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(userData));
    setUser(userData);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    setUser(null);
  }, []);

  const value = useMemo<AuthContextType>(
    () => ({
      user,
      login,
      logout,
      isAuthenticated: user !== null,
      isLoading: false,
    }),
    [user, login, logout]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
